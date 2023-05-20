using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private GridVisual _MyVisual;
    [SerializeField] private Outline _Outline;

    public void Show(Material material)
    {
        //if (outLine != null)
        //    outLine.enabled = true;
    }

    public void Hide()
    {
        if (_Outline != null)
            _Outline.OutlineColor = new Color(0, 0, 0, 0);
    }

    public void SetGridOutLineColor(Color Color)
    {
        if (_Outline != null)
        {
            _Outline.ChangeOutLineColor(Color);
            //print(Color);
        }
        
    }

}