using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuActions : MonoBehaviour
{
    [SerializeField]
    private int Gameplay;
    [SerializeField]
    private int GUI;
    [SerializeField]
    private int Settings;
    [SerializeField]
    private int MainMenu;
    [SerializeField]
    private int PauseMenu;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartButton()
    {
        // Démarrage normal : activer l'oxygène
        PlayerPrefs.SetInt("DisableOxygenSystem", 0);
        PlayerPrefs.SetInt("SpawnPointIndex", 0); // Spawn par défaut
        SceneManager.LoadScene(Gameplay, LoadSceneMode.Single);
    }

    public void SettingsButton()
    {
        SceneManager.LoadScene(Settings, LoadSceneMode.Single);
    }

    public void ExitGameButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackButton()
    {
        SceneManager.LoadScene(MainMenu, LoadSceneMode.Single);
    }

    public void StartAtSpawnPoint(int i)
    {
        PlayerPrefs.SetInt("SpawnPointIndex", i);
        PlayerPrefs.SetInt("DisableOxygenSystem", 1); // Désactiver OxygenSystem
        Time.timeScale = 1;
        SceneManager.LoadScene(Gameplay, LoadSceneMode.Single);

    }

    public void CloseButton()
    {
        SceneManager.UnloadSceneAsync(PauseMenu);
    }
}
