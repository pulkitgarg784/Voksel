using System.Collections;
using System.Collections.Generic;
using HSVPicker;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlaceCube : MonoBehaviour
{
    private Camera mainCamera;
    public Transform parent;
    public GameObject cube;
    public float gridSize;
    Vector3 wordPos;
    public ColorPicker picker;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos=new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        if(Input.GetMouseButtonDown(0)) {
            if(EventSystem.current.IsPointerOverGameObject())
                return;
            Ray ray=mainCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,1000f)) {
                wordPos=hit.point;
                if (hit.collider.CompareTag("Box") && Input.GetKey(KeyCode.LeftShift))
                {
                    Debug.Log("hit box");
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    wordPos.x = Mathf.Round((wordPos.x / gridSize) *gridSize);
                    wordPos.y = Mathf.Round((wordPos.y / gridSize) *gridSize);
                    wordPos.z = Mathf.Round((wordPos.z / gridSize) *gridSize);
                    Debug.Log(wordPos);
            
                    GameObject go =  Instantiate(cube,wordPos,Quaternion.identity); 
                    go.transform.SetParent(parent);
                    go.GetComponent<Renderer>().material.color = picker.CurrentColor;
                }
            }
            
        }
        
    }
    
}
