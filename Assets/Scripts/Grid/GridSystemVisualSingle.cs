using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public void Show(Material material) { meshRenderer.enabled = true;  meshRenderer.material = material; }

    public void Hide()
    {
        meshRenderer.material.color = new Color(1, 1, 1, 0.07f);
    }

}