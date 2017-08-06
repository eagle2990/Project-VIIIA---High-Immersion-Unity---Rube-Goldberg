using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMarker : MonoBehaviour {

    //public Color validColor;
    //public Color invalidColor;
    public Material validMaterial;
    public Material invalidMaterial;

    public MeshRenderer markerMesh;
    public MeshRenderer markerIcon;

    public void SetValid()
    {
        markerMesh.material = validMaterial;
        markerIcon.material = validMaterial;
    }

    public void SetInvalid()
    {
        markerMesh.material = invalidMaterial;
        markerIcon.material = invalidMaterial;
    }
}
