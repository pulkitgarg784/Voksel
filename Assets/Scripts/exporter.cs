using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEditor;
using UnityEngine;
using UnityFBXExporter;
using UnityEngine.UI;
public class exporter : MonoBehaviour
{
    public GameObject objMeshToExport;
    public InputField filePath;
    public InputField fileName;

    public void ExportFbx()
    {
        Debug.Log("trying to export");
        Debug.Log(fileName.text);
        Debug.Log(filePath.text);
        //string path  = EditorUtility.SaveFilePanel("Export as FBX", "", "untitled", "fbx");
        string path = StandaloneFileBrowser.SaveFilePanel("Export as FBX", "", "untitled", "fbx");
        //string path = @"C:\Users\Pulkit\Desktop\untitled.fbx";
        path = path.Replace("\\", "/");
        Debug.Log(path);
        FBXExporter.ExportGameObjToFBX(objMeshToExport, path, true, true);
        Debug.Log("exported");

        }
    }


