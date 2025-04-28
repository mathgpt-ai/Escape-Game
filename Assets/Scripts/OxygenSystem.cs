using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenSystem : MonoBehaviour
{
    private OxygenBar oxygenBar;
    private HealthBar healthBar;
    private GameObject oxygenBarUI;

    [SerializeField] private float oxygenDecreaseRate = 0.05f; // oxyg�ne perdu par seconde
    [SerializeField] private float healthDecreaseRate = 1f;    // sant� perdue par seconde apr�s oxy vide
    [SerializeField] private int indexEnigmeTuyaux = 0; // l'index de l'�nigme Tuyaux dans GameManager.nomScenesEnigmes

    private bool oxygenDepleted = false;
    private bool puzzleCompleted = false;

    private void Start()
    {
        // Trouver automatiquement les r�f�rences dans toutes les sc�nes
        oxygenBar = FindObjectOfType<OxygenBar>();
        healthBar = FindObjectOfType<HealthBar>();

        if (oxygenBar != null)
        {
            oxygenBar.SetMaxOxygen(1f);
            oxygenBarUI = oxygenBar.transform.parent.gameObject; // r�cup�rer le parent (Panel) pour cacher apr�s
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(1f); // Assume que la vie commence � 1 aussi (peut ajuster ici si diff�rent)
        }
    }

    private void Update()
    {
        if (puzzleCompleted || oxygenBar == null || healthBar == null)
            return;

        // Descendre oxyg�ne en continu
        oxygenBar.SetOxygen(Mathf.Max(0f, oxygenBar.GetCurrentOxygen() - oxygenDecreaseRate * Time.deltaTime));

        if (oxygenBar.GetCurrentOxygen() <= 0f)
        {
            oxygenDepleted = true;
        }

        // Si plus d'oxyg�ne = descendre la sant�
        if (oxygenDepleted)
        {
            healthBar.SetHealth(Mathf.Max(0f, healthBar.GetCurrentHealth() - healthDecreaseRate * Time.deltaTime));
        }

        if (GameManager.EstEnigmeComplete(indexEnigmeTuyaux))
        {
            HandlePuzzleTuyauxCompleted();
        }
    }

    public void HandlePuzzleTuyauxCompleted()
    {
        puzzleCompleted = true;

        if (oxygenBar != null)
        {
            oxygenBar.SetOxygen(1f); // Remettre oxyg�ne � 1
            if (oxygenBarUI != null)
            {
                oxygenBarUI.SetActive(false); // Cacher l'UI
            }
        }
    }
}
