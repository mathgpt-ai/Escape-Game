using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    public float chargeAmount;
    private Renderer objRenderer;
    private Material objMaterial;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        objMaterial = objRenderer.material;

        chargeAmount = Random.Range(0, 2) == 0 ? -1f : 1f;

        UpdateGlow();
    }

    public float GetCharge()
    {
        return chargeAmount;
    }

    public void UpdateGlow()
    {
        Color glowColor = chargeAmount > 0 ? Color.red : Color.blue;
        objMaterial.EnableKeyword("_EMISSION");
        objMaterial.SetColor("_EmissionColor", glowColor * 0.5f);
    }
}
