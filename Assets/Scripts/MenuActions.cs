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
    public void StartButton()
    {
        SceneManager.LoadScene(Gameplay, LoadSceneMode.Single);
        SceneManager.LoadScene(GUI, LoadSceneMode.Additive);
    }

    public void SettingsButton()
    {
        SceneManager.LoadScene(Settings, LoadSceneMode.Single);
    }

    public void ExitGameButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void BackButton()
    {
        SceneManager.LoadScene(MainMenu, LoadSceneMode.Single);
    }
}
