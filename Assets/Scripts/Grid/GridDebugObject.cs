using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro debugText;
    private GridObject _gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this._gridObject = gridObject;
    }

    private void Update()
    {
        debugText.text = _gridObject.ToString();
    }

}