using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private static string[] nomsScenesEnigmes = { "ÉnigmeTuyaux", "Énigme3", "Énigme4", "ÉnigmeLab" };
    [SerializeField]
    private GameObject porteDeFin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Supprime les doublons
        }
    }

    private void Start()
    {
        VerifierProgression();
    }

    public void VerifierProgression()
    {
        bool toutesFaites = true;

        for (int i = 0; i < nomsScenesEnigmes.Length; i++)
        {
            if (PlayerPrefs.GetInt("Enigme_" + i, 0) == 0)
            {
                toutesFaites = false;
                break;
            }
        }

        if (toutesFaites)
        {
            Debug.Log("Toutes les énigmes complétées !");
            if (porteDeFin != null)
            {
                porteDeFin.SetActive(true);
            }

        }
    }

    public static void MarquerEnigmeComplete(int i)
    {
        if (i >= 0 && i < nomsScenesEnigmes.Length)
        {
            PlayerPrefs.SetInt("Enigme_" + i, 1);
            PlayerPrefs.Save();

            Debug.Log($"Énigme {i} complétée !");

            // Vérifie s’il existe une instance valide avant d'appeler VerifierProgression()
            if (Instance != null)
            {
                Instance.VerifierProgression();
            }

            if (ChecklistManager.Instance != null)
            {
                ChecklistManager.Instance.MarquerEnigmeDone(i);
            }
        }
    }

    public static bool EstEnigmeComplete(int i)
    {
        return PlayerPrefs.GetInt("Enigme_" + i, 0) == 1;
    }

    public void ResetProgression()
    {
        for (int i = 0; i < nomsScenesEnigmes.Length; i++)
        {
            PlayerPrefs.DeleteKey("Enigme_" + i);
        }
        PlayerPrefs.Save();
    }
}
