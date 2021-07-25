using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandConsole : MonoBehaviour
{
    private bool showConsole;
    private bool showHelp;
    private string input;

    public static UtilityCommand HELP;
    public static UtilityCommand VOXEL;
    public static UtilityCommand<int,Vector3> BOX;
    public static UtilityCommand<Vector3,Vector3> LINE;


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
        HELP = new UtilityCommand("help", "Show the list of all commands", "help", () => { showHelp = true; });
        VOXEL = new UtilityCommand("voxel", "Generate a voxel", "voxel",
            () => { PlaceCube.instance.createCube(0, 0, 0); });

        BOX = new UtilityCommand<int, Vector3>("box", "Generate a box", "box <size> <start x> <start y> <start z>",
            (x, v3) =>
            {
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        for (int k = 0; k < x; k++)
                        {
                            PlaceCube.instance.createCube(v3.x + i, v3.y + j, v3.z + k);
                        }
                    }
                }
            });

        LINE = new UtilityCommand<Vector3, Vector3>("line", "Generate a line",
            "line <start x> <start y> <start z> <end x> <end y> <end z>", (startv3, endv3) =>
            {
                int x1 = (int)startv3.x;
                int y1 = (int)startv3.y;
                int z1 = (int)startv3.z;
                int x0 = (int) endv3.x;
                int y0 = (int) endv3.y;
                int z0 = (int) endv3.z;
                int dx = Mathf.Abs(x1 - x0);
                int dy = Mathf.Abs(y1 - y0);
                int dz = Mathf.Abs(z1 - z0);
                int stepX = x0 < x1 ? 1 : -1;
                int stepY = y0 < y1 ? 1 : -1;
                int stepZ = z0 < z1 ? 1 : -1;
                double hypotenuse = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2) + Mathf.Pow(dz, 2));
                double tMaxX = hypotenuse * 0.5 / dx;
                double tMaxY = hypotenuse * 0.5 / dy;
                double tMaxZ = hypotenuse * 0.5 / dz;
                double tDeltaX = hypotenuse / dx;
                double tDeltaY = hypotenuse / dy;
                double tDeltaZ = hypotenuse / dz;
                while (x0 != x1 || y0 != y1 || z0 != z1)
                {
                    if (tMaxX < tMaxY)
                    {
                        if (tMaxX < tMaxZ)
                        {
                            x0 = x0 + stepX;
                            tMaxX = tMaxX + tDeltaX;
                        }
                        else if (tMaxX > tMaxZ)
                        {
                            z0 = z0 + stepZ;
                            tMaxZ = tMaxZ + tDeltaZ;
                        }
                        else
                        {
                            x0 = x0 + stepX;
                            tMaxX = tMaxX + tDeltaX;
                            z0 = z0 + stepZ;
                            tMaxZ = tMaxZ + tDeltaZ;
                        }
                    }
                    else if (tMaxX > tMaxY)
                    {
                        if (tMaxY < tMaxZ)
                        {
                            y0 = y0 + stepY;
                            tMaxY = tMaxY + tDeltaY;
                        }
                        else if (tMaxY > tMaxZ)
                        {
                            z0 = z0 + stepZ;
                            tMaxZ = tMaxZ + tDeltaZ;
                        }
                        else
                        {
                            y0 = y0 + stepY;
                            tMaxY = tMaxY + tDeltaY;
                            z0 = z0 + stepZ;
                            tMaxZ = tMaxZ + tDeltaZ;

                        }
                    }
                    else
                    {
                        if (tMaxY < tMaxZ)
                        {
                            y0 = y0 + stepY;
                            tMaxY = tMaxY + tDeltaY;
                            x0 = x0 + stepX;
                            tMaxX = tMaxX + tDeltaX;
                        }
                        else if (tMaxY > tMaxZ)
                        {
                            z0 = z0 + stepZ;
                            tMaxZ = tMaxZ + tDeltaZ;
                        }
                        else
                        {
                            x0 = x0 + stepX;
                            tMaxX = tMaxX + tDeltaX;
                            y0 = y0 + stepY;
                            tMaxY = tMaxY + tDeltaY;
                            z0 = z0 + stepZ;
                            tMaxZ = tMaxZ + tDeltaZ;

                        }
                    }

                    PlaceCube.instance.createCube(x0, y0, z0);
                }

            });
    

    //TODO: Add a Sphere command
        
        //Add commands to list
        commandList = new List<object>
        {
            HELP,
            VOXEL,
            LINE,
            BOX

        };
    }

    private Vector2 scroll;
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

        float y = 60;

        if (showHelp)
        {
            
            GUI.Box(new Rect(0,y,Screen.width,100), "");
            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                UtilityCommandBase command = commandList[i] as UtilityCommandBase;
                string label = $"{command.commandFormat} - {command.commandDescription}";
                GUIStyle helpStyle = new GUIStyle(GUI.skin.label);
                helpStyle.fontSize = 16;
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 40);
                GUI.Label(labelRect, label,helpStyle);
            }
            GUI.EndScrollView();
            y += 100;
        }
        GUI.Box(new Rect(0,y,Screen.width,45), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUIStyle textStyle = new GUIStyle(GUI.skin.textField);
        textStyle.fontSize = 20;

        input = GUI.TextField(new Rect(10f, y+7.5f, Screen.width - 20f, 30f), input, textStyle);
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
                else if (commandList[i] as UtilityCommand<Vector3,Vector3> != null)
                {
                    (commandList[i] as UtilityCommand<Vector3,Vector3>).Invoke(new Vector3(int.Parse(props[1]),int.Parse(props[2]),int.Parse(props[3])),new Vector3(int.Parse(props[4]),int.Parse(props[5]),int.Parse(props[6])));
                }
            }
        }
    }
}
