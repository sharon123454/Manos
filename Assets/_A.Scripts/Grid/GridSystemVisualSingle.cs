using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private GridVisual _MyVisual;
    [SerializeField] private Outline _Outline;

    public void Show(Color color)
    {
        if (_MyVisual)
        {
            _MyVisual.UpdateVisualGridColor(color);
            _MyVisual.ShowGridVisual();
        }

        if (_Outline)
            _Outline.OutlineColor = color;
    }

    public void Hide()
    {
        if (_MyVisual)
        {
            _MyVisual.UpdateVisualGridColor(Color.white);
            _MyVisual.HideGridVisual();
        }

        if (_Outline)
            _Outline.OutlineColor = new(0, 0, 0, 0);
    }

    //public void UpdateGridVisualSingle(Color color)
    //{
    //    if (_MyVisual)
    //        _MyVisual.UpdateVisualGridColor(color);

    //    if (_Outline)
    //        _Outline.OutlineColor = color;
    //}

}