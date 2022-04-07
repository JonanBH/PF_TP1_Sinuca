using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGauge : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    
    public void StartGauge(Vector3 position)
    {
        lineRenderer.gameObject.SetActive(true);
        lineRenderer.SetPosition(0, position);
        lineRenderer.SetPosition(1, position);
    }

    public void UpdateGauge(Vector3 position)
    {
        lineRenderer.SetPosition(1, position);
    }

    public void HideRenderer()
    {
        lineRenderer.gameObject.SetActive(false);
    }
}
