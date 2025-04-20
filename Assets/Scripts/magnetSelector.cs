using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSelector : MonoBehaviour
{
    public static MagnetsObjects selectedMagnet = null; // 🌟 Aimant actif
    private static Material defaultMaterial;
    private static Material selectedMaterial;

    void Start()
    {
        // 🎨 Définir les couleurs
        defaultMaterial = new Material(Shader.Find("Standard"));
        defaultMaterial.color = Color.white;

        selectedMaterial = new Material(Shader.Find("Standard"));
        selectedMaterial.color = Color.yellow; // 🟡 Couleur sélectionnée
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MagnetsObjects magnet = hit.collider.GetComponent<MagnetsObjects>();
                if (magnet != null)
                {
                    if (selectedMagnet != null)
                    {
                        // 🔄 Réinitialiser l'ancien aimant
                        selectedMagnet.SetMaterial(defaultMaterial);
                    }

                    selectedMagnet = magnet;
                    selectedMagnet.SetMaterial(selectedMaterial); // 🌟 Appliquer couleur
                    Debug.Log($"🧲 Aimant sélectionné : {magnet.gameObject.name}");
                }
            }
        }
    }
}
