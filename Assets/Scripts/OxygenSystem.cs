using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenSystem : MonoBehaviour
{
    private OxygenBar oxygenBar;
    private HealthBar healthBar;
    private GameObject oxygenBarUI;

    [SerializeField] private float oxygenDecreaseRate = 0.05f; // oxygène perdu par seconde
    [SerializeField] private float healthDecreaseRate = 1f;    // santé perdue par seconde après oxy vide
    [SerializeField] private int indexEnigmeTuyaux = 0; // l'index de l'énigme Tuyaux dans GameManager.nomScenesEnigmes

    private bool oxygenDepleted = false;
    private bool puzzleCompleted = false;

    private void Start()
    {
        // Trouver automatiquement les références dans toutes les scènes
        oxygenBar = FindObjectOfType<OxygenBar>();
        healthBar = FindObjectOfType<HealthBar>();

        if (oxygenBar != null)
        {
            oxygenBar.SetMaxOxygen(1f);
            oxygenBarUI = oxygenBar.transform.parent.gameObject; // récupérer le parent (Panel) pour cacher après
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(1f); // Assume que la vie commence à 1 aussi (peut ajuster ici si différent)
        }
    }

    private void Update()
    {
        if (puzzleCompleted || oxygenBar == null || healthBar == null)
            return;

        // Descendre oxygène en continu
        oxygenBar.SetOxygen(Mathf.Max(0f, oxygenBar.GetCurrentOxygen() - oxygenDecreaseRate * Time.deltaTime));

        if (oxygenBar.GetCurrentOxygen() <= 0f)
        {
            oxygenDepleted = true;
        }

        // Si plus d'oxygène = descendre la santé
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
            oxygenBar.SetOxygen(1f); // Remettre oxygène à 1
            if (oxygenBarUI != null)
            {
                oxygenBarUI.SetActive(false); // Cacher l'UI
            }
        }
    }
}
