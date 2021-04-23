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
    public bool combineMeshes;
    public void ExportFbx()
    {
        createMesh(getPath());
    }

    string getPath()
    {
        return StandaloneFileBrowser.SaveFilePanel("Export as FBX", "", "untitled", "fbx").Replace("\\", "/");
    }
    void createMesh(string path)
    {
        GameObject tempModel = Instantiate(objMeshToExport);
        tempModel.transform.position = objMeshToExport.transform.position;
        if(combineMeshes){combineMesh(tempModel);}
        FBXExporter.ExportGameObjToFBX(objMeshToExport, path, true, true);
        Destroy(tempModel);
    }

    void combineMesh(GameObject tempModel)
    {
        meshCombiner = tempModel.AddComponent<MeshCombiner>();
        meshCombiner.CreateMultiMaterialMesh = true;
        meshCombiner.DeactivateCombinedChildren = false;
        meshCombiner.DestroyCombinedChildren = true;
        meshCombiner.CombineMeshes(false);
    }
}



