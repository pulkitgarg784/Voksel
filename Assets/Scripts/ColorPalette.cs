using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using HSVPicker;
using UnityEngine;
using UnityEngine.UI;

public class ColorPalette : MonoBehaviour
{
    public static ColorPalette instance;
    public Material[] colorMaterials;
    public Button[] colorIndicatorButtons;
    public int currentMaterialIndex;
    private Color currentColor;
    
    public ColorPicker picker;

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
        setIndicatorColors();
        picker.onValueChanged.AddListener(color =>
        {
            colorMaterials[currentMaterialIndex].color = color;
            setIndicatorColors();
        });
    }

    private void Update()
    {
        //TODO remove this from update and run this only when we change the color of the material
    }

    public void TaskOnClick( int buttonIndex )
    {
        selectCurrentColor(buttonIndex);
        for (int i = 0; i < colorIndicatorButtons.Length; i++)
        {
            RectTransform rectTransform = colorIndicatorButtons[i].GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(25, 25);
        }
        colorIndicatorButtons[buttonIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);

    }

    public void setIndicatorColors()
    {
        for (int i = 0; i < colorIndicatorButtons.Length; i++)
        {
            colorIndicatorButtons[i].GetComponent<Image>().color = colorMaterials[i].color;
        }

    }
    public void selectCurrentColor(int i)
    {
        currentMaterialIndex = i;
        picker.CurrentColor = colorMaterials[currentMaterialIndex].color;

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
