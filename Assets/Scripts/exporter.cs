using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityFBXExporter;
using UnityEngine.UI;
public class exporter : MonoBehaviour
{
    public GameObject meshToExport;
    private MeshCombiner _meshCombiner;
    bool _combineMeshes;
    public Toggle combineToggle;
    public Dropdown fileFormatSelector;
    private string _path;
    public void ExportModel()
    {
        _combineMeshes = combineToggle.isOn;
        if (fileFormatSelector.value == 0){ _path = GetPath("fbx");}

        if (fileFormatSelector.value == 1)
        {
            _path = GetPath("obj");
            _combineMeshes = true;
        }
        if (_path != null)
        {CreateMesh();}
    }

    string GetPath(string format)
    {
        return StandaloneFileBrowser.SaveFilePanel("Export as "+ format.ToUpper(), "", "untitled", format).Replace("\\", "/");
    }
    void CreateMesh()
    {
        GameObject tempModel = Instantiate(meshToExport);
        tempModel.transform.position = meshToExport.transform.position;
        if(_combineMeshes){CombineMesh(tempModel);}
        ExportMesh(tempModel);
        Destroy(tempModel);
    }
    void CombineMesh(GameObject tempModel)
    {
        _meshCombiner = tempModel.AddComponent<MeshCombiner>();
        _meshCombiner.CreateMultiMaterialMesh = true;
        _meshCombiner.DeactivateCombinedChildren = false;
        _meshCombiner.DestroyCombinedChildren = true;
        _meshCombiner.CombineMeshes(false);
    }

    void ExportMesh(GameObject obj)
    {
        if (fileFormatSelector.value == 0){FBXExporter.ExportGameObjToFBX(obj, _path, false, false);}

        if (fileFormatSelector.value == 1)
        {
            ObjExporter.MeshToFile(obj.GetComponent<MeshFilter>(),_path);
        }
        Debug.Log("exported");
    }

    public void SetCombineToggleState()
    {
        combineToggle.gameObject.SetActive(!(fileFormatSelector.value == 1));
    }
}



