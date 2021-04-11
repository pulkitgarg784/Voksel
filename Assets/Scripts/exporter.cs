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
    private MeshCombiner meshCombiner;
    public void ExportFbx()
    {
        //string path  = EditorUtility.SaveFilePanel("Export as FBX", "", "untitled", "fbx");
        string path = StandaloneFileBrowser.SaveFilePanel("Export as FBX", "", "untitled", "fbx");
        //string path = @"C:\Users\Pulkit\Desktop\untitled.fbx";
        path = path.Replace("\\", "/");
        Debug.Log(path);
        createMesh(path);
        Debug.Log("exported");
    }

    void createMesh(string path)
    {
        
        GameObject tempModel = Instantiate(objMeshToExport);
        meshCombiner = tempModel.AddComponent<MeshCombiner>();
        meshCombiner.CreateMultiMaterialMesh = true;
        meshCombiner.DeactivateCombinedChildren = false;
        meshCombiner.DestroyCombinedChildren = true;
        meshCombiner.CombineMeshes(false);
        FBXExporter.ExportGameObjToFBX(tempModel, path, true, true);
        Destroy(tempModel);


    }
}



