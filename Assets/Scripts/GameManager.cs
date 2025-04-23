using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private static string[] nomsScenesEnigmes = {"�nigmeTuyaux", "�nigme3", "�nigme4", "�nigmeLab"};
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
            Debug.Log("Toutes les �nigmes compl�t�es !");
            if (porteDeFin != null)
            {
                porteDeFin.SetActive(true);
            }
                
        }
    }

    public static void MarquerEnigmeComplete(int index)
    {
        if (index >= 0 && index < nomsScenesEnigmes.Length)
        {
            PlayerPrefs.SetInt("Enigme_" + index, 1);
            PlayerPrefs.Save();

            Debug.Log($"�nigme {index + 1} compl�t�e !");

            // V�rifie s�il existe une instance valide avant d'appeler VerifierProgression()
            if (Instance != null)
            {
                Instance.VerifierProgression();
            }
        }
    }

    public bool EstEnigmeComplete(int index)
    {
        return PlayerPrefs.GetInt("Enigme_" + index, 0) == 1;
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
