using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[System.Serializable]
public class box : MonoBehaviour
{
    // Start is called before the first frame update
    private Outline outlineshader;
    public cubeData cubedata;
    public saveData savedata;
    public bool canDelete;

    private void Awake()
    {
        outlineshader = GetComponent<Outline>();
        outlineshader.enabled = false;
    }
    private void OnMouseEnter()
    {
        outlineshader.enabled = true;
    }
    private void OnMouseExit()
    {
        outlineshader.enabled = false;
    }
    
    private void Start()
    {
        if (string.IsNullOrEmpty(cubedata.id))
        {
            cubedata.id = System.DateTime.Now.ToLongDateString() + System.DateTime.Now.ToLongTimeString() +
                          Random.Range(0, int.MaxValue).ToString();
            cubedata.position = transform.position;
            //cubedata.color = Color.green;
            saveData.current.cubes.Add(cubedata);
            Debug.Log(saveData.current.cubes);
        }
        
        //assign to load event
    }

    private void OnDestroy()
    {
        saveData.current.cubes.Remove(cubedata);
    }

    void destroyMe()
    {
        //unassign from load event
        Destroy(gameObject);
    }
}