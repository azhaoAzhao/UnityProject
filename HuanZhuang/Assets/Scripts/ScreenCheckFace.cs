using System;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnityExample;
using UnityEngine;

public class ScreenCheckFace : Singleton<ScreenCheckFace>
{
    private RectTransform _RectTransform;
    private Action<bool> CallBack;
    private Texture2D screenShot;

    public void CheckFace(RectTransform _rectTransform, Action<bool> callBack)
    {
        _RectTransform = _rectTransform;
        screenShot = new Texture2D((int) _RectTransform.rect.width, (int) _RectTransform.rect.height,
            TextureFormat.RGB24, true);
        CallBack = callBack;
    }

    public void StopCheck()
    {
        _RectTransform = null;
    }

    private void OnPreRender()
    {
        if (_RectTransform == null)
        {
            return;
        }

        float x = _RectTransform.localPosition.x + (Screen.width - _RectTransform.rect.width) / 2;
        float y = _RectTransform.localPosition.y + (Screen.height - _RectTransform.rect.height) / 2;
        Rect position = new Rect(x, y, _RectTransform.rect.width, _RectTransform.rect.height);
        screenShot.ReadPixels(position, 0, 0, true); //按照设定区域读取像素；注意是以左下角为原点读取
        screenShot.Apply();
        CallBack?.Invoke(MyFaceDetection.Instance.CheckFace(screenShot));
    }
}