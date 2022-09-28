using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager instance;

    [Header("Normal")]
    [SerializeField] private Texture2D normalCursor = null;
    [SerializeField] private Vector2 normalHotSpot = Vector2.zero;

    private CursorMode cursorMode = CursorMode.ForceSoftware;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        setCursorToNormal();
    }

    public static void setCursorToNormal()
    {
        Cursor.visible = true;
        Cursor.SetCursor(instance.normalCursor, instance.normalHotSpot, instance.cursorMode);
    }

    public static void SetCursorToEmpty()
    {
        Cursor.visible = false;
    }
}
