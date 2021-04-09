using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPalette : MonoBehaviour
{
    public static ColorPalette instance;
    public Material[] colorMaterials;
    public Button[] colorIndicatorButtons;
    public int currentMaterialIndex;
    

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        for (int i = 0; i < colorIndicatorButtons.Length; i++)
        {
            int closureIndex = i ; // Prevents the closure problem
            colorIndicatorButtons[closureIndex].onClick.AddListener( () => TaskOnClick( closureIndex ) );
        }
        // ...
    }

    private void Update()
    {
        //TODO remove this from update and run this only when we change the color of the material
        setIndicatorColor();

    }

    public void TaskOnClick( int buttonIndex )
    {
        Debug.Log("You have clicked the button #" + buttonIndex, colorIndicatorButtons[buttonIndex]);
        selectCurrentColor(buttonIndex);
    }

    public void setIndicatorColor()
    {
        for (int i = 0; i < colorIndicatorButtons.Length; i++)
        {
            colorIndicatorButtons[i].GetComponent<Image>().color = colorMaterials[i].color;
        }
    }
    public void selectCurrentColor(int i)
    {
        currentMaterialIndex = i;
    }
    
}
