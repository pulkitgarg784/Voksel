using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWindow : MonoBehaviour,IDragHandler,IPointerDownHandler
{
    
    private Vector2 currentPointerPosition;
    private Vector2 previousPointerPosition;
    
    private Canvas parentCanvas;
    public RectTransform rectTransform;
    public Vector2 minSize;
    public Vector2 maxSize;
    private void Awake()
    {
        parentCanvas=  transform.GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null)
            return;
        Vector2 sizeDelta = rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, eventData.position, eventData.pressEventCamera, out currentPointerPosition);
        Vector2 resizeValue = currentPointerPosition - previousPointerPosition;

        sizeDelta += new Vector2 (resizeValue.x, -resizeValue.y);
        sizeDelta = new Vector2 (
            Mathf.Clamp (sizeDelta.x, minSize.x, maxSize.x),
            Mathf.Clamp (sizeDelta.y, minSize.y, maxSize.y)
        );

        rectTransform.sizeDelta = sizeDelta;

        previousPointerPosition = currentPointerPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, eventData.position, eventData.pressEventCamera, out previousPointerPosition);
    }
}
