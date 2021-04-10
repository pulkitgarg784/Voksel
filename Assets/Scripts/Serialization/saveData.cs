using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class saveData
{
    private static saveData _current;
    public static saveData current
    {
        get {
            if (_current == null)
            {
                _current = new saveData();
            }
            return _current;
        }
        set
        {
            if(value != null)
            {
                _current = value;
            }
        }
    }
    public List<cubeData> cubes = new List<cubeData>();
    public List<Color> colorData = new List<Color>();
}
