using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceCube : MonoBehaviour
{
    private Camera mainCamera;
    public Transform parent;
    public GameObject cube;
    public float gridSize;
    Vector3 worldPos;
    public Text coordsText;

    public static PlaceCube instance;
    private EditorCamera editorCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            worldPos = hit.point;
            if (coordsText != null)
            {
                coordsText.text = "(" + RoundToCoordinate(worldPos.x) + "," + RoundToCoordinate(worldPos.y) + "," +
                                  RoundToCoordinate(worldPos.z) + ")";
            }

            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    deleteCube(hit);
                }
                else
                {
                    Debug.Log("create");
                    createCube(worldPos.x, worldPos.y, worldPos.z);
                }
            }
        }
    }


    public void createCube(float xpos, float ypos, float zpos)
    {
        Vector3 pos = new Vector3();

        pos.x = RoundToCoordinate(xpos);
        pos.y = RoundToCoordinate(ypos) - 0.5f;
        pos.z = RoundToCoordinate(zpos);
        GameObject go = Instantiate(cube, pos, Quaternion.identity);
        go.transform.SetParent(parent);
        //go.GetComponent<Renderer>().material.color = picker.CurrentColor;
        go.GetComponent<box>().materialIndex = ColorPalette.instance.currentMaterialIndex;
    }

    int RoundToCoordinate(float num)
    {
        return ((int) Mathf.Round((num / gridSize) * gridSize));
    }

    void deleteCube(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Box"))
        {
            Debug.Log("hit box");
            Destroy(hit.collider.gameObject);
        }
    }
}