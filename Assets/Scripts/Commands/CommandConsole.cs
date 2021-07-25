using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConsole : MonoBehaviour
{
    public bool showConsole;
    private string input;

    public static UtilityCommand VOXEL;
    public static UtilityCommand<int,Vector3> BOX;
    public static UtilityCommand<int,Vector3> LINE;


    public List<object> commandList;
    // Start is called before the first frame update
    void Start()
    {
        showConsole = false;
    }

    public void Awake()
    {
        DefineCommands();
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

    void DefineCommands()
    {
        //Commands
        VOXEL = new UtilityCommand("voxel", "Generate a voxel", "voxel", () =>
        {
            PlaceCube.instance.createCube(0,0,0);
        });
        
        BOX = new UtilityCommand<int,Vector3>("box", "Generate a box", "box <size> <start x> <start y> <start z>", (x,v3) =>
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    for (int k = 0; k < x; k++)
                    {
                        PlaceCube.instance.createCube(v3.x+i,v3.y+j,v3.z+k);
                    }
                }
            }
        });
        
        LINE = new UtilityCommand<int,Vector3>("line", "Generate a line", "line <length> <start x> <start y> <start z>", (x,v3) =>
        {
            for (int i = 0; i <= x; i++)
            {
                PlaceCube.instance.createCube(v3.x+ i,v3.y,v3.z);
            }
        });
        
        //TODO: Add a Sphere command
        
        //Add commands to list
        commandList = new List<object>
        {
            VOXEL,
            BOX,
            LINE
        };
    }
    
    private void OnGUI()
    {
        if (!showConsole) { return; }
        if (Event.current.keyCode == KeyCode.Return)
        {
            HandleInput();
            input = "";
            GUI.FocusControl(null);
            Event.current.Use();
        }
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
                
                else if (commandList[i] as UtilityCommand<int,Vector3> != null)
                {
                    (commandList[i] as UtilityCommand<int,Vector3>).Invoke(int.Parse(props[1]),new Vector3(int.Parse(props[2]),int.Parse(props[3]),int.Parse(props[4])));
                }
            }
        }
    }
}
