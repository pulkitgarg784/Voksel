using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                //string path = Path.Combine(Application.persistentDataPath, "data");
                string path = Path.Combine(filePath.text, fileName.text + ".fbx");
                //Create Directory if it does not exist
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                FBXExporter.ExportGameObjToFBX(objMeshToExport, path, true, true);
                Debug.Log("exported");

        }
    }


