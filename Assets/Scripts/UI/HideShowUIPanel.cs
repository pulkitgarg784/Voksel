using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowUIPanel : MonoBehaviour
{
    public void showPanel(GameObject panel)
    {
        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
        }
    }
    public void hidePanel(GameObject panel)
    {
        if (panel.activeSelf == true)
        {
            panel.SetActive(false);
        }
    }

    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}
