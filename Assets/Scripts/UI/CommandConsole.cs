using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConsole : MonoBehaviour
{
    public bool showConsole;
    private string input;

    public static UtilityCommand DEBUG;

    public List<object> commandList;
    // Start is called before the first frame update
    void Start()
    {
        showConsole = false;
    }

    public void Awake()
    {
        DEBUG = new UtilityCommand("debug", "Generate a box", "debug", () =>
        {
            Debug.Log("debug command was run");
        });

        commandList = new List<object>
        {
            DEBUG
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
           showConsole = !showConsole;
        }

        if (Input.GetKeyDown(KeyCode.Return) && showConsole)
        {
            Debug.Log("enter");
            HandleInput();
            input = "";
        }
    }

    private void OnGUI()
    {
        if (!showConsole) { return; }

        float y = Screen.height - 30;
        GUI.Box(new Rect(0,y,Screen.width,30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y+5f, Screen.width - 20f, 20f), input);
    }

    void HandleInput()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            UtilityCommandBase commandBase = commandList[i] as UtilityCommandBase;
            if (input.Contains(commandBase.commandID))
            {
                if (commandList[i] as UtilityCommand != null)
                {
                    (commandList[i] as UtilityCommand).Invoke();
                }
            }
        }
    }
}
