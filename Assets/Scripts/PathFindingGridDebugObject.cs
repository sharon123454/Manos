using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathFindingGridDebugObject : GridDebugObject
{
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.gray : Color.black;
    }

}
