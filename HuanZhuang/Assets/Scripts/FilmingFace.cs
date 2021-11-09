using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenCVForUnityExample;
using UnityEngine.UI;

public class FilmingFace : Singleton<FilmingFace>
{
    public GameObject TakingPicturesPanel;
    public RectTransform _RectTransform;
    public UIClick Btn_Shoot;
    public Image Outline;
    public Text Countdown;

    private bool is_Face; //是否有人脸

    private bool is_OpenPicture; //已经开启拍照

    private bool is_OpenCheck;

    public void Open()
    {
        TakingPicturesPanel.SetActive(true);
        // Outline.SetActive(true);
        is_OpenPicture = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Btn_Shoot.AddListener(() =>
        {
            if (is_OpenCheck)
            {
                return;
            }

            is_OpenCheck = true;
            StartCoroutine(CheckFace()); //点击拍照的时候开启检测
        });
    }

    IEnumerator CheckFace()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            // Outline.SetActive(false);
            ScreenCheckFace.Instance.CheckFace(_RectTransform, Back);
        }

        void Back(bool b)
        {
            is_Face = b;
            if (b && !is_OpenPicture)
            {
                Countdown.gameObject.SetActive(false);
                ScreenCheckFace.Instance.StopCheck();
                // StartCoroutine(getScreenTexture(_RectTransform));
                getScreenTexture(_RectTransform);
                is_OpenPicture = true;
            }
            else
            {
                // Outline.SetActive(true);
                // Countdown.text = "未识别到人脸";
                // Countdown.gameObject.SetActive(true);
            }
        }
    }

    void getScreenTexture(RectTransform rectT)
    {


        // Outline.gameObject.SetActive(false);
        Countdown.gameObject.SetActive(false);

        Texture2D screenShot = new Texture2D((int) rectT.rect.width, (int) rectT.rect.height, TextureFormat.RGB24, true);
        float x = rectT.localPosition.x + (Screen.width - rectT.rect.width) / 2;
        float y = rectT.localPosition.y + (Screen.height - rectT.rect.height) / 2;
        Rect position = new Rect(x, y, rectT.rect.width, rectT.rect.height);
        screenShot.ReadPixels(position, 0, 0, true); //按照设定区域读取像素；注意是以左下角为原点读取
        screenShot.Apply();
        // GameManager.Instance.CurrTexture2D = screenShot;
        MyFaceDetection.Instance.Run(screenShot); //赋值拍出来的人脸
        GameManager.Instance.SetFacialImages();
        TakingPicturesPanel.SetActive(false);

        is_OpenCheck = false;

        StopAllCoroutines();
    }
}