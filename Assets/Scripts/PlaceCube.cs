using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceCube : MonoBehaviour
{

    private Camera mainCamera;
    public Transform parent;
    public GameObject cube;
    public float gridSize;
    Vector3 worldPos;

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

            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftAlt))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                Ray ray = mainCamera.ScreenPointToRay(mousePos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    worldPos = hit.point;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        deleteCube(hit);
                    }
                    else
                    {
                        Debug.Log("create");
                        createCube(worldPos.x, worldPos.y,worldPos.z);
                    }
                }
            }
        }

        public void createCube(float xpos, float ypos, float zpos)
        {
            Vector3 pos = new Vector3();

            pos.x = Mathf.Round((xpos / gridSize) * gridSize);
            pos.y = Mathf.Round((ypos / gridSize) * gridSize) - 0.5f;
            pos.z = Mathf.Round((zpos / gridSize) * gridSize);
            GameObject go = Instantiate(cube, pos, Quaternion.identity);
            go.transform.SetParent(parent);
            //go.GetComponent<Renderer>().material.color = picker.CurrentColor;
            go.GetComponent<box>().materialIndex = ColorPalette.instance.currentMaterialIndex;
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