using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGrid : MonoBehaviour
{
    private Renderer renderer;
    [Range(0, 50)]
    public int gridSize;

    public Text gridSizeText;
    public Slider gridSizeSlider;
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        gridSize = 10;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(gridSize, .1f, gridSize);
        renderer.material.mainTextureScale =
            new Vector2(gameObject.transform.localScale.x , gameObject.transform.localScale.z);
        if (gridSize % 2 == 0)
        {
            renderer.material.mainTextureOffset = new Vector2(0.5f, 0.5f);
        }
        else
        {
            renderer.material.mainTextureOffset = new Vector2(0f, 0f);

        }
    }
    public void SetSliderValue()
    {
        gridSize = (int) gridSizeSlider.value;
        gridSizeText.text = gridSize.ToString();
    }
}
