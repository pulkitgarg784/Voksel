using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCube : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject cube;
    public float gridSize;
    Vector3 wordPos;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos=new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        if(Input.GetMouseButtonDown(0)) {
            Ray ray=mainCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,1000f)) {
                wordPos=hit.point;
            } else {
                wordPos=mainCamera.ScreenToWorldPoint(mousePos);
            }
            wordPos.x = Mathf.Round((wordPos.x / gridSize) *gridSize);
            wordPos.y = Mathf.Round((wordPos.y / gridSize) *gridSize);
            wordPos.z = Mathf.Round((wordPos.z / gridSize) *gridSize);
            Debug.Log(wordPos);
            Instantiate(cube,wordPos,Quaternion.identity); 
        }
        
    }
    
}
