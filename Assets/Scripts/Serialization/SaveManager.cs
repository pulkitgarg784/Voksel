using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SaveManager : MonoBehaviour
{
    public GameObject cube;
    public Transform modelHolder;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSave();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (Transform child in modelHolder) {
                GameObject.Destroy(child.gameObject);
            }
            OnLoad();
        }

    }

    public void OnSave()
    {
        SerializationManager.Save("save", saveData.current);
        Debug.Log("saved");
    }

    public void OnLoad()
    {
        saveData.current = (saveData) SerializationManager.Load(Application.persistentDataPath + "/saves/save.voksel");
        for (int i = 0; i < saveData.current.cubes.Count; i++)
        {
            cubeData currentCube = saveData.current.cubes[i];
            GameObject obj = Instantiate(cube);
            box boxObj = obj.GetComponent<box>();
            boxObj.cubedata = currentCube;
            boxObj.transform.position = currentCube.position;
            
        }
    }
}
