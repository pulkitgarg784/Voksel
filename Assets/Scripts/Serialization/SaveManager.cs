using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public GameObject cube;
    public Transform modelHolder;
    public Text projectTitle;
    public GameObject saveLoadPrompt;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (Transform child in modelHolder) {
                GameObject.Destroy(child.gameObject);
            }
            projectTitle.text = "untitled.voksel";

        }
    }

    public void OnSave()
    {
        string path = EditorUtility.SaveFilePanel("Save as", "", "untitled", "voksel");
        SerializationManager.Save(path, saveData.current);
        Debug.Log("saved");
    }

    public void OnLoad()
    {
        string path = EditorUtility.OpenFilePanel("Open Project", "", "voksel");
        if (File.Exists(path))
        {
            saveData.current = (saveData) SerializationManager.Load(path);

            for (int i = 0; i < saveData.current.cubes.Count; i++)
            {
                cubeData currentCube = saveData.current.cubes[i];
                GameObject obj = Instantiate(cube);
                obj.transform.SetParent(modelHolder);
                box boxObj = obj.GetComponent<box>();
                boxObj.cubedata = currentCube;
                boxObj.transform.position = currentCube.position;
                boxObj.gameObject.GetComponent<Renderer>().material.color = currentCube.color;
            }
            saveLoadPrompt.SetActive(false);
        }
    }
}
