using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pictures : MonoBehaviour
{
    public RawImage showClothes;

    public void Initialized(Action<Texture2D, string> callBack, Texture2D texture2D, string name)
    {
        this.GetComponent<UIClick>().AddListener(() => { callBack?.Invoke(texture2D, name); });
        showClothes.texture = texture2D;
    }
}