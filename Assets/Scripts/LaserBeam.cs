using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform gunTip;

    public void DrawLaser(Vector3 targetPosition)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, targetPosition);
    }

    public void DisableLaser()
    {
        lineRenderer.enabled = false;
    }
}