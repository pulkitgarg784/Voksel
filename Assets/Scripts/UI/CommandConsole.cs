﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConsole : MonoBehaviour
{
    public bool showConsole;
    private string input;

    public static UtilityCommand VOXEL;
    public static UtilityCommand<int> BOX;

    public List<object> commandList;
    // Start is called before the first frame update
    void Start()
    {
        showConsole = false;
    }

    public void Awake()
    {
        VOXEL = new UtilityCommand("voxel", "Generate a voxel", "voxel", () =>
        {
            PlaceCube.instance.createCube(0,0,0);
        });
        
        BOX = new UtilityCommand<int>("box", "Generate a box", "box <size>", (x) =>
        {
            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= x; j++)
                {
                    for (int k = 0; k <= x; k++)
                    {
                        PlaceCube.instance.createCube(i,j,k);
                    }
                }
            }
        });

        commandList = new List<object>
        {
            VOXEL,
            BOX
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
        string[] props = input.Split(' ');
        for (int i = 0; i < commandList.Count; i++)
        {
            UtilityCommandBase commandBase = commandList[i] as UtilityCommandBase;
            if (input.Contains(commandBase.commandID))
            {
                if (commandList[i] as UtilityCommand != null)
                {
                    (commandList[i] as UtilityCommand).Invoke();
                }
                else if (commandList[i] as UtilityCommand<int> != null)
                {
                    (commandList[i] as UtilityCommand<int>).Invoke(int.Parse(props[1]));
                }
            }
        }
    }
}
