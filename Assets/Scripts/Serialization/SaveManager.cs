using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public GameObject cube;
    public Transform modelHolder;
    public InputField fileName;
    public Text projectTitle;
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
        SerializationManager.Save(fileName.text, saveData.current);
        projectTitle.text = fileName.text+".voksel";
        fileName.text = "";
        Debug.Log("saved");
    }

    public void OnLoad()
    {
        string path = Application.persistentDataPath + "/saves/" + fileName.text + ".voksel";
        if (File.Exists(path))
        {
            saveData.current = (saveData) SerializationManager.Load(path);
            projectTitle.text = fileName.text + ".voksel";
            fileName.text = "";

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
        }
    }
}
