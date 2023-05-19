using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private GridVisual _MyVisual;
    [SerializeField] private Outline outLine;

    public void Show(Material material)
    {
        _MyVisual.SetIsActivate(true);

        //if (outLine != null)
        //    outLine.enabled = true;
    }

    public void Hide()
    {
        _MyVisual.SetIsActivate(false);

        if (outLine != null)
            outLine.OutlineColor = new Color(0, 0, 0, 0);
    }

    public void SetGridOutLineColor(Color Color)
    {
        if (outLine != null)
        {
            outLine.ChangeOutLineColor(Color);
            print(Color);
        }
        
    }

}