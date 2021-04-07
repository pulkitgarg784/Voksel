using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        string path  = EditorUtility.SaveFilePanel("Export as FBX", "", "untitled", "fbx");
        FBXExporter.ExportGameObjToFBX(objMeshToExport, path, true, true);
        Debug.Log("exported");

        }
    }


