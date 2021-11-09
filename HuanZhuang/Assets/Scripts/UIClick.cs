using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Action onClick;

    public void AddListener(Action callBack)
    {
        onClick = callBack;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //按下
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //抬起
        transform.localScale = Vector3.one;
        onClick?.Invoke();
    }
}