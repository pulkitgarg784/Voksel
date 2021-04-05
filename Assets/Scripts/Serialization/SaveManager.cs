using System;
using System.Collections;
using System.Collections.Generic;
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
        SerializationManager.Save(fileName.text, saveData.current);
        projectTitle.text = fileName.text+".voksel";
        fileName.text = "";
        Debug.Log("saved");
    }

    public void OnLoad()
    {
        saveData.current = (saveData) SerializationManager.Load(Application.persistentDataPath + "/saves/"+ fileName.text+  ".voksel");
        projectTitle.text = fileName.text+".voksel";
        fileName.text = "";

        for (int i = 0; i < saveData.current.cubes.Count; i++)
        {
            cubeData currentCube = saveData.current.cubes[i];
            GameObject obj = Instantiate(cube);
            box boxObj = obj.GetComponent<box>();
            boxObj.cubedata = currentCube;
            boxObj.transform.position = currentCube.position;
            boxObj.gameObject.GetComponent<Renderer>().material.color = currentCube.color;

        }
    }
}
