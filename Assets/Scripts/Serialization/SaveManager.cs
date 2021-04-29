using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HSVPicker;
using SFB;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public GameObject cube;
    public Transform modelHolder;
    public Text projectTitle;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
        {
            OnNew();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            OnSave();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O))
        {
            OnLoad();
        }
        
    }

    public void OnNew()
    {
        foreach (Transform child in modelHolder) {
            GameObject.Destroy(child.gameObject);
        }
        projectTitle.text = "untitled.voksel";
    }

    public void OnSave()
    {
        //string path = EditorUtility.SaveFilePanel("Save as", "", "untitled", "voksel");
        ColorPalette.instance.witePaletteToSave();
        string path = StandaloneFileBrowser.SaveFilePanel("Save as", "", "untitled", "voksel");
        path = path.Replace("\\", "/");
        if (path.Length > 0)
        {
            SerializationManager.Save(path, saveData.current);
            Debug.Log("saved");
        }
    }

    public void OnLoad()
    {
        
        //string path = EditorUtility.OpenFilePanel("Open Project", "", "voksel");
        var paths = StandaloneFileBrowser.OpenFilePanel("Open Project", "", "voksel", false);
        if (paths.Length > 0)
        {
            string path = paths[0];
            path = path.Replace("\\", "/");
            if (File.Exists(path))
            {
                foreach (Transform child in modelHolder) {
                    GameObject.Destroy(child.gameObject);
                }
                saveData.current = (saveData) SerializationManager.Load(path);

                for (int i = 0; i < saveData.current.cubes.Count; i++)
                {
                    cubeData currentCube = saveData.current.cubes[i];
                    GameObject obj = Instantiate(cube);
                    obj.transform.SetParent(modelHolder);
                    box boxObj = obj.GetComponent<box>();
                    boxObj.cubedata = currentCube;
                    boxObj.transform.position = currentCube.position;
                    //boxObj.gameObject.GetComponent<Renderer>().material = ColorPalette.instance.colorMaterials[currentCube.materialIndex];
                    boxObj.gameObject.GetComponent<box>().materialIndex = currentCube.materialIndex;
                }

                Debug.Log("loading colors");
                Debug.Log(saveData.current.colorData.Count);
                for (int i = 0; i < saveData.current.colorData.Count; i++)
                {
                    //colorData currentColor = saveData.current.colors[i];
                    //Debug.Log(i);
                    ColorPalette.instance.colorMaterials[i].color = saveData.current.colorData[i];
                }

                ColorPalette.instance.setIndicatorColors();

            }
        }

    }
}
