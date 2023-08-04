using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }
    public List<Texture2D> Cursors;

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(Cursors[0], hotSpot, cursorMode);
    }
    public void SetClickableCursor()
    {
        Cursor.SetCursor(Cursors[1], hotSpot, cursorMode);
    }
    public void SetOptimalCursor()
    {
        Cursor.SetCursor(Cursors[2], hotSpot, cursorMode);
    }
    public void SetSubOptimalCursor()
    {
        Cursor.SetCursor(Cursors[3], hotSpot, cursorMode);
    }
    public void SetOutOfrangeCursor()
    {
        Cursor.SetCursor(Cursors[4], hotSpot, cursorMode);
    }

}