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
    bool combineMeshes;
    public Toggle combineToggle;
    public Dropdown fileFormatSelector;
    private string path;
    public void ExportFbx()
    {
        combineMeshes = combineToggle.isOn;
        if (fileFormatSelector.value == 0){ path = getPath("fbx");}

        if (fileFormatSelector.value == 1)
        {
            path = getPath("obj");
            combineMeshes = true;
        }
        if (path != null)
        {createMesh();}
    }

    string getPath(string format)
    {
        return StandaloneFileBrowser.SaveFilePanel("Export as "+ format.ToUpper(), "", "untitled", format).Replace("\\", "/");
    }
    void createMesh()
    {
        GameObject tempModel = Instantiate(objMeshToExport);
        tempModel.transform.position = objMeshToExport.transform.position;
        if(combineMeshes){combineMesh(tempModel);}
        exportMesh(tempModel);
        //Destroy(tempModel);
    }
    void combineMesh(GameObject tempModel)
    {
        meshCombiner = tempModel.AddComponent<MeshCombiner>();
        meshCombiner.CreateMultiMaterialMesh = true;
        meshCombiner.DeactivateCombinedChildren = false;
        meshCombiner.DestroyCombinedChildren = true;
        meshCombiner.CombineMeshes(false);
    }

    void exportMesh(GameObject obj)
    {
        if (fileFormatSelector.value == 0){FBXExporter.ExportGameObjToFBX(obj, path, false, false);}

        if (fileFormatSelector.value == 1)
        {
            ObjExporter.MeshToFile(obj.GetComponent<MeshFilter>(),path);
        }
        Debug.Log("exported");
    }

    public void setCombineToggleState()
    {
        combineToggle.gameObject.SetActive(!(fileFormatSelector.value == 1));
    }
}



