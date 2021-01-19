using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorManager : MonoBehaviour
{
    public static cursorManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public enum CursorType
    {
        Default,
        Orbit,
        Zoom,
        Pan
    }
    [System.Serializable]
    public class CustomCursor
    {
        public CursorType cursorType;
        public Texture2D texture;
        
    }

    [SerializeField]private List<CustomCursor> customCursorList;
    private CustomCursor customCursor;

    private Vector2 cursorOffset;
    // Start is called before the first frame update
    void Start()
    {
        setCursor(CursorType.Pan);
    }

    public void setCursor(CursorType customCursor)
    {
        
        this.customCursor = getCursor(customCursor);
    }

    private CustomCursor getCursor(CursorType cursorType)
    {
        foreach (CustomCursor cursor in customCursorList)
        {
            if (cursor.cursorType == cursorType)
            {
                return cursor;
            }
        }

        return null;
    }
    

    // Update is called once per frame
    void Update()
    {

        if (customCursor != null)
        {
            cursorOffset = new Vector2(10,10);

            //Sets the cursor to the Crosshair sprite with given offset 
            //and automatic switching to hardware default if necessary
            Cursor.SetCursor(customCursor.texture, cursorOffset, CursorMode.Auto);

        }
    }
}
