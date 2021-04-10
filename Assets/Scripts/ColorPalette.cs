using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class ColorPalette : MonoBehaviour
{
    public static ColorPalette instance;
    public Material[] colorMaterials;
    public Button[] colorIndicatorButtons;
    public int currentMaterialIndex;
    private Color currentColor;
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

    private void Start()
    {
        setIndicatorColor();
    }

    private void Update()
    {
        //TODO remove this from update and run this only when we change the color of the material
        setIndicatorColor();
    }

    public void TaskOnClick( int buttonIndex )
    {
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

    public void witePaletteToSave()
    {
        saveData.current.colorData.Clear();
        for (int i = 0; i < colorMaterials.Length; i++)
        {
            currentColor = colorMaterials[i].color;
            saveData.current.colorData.Add(currentColor);
        }
    }
    
}
