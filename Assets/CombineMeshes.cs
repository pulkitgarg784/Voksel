using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeshes : MonoBehaviour
{
    private MeshCombiner meshCombiner;
    private void Start()
    {
        meshCombiner = gameObject.AddComponent<MeshCombiner>();
        meshCombiner.CreateMultiMaterialMesh = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            meshCombiner.CombineMeshes(false);
        }
    }
}
