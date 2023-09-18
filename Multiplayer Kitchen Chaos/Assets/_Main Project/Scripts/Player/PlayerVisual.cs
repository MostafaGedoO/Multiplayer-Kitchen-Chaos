using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer headMesh;
    [SerializeField] private MeshRenderer bodyMesh;

    private Material material;

    private void Awake()
    {
        material = new Material(headMesh.material);
        headMesh.material = material;
        bodyMesh.material = material;   
    }

    public void SetPlayerColor(Color _color)
    {
        material.color = _color;
    }
}
