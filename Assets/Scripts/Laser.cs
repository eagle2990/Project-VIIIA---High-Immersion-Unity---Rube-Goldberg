using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Material validTeleportMaterial;
    public Material invalidTeleportMaterial;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetValid()
    {
        lineRenderer.material = validTeleportMaterial;
    }

    public void SetInvalid()
    {
        lineRenderer.material = invalidTeleportMaterial;
    }

    public void SetBegining(Vector3 position)
    {
        if (position == null)
        {
            position = Vector3.zero;
        }
        lineRenderer.SetPosition(0, position);
    }

    public void SetEnd(Vector3 position)
    {
        if (position == null)
        {
            position = Vector3.forward;
        }
        lineRenderer.SetPosition(1, position);
    }

}
