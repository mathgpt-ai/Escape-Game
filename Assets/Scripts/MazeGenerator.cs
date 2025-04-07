using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; // Add this for UI elements
using UnityEngine.SceneManagement; // For scene reloading

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth; // Largeur du labyrinthe
    [SerializeField] private int _mazeDepth; // Profondeur du labyrinthe
    private MazeCell[,] _mazeGrid;
    [SerializeField] private float CellSize = 4f; // Changed to 4 to match your cell scale

    [SerializeField] private GameObject dragon1;
    [SerializeField] private GameObject dragon2;
    [SerializeField] private GameObject dragon3;
    [SerializeField] private GameObject door;
    [SerializeField] private Transform playerStartPosition; // Position where player starts/respawns

    // Timer related variables
    [SerializeField] private Text timerText; // Reference to the Text component that will display the timer
    [SerializeField] private float startingTime = 120f; // Starting time in seconds (2 minutes by default)
    [SerializeField] private AudioClip tickSound; // Sound to play every second
    [SerializeField] private AudioClip timeUpSound; // Sound to play when timer reaches zero
    [SerializeField] private AudioClip tenSecondsSound; // Sound to play when 10 seconds remain
    [SerializeField] private float resetDelay = 2f; // Delay before reloading the scene

    // Sound volume properties - these will be controlled by the audio settings script
    public static float TickSoundVolume { get; set; } = 1.0f;
    public static float TimeUpSoundVolume { get; set; } = 1.0f;
    public static float TenSecondsSoundVolume { get; set; } = 1.0f;

    private float timer; // Current timer value
    private int lastSecond = -1;
    private bool isTimerRunning = false;
    private bool isResetting = false;
    private bool tenSecondsWarningPlayed = false;
    private FirstPersonController playerController;
    private Rigidbody playerRigidbody;

    public static MazeGenerator Instance { get; private set; } // Singleton pattern

    private List<MazeCell> usedCells = new List<MazeCell>(); // Pour garder en mémoire les cellules utilisées
    private Dictionary<MazeCell, List<string>> usedWalls = new Dictionary<MazeCell, List<string>>(); // Pour éviter que deux dragons spawn sur le même mur

    void Awake()
    {
        // Set up singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Set default volumes if they haven't been set yet
        if (TickSoundVolume <= 0) TickSoundVolume = 0.5f;
        if (TimeUpSoundVolume <= 0) TimeUpSoundVolume = 1.0f;
        if (TenSecondsSoundVolume <= 0) TenSecondsSoundVolume = 0.8f;
    }

    void Start()
    {
        // Find player controller
        playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerRigidbody = playerController.GetComponent<Rigidbody>();

            // If playerStartPosition wasn't assigned, set it to player's current position
            if (playerStartPosition == null)
            {
                GameObject startPoint = new GameObject("PlayerStartPosition");
                startPoint.transform.position = playerController.transform.position;
                startPoint.transform.rotation = playerController.transform.rotation;
                playerStartPosition = startPoint.transform;
            }
        }
        else
        {
            Debug.LogWarning("FirstPersonController not found! Player respawn will not work.");
        }

        // Initialize timer
        timer = startingTime;
        UpdateTimerDisplay();

        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // 🔹 Création de la grille
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(
                    _mazeCellPrefab,
                    new Vector3(x * CellSize, 0, z * CellSize),
                    Quaternion.identity
                );
            }
        }
        StartCoroutine(DisableMiniMapAfterDelay());

        // Generate maze and verify it's solvable
        bool isSolvable = false;
        int attempts = 0;
        const int MAX_ATTEMPTS = 5;

        while (!isSolvable && attempts < MAX_ATTEMPTS)
        {
            // Clear previous generation if this is a retry
            if (attempts > 0)
            {
                ResetMazeCells();
            }

            // Generate the maze
            GenerateMazeWithRandomizedDFS();

            // Check if maze is solvable from start to end
            isSolvable = IsMazeSolvable();

            attempts++;
        }

        if (!isSolvable)
        {
            Debug.LogWarning("Could not generate a solvable maze after " + MAX_ATTEMPTS + " attempts. Using fallback maze generation.");
            ResetMazeCells();
            GenerateFallbackMaze(); // Generate a simpler, guaranteed-solvable maze
        }

        CreateEntryAndExit();
        SpawnDragons();

        // Start the timer after a 5-second delay
        StartCoroutine(StartTimerAfterDelay(5f));
    }

    // Check if the maze is solvable from start to end
    private bool IsMazeSolvable()
    {
        // Create a visited array
        bool[,] visited = new bool[_mazeWidth, _mazeDepth];

        // Start from the entry point (0, 0)
        return CanReachExit(0, 0, visited);
    }

    // Recursive method to check if we can reach the exit from a given cell
    private bool CanReachExit(int x, int z, bool[,] visited)
    {
        // Check if we're out of bounds or already visited
        if (x < 0 || x >= _mazeWidth || z < 0 || z >= _mazeDepth || visited[x, z])
        {
            return false;
        }

        // Check if we reached the exit
        if (x == _mazeWidth - 1 && z == _mazeDepth - 1)
        {
            return true;
        }

        // Mark current cell as visited
        visited[x, z] = true;

        // Check all four directions
        MazeCell currentCell = _mazeGrid[x, z];

        // Check right
        if (!currentCell.IsRightWallActive && CanReachExit(x + 1, z, visited))
        {
            return true;
        }

        // Check left
        if (!currentCell.IsLeftWallActive && CanReachExit(x - 1, z, visited))
        {
            return true;
        }

        // Check front
        if (!currentCell.IsFrontWallActive && CanReachExit(x, z + 1, visited))
        {
            return true;
        }

        // Check back
        if (!currentCell.IsBackWallActive && CanReachExit(x, z - 1, visited))
        {
            return true;
        }

        // No path found
        return false;
    }

    // Reset all cells to unvisited with all walls active
    private void ResetMazeCells()
    {
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                Destroy(_mazeGrid[x, z].gameObject);
                _mazeGrid[x, z] = Instantiate(
                    _mazeCellPrefab,
                    new Vector3(x * CellSize, 0, z * CellSize),
                    Quaternion.identity
                );
            }
        }
    }

    // Fallback method to generate a simple, guaranteed-solvable maze
    private void GenerateFallbackMaze()
    {
        // Create a simple path from start to finish
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z].Visit();
            }
        }

        // Create a winding path from start to finish
        int currentX = 0;
        int currentZ = 0;

        while (currentX < _mazeWidth - 1 || currentZ < _mazeDepth - 1)
        {
            if (currentX < _mazeWidth - 1 && (currentZ == _mazeDepth - 1 || UnityEngine.Random.value < 0.5f))
            {
                // Move right
                _mazeGrid[currentX, currentZ].ClearRightWall();
                _mazeGrid[currentX + 1, currentZ].ClearLeftWall();
                currentX++;
            }
            else if (currentZ < _mazeDepth - 1)
            {
                // Move forward
                _mazeGrid[currentX, currentZ].ClearFrontWall();
                _mazeGrid[currentX, currentZ + 1].ClearBackWall();
                currentZ++;
            }
        }

        // Add some random connections to make it more interesting
        for (int i = 0; i < (_mazeWidth * _mazeDepth) / 3; i++)
        {
            int x = UnityEngine.Random.Range(0, _mazeWidth);
            int z = UnityEngine.Random.Range(0, _mazeDepth);

            int direction = UnityEngine.Random.Range(0, 4);

            switch (direction)
            {
                case 0: // Right
                    if (x < _mazeWidth - 1)
                    {
                        _mazeGrid[x, z].ClearRightWall();
                        _mazeGrid[x + 1, z].ClearLeftWall();
                    }
                    break;
                case 1: // Left
                    if (x > 0)
                    {
                        _mazeGrid[x, z].ClearLeftWall();
                        _mazeGrid[x - 1, z].ClearRightWall();
                    }
                    break;
                case 2: // Front
                    if (z < _mazeDepth - 1)
                    {
                        _mazeGrid[x, z].ClearFrontWall();
                        _mazeGrid[x, z + 1].ClearBackWall();
                    }
                    break;
                case 3: // Back
                    if (z > 0)
                    {
                        _mazeGrid[x, z].ClearBackWall();
                        _mazeGrid[x, z - 1].ClearFrontWall();
                    }
                    break;
            }
        }
    }

    void Update()
    {
        // Update the timer if it's running
        if (isTimerRunning && !isResetting)
        {
            // Countdown timer
            timer -= Time.deltaTime;

            // Check if timer has ended
            if (timer <= 0)
            {
                timer = 0;
                StopTimer();
                OnTimerEnd();
            }
            // Check if timer is at 10 seconds
            else if (timer <= 10f && !tenSecondsWarningPlayed)
            {
                PlayTenSecondsSound();
                tenSecondsWarningPlayed = true;
            }

            UpdateTimerDisplay();

            // Check if a new second has passed to play the tick sound
            int currentSecond = Mathf.FloorToInt(timer);
            if (currentSecond != lastSecond)
            {
                PlayTickSound();
                lastSecond = currentSecond;
            }
        }
    }

    // Play the tick sound at the specified position
    private void PlayTickSound()
    {
        if (tickSound != null)
        {
            // Play the sound at the camera position for best audio experience
            AudioSource.PlayClipAtPoint(tickSound, Camera.main.transform.position, TickSoundVolume);
        }
    }

    // Play the 10 seconds warning sound
    private void PlayTenSecondsSound()
    {
        if (tenSecondsSound != null)
        {
            AudioSource.PlayClipAtPoint(tenSecondsSound, Camera.main.transform.position, TenSecondsSoundVolume);
            Debug.Log("10 seconds remaining!");
        }
    }

    // Format and display the timer
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Make the timer text flash red when 10 seconds or less remain
            if (timer <= 10f)
            {
                // Use a sine wave to oscillate the alpha value for a flashing effect
                float alpha = Mathf.Sin(Time.time * 10f) * 0.5f + 0.5f;
                timerText.color = new Color(1f, 0f, 0f, alpha);
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }

    // Called when timer reaches zero
    private void OnTimerEnd()
    {
        if (isResetting) return; // Prevent multiple calls

        isResetting = true;
        Debug.Log("Time's up!");

        // Play the time up sound
        if (timeUpSound != null)
        {
            AudioSource.PlayClipAtPoint(timeUpSound, Camera.main.transform.position, TimeUpSoundVolume);
        }

        // Wait before reloading the scene
        StartCoroutine(ReloadSceneAfterDelay(resetDelay));
    }

    // Reload the current scene after a delay
    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Move player back to start position
        RespawnPlayer();

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Respawn the player at the starting position
    private void RespawnPlayer()
    {
        if (playerController != null && playerStartPosition != null)
        {
            // Stop all player movement
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.angularVelocity = Vector3.zero;
            }

            // Move player back to start
            playerController.transform.position = playerStartPosition.position;
            playerController.transform.rotation = playerStartPosition.rotation;

            Debug.Log("Player respawned at starting position");
        }
    }

    // Coroutine to start the timer after a delay
    private IEnumerator StartTimerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartTimer();
    }

    // Public methods to control the timer
    public void StartTimer()
    {
        isTimerRunning = true;
        tenSecondsWarningPlayed = false;
        Debug.Log("Timer started!");
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer stopped at: " + timer.ToString("F2") + " seconds");
    }

    public void ResetTimer()
    {
        timer = startingTime;
        tenSecondsWarningPlayed = false;
        UpdateTimerDisplay();
    }

    // Set a new duration for the timer
    public void SetTimerDuration(float seconds)
    {
        startingTime = seconds;
        timer = seconds;
        tenSecondsWarningPlayed = timer <= 10f;
        UpdateTimerDisplay();
    }

    // Add time to the current timer
    public void AddTime(float seconds)
    {
        timer += seconds;
        // Reset the 10 seconds warning if we're now above 10 seconds
        if (timer > 10f)
        {
            tenSecondsWarningPlayed = false;
        }
        UpdateTimerDisplay();
    }

    // Get the current timer value (useful for other scripts)
    public float GetTimerValue()
    {
        return timer;
    }

    // 🔹 Génération du labyrinthe avec DFS
    void GenerateMazeWithRandomizedDFS()
    {
        Stack<MazeCell> stack = new Stack<MazeCell>();
        MazeCell startCell = _mazeGrid[0, 0];
        startCell.Visit();
        stack.Push(startCell);

        while (stack.Count > 0)
        {
            MazeCell currentCell;

            // 20% de chance de piger une cellule aléatoire dans la pile
            if (UnityEngine.Random.value < 0.2f && stack.Count > 1)
            {
                int randomIndex = UnityEngine.Random.Range(0, stack.Count - 1);
                currentCell = stack.ElementAt(randomIndex);
                stack = new Stack<MazeCell>(stack.Where(c => c != currentCell));
            }
            else
            {
                currentCell = stack.Peek();
            }

            MazeCell nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                ClearWalls(currentCell, nextCell);
                nextCell.Visit();
                stack.Push(nextCell);
            }
            else
            {
                stack.Pop();
            }
        }
    }

    // 🔹 Récupère une cellule non visitée aléatoirement
    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).OrderBy(_ => UnityEngine.Random.Range(1, 10)).ToList();
        return unvisitedCells.FirstOrDefault();
    }

    // 🔹 Vérifie les cellules adjacentes non visitées
    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)(currentCell.transform.position.x / CellSize);
        int z = (int)(currentCell.transform.position.z / CellSize);

        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited) yield return _mazeGrid[x + 1, z];
        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited) yield return _mazeGrid[x - 1, z];
        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited) yield return _mazeGrid[x, z + 1];
        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited) yield return _mazeGrid[x, z - 1];
    }

    // 🔹 Enlève les murs entre les cellules adjacentes
    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        Vector3 prevPos = previousCell.transform.position;
        Vector3 currPos = currentCell.transform.position;

        if (prevPos.x < currPos.x) { previousCell.ClearRightWall(); currentCell.ClearLeftWall(); }
        else if (prevPos.x > currPos.x) { previousCell.ClearLeftWall(); currentCell.ClearRightWall(); }
        else if (prevPos.z < currPos.z) { previousCell.ClearFrontWall(); currentCell.ClearBackWall(); }
        else if (prevPos.z > currPos.z) { previousCell.ClearBackWall(); currentCell.ClearFrontWall(); }
    }

    // 🔹 Ajoute une entrée et une sortie
    private void CreateEntryAndExit()
    {
        _mazeGrid[0, 0].ClearBackWall();
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].ClearFrontWall();
        Vector3 end = new Vector3(_mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.x,
                                  _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.y,
                                  _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position.z + 2);
        Instantiate(door, end, Quaternion.identity);
    }

    // 🔹 Spawn des dragons
    private void SpawnDragons()
    {
        GameObject[] dragons = { dragon1, dragon2, dragon3 };
        int attempts = 0;

        for (int i = 0; i < dragons.Length; i++)
        {
            MazeCell randomCell;
            List<string> availableWalls;

            do
            {
                randomCell = GenerateRandomCell();
                availableWalls = GetAvailableWalls(randomCell);
                attempts++;

                if (attempts > 1000)
                {
                    Debug.LogWarning("Failed to place all dragons due to wall limitations.");
                    return;
                }

            } while (usedCells.Contains(randomCell) || availableWalls.Count == 0);

            usedCells.Add(randomCell);
            InstantiateDragon(randomCell, dragons[i]);
        }

        if (usedCells.Count < 3)
        {
            Debug.LogError($"Only {usedCells.Count} dragons were placed instead of 3.");
        }
    }

    private List<string> GetAvailableWalls(MazeCell cell)
    {
        List<string> availableWalls = new List<string>();
        int cellX = (int)(cell.transform.position.x / CellSize);
        int cellY = (int)(cell.transform.position.z / CellSize);

        if (cell.IsBackWallActive && cellY > 0) availableWalls.Add("Back");
        if (cell.IsFrontWallActive && cellY < _mazeDepth - 1) availableWalls.Add("Front");
        if (cell.IsLeftWallActive && cellX > 0) availableWalls.Add("Left");
        if (cell.IsRightWallActive && cellX < _mazeWidth - 1) availableWalls.Add("Right");

        return availableWalls;
    }

    // 🔹 Place un dragon sur un mur disponible
    private void InstantiateDragon(MazeCell cell, GameObject dragonPrefab)
    {
        List<string> freeWalls = GetAvailableWalls(cell);
        if (freeWalls.Count == 0) return; // Fixed the syntax error from .0 to 0

        string chosenWall = freeWalls[UnityEngine.Random.Range(0, freeWalls.Count)];
        usedWalls[cell] = usedWalls.ContainsKey(cell) ? usedWalls[cell] : new List<string>();
        usedWalls[cell].Add(chosenWall);

        switch (chosenWall)
        {
            case "Back": Instantiate(dragonPrefab, cell.transform.position + new Vector3(0, 1, -1.5f), Quaternion.Euler(90, 0, 0)); break;
            case "Front": Instantiate(dragonPrefab, cell.transform.position + new Vector3(0, 1, 1.5f), Quaternion.Euler(90, 180, 0)); break;
            case "Left": Instantiate(dragonPrefab, cell.transform.position + new Vector3(-1.5f, 1, 0), Quaternion.Euler(90, 90, 0)); break;
            case "Right": Instantiate(dragonPrefab, cell.transform.position + new Vector3(1.5f, 1, 0), Quaternion.Euler(90, -90, 0)); break;
        }
    }

    private MazeCell GenerateRandomCell()
    {
        return _mazeGrid[UnityEngine.Random.Range(0, _mazeWidth), UnityEngine.Random.Range(0, _mazeDepth)];
    }

    private IEnumerator DisableMiniMapAfterDelay()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        if (MiniMap.Instance != null)
        {
            MiniMap.Instance.DisableMiniMap();
        }
    }
}