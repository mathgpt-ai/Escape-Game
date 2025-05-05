using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OxygenSystem : MonoBehaviour
{
    //sliders
    [SerializeField] 
    private OxygenBar oxygenBar;
    [SerializeField] 
    private HealthBar healthBar;

    //vitesse de damage
    [SerializeField] 
    private float oxygenDepletionRate = 1f; // Par seconde
    [SerializeField] 
    private float healthDepletionRate = 2f; // Par seconde

    //scene de fin
    [SerializeField]
    private int sceneFinale = 0;

    private bool isOxygenEmpty = false;

    private void Start()
    {
        //initialise avec les valeurs max si nécessaire(sécurité)
        if (oxygenBar.GetCurrentOxygen() <= 0)
        {
            oxygenBar.SetOxygen(oxygenBar.GetMaxOxygen());
        }

        if (healthBar.GetCurrentHealth() <= 0)
        {
            healthBar.SetHealth(healthBar.GetMaxHealth());
        }
    }

    private void Update()
    {
        if (!isOxygenEmpty)
        {
            float currentO2 = oxygenBar.GetCurrentOxygen();
            currentO2 -= oxygenDepletionRate * Time.deltaTime;
            oxygenBar.SetOxygen(Mathf.Max(0, currentO2));

            if (currentO2 <= 0)
            {
                isOxygenEmpty = true;
            }
        }
        else
        {
            float currentHP = healthBar.GetCurrentHealth();
            currentHP -= healthDepletionRate * Time.deltaTime;
            healthBar.SetHealth(Mathf.Max(0, currentHP));

            if (currentHP <= 0)
            {
                SceneManager.LoadScene(sceneFinale);
            }
        }
    }
}
