using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour,IDragHandler,IPointerDownHandler
{
    private RectTransform dragRectTransform;
    [SerializeField] private Canvas parentCanvas;

    private void Awake()
    {
        dragRectTransform = transform.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta/parentCanvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.SetAsLastSibling();
    }
}
