using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshots : Singleton<Screenshots>
{
    bool grab;

    // The "m_Display" is the GameObject whose Texture will be set to the captured image.
    public Renderer m_Display;

    private WebCamTexture _texture;
    private Action<Texture2D> CallBack;

    public void Save(Action<Texture2D> callBack, WebCamTexture texture)
    {
        grab = true;
        _texture = texture;
        CallBack = callBack;
    }


    private void OnPreRender()
    {
        if (grab == false || _texture == null)
        {
            return;
        }

        Texture2D t2d = new Texture2D(1080, 1920, TextureFormat.ARGB32, true);
        t2d.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        t2d.Apply();
        grab = false;
        _texture = null;
        CallBack?.Invoke(t2d);
    }
}