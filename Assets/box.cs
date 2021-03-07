using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    // Start is called before the first frame update
    private Outline outlineshader;
    public bool canDelete;

    private void Awake()
    {
        outlineshader = GetComponent<Outline>();
        outlineshader.enabled = false;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && canDelete == true)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        outlineshader.enabled = true;
    }

    private void OnMouseExit()
    {
        outlineshader.enabled = false;
    }
}