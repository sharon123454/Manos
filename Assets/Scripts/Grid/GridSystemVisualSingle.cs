using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer box;
    [SerializeField] private Outline outLine;

    public void Show(Material material) { outLine.enabled = true; /*box.enabled = false;*/ meshRenderer.material = material; }

    public void Hide()
    {
        meshRenderer.material.color = new Color(1, 1, 1, 0.07f);
        box.enabled = true;
        outLine.enabled = false;
    }

    public void SetGridOutLineColor(Color Color)
    {
        outLine.ChangeOutLineColor(Color);
    }
}