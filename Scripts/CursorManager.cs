using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    [Header("Normal")]
    [SerializeField] private Texture2D _normalCursor = null;
    [SerializeField] private Vector2 _normalHotSpot = Vector2.zero;

    private CursorMode _cursorMode = CursorMode.ForceSoftware;

    private void Start()
    {
        SetCursorToNormal();
    }

    public static void SetCursorToNormal()
    {
        Cursor.visible = true;
        Cursor.SetCursor(_instance._normalCursor, _instance._normalHotSpot, _instance._cursorMode);
    }

    public static void SetCursorToEmpty()
    {
        Cursor.visible = false;
    }
}
