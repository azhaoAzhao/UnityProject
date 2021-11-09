using System;
using System.Collections;
using System.Collections.Generic;
using LCPrinter;
using UnityEngine;
using UnityEngine.UI;

public class PrintPanel : MonoBehaviour
{
    public RawImage ShowImage;
    public UIClick btn_Print;
    public UIClick btn_Back;

    private string currPath;

    private Texture2D _texture2D;

    private void Start()
    {
        btn_Back.AddListener(() => { gameObject.SetActive(false); });
        btn_Print.AddListener(ImagePrinat);
    }

    public void ImagePrinat()
    {
        try
        {
            var be = _texture2D.EncodeToPNG();
            Print.PrintTexture(be, 1, GameManager.Instance.Configuration.printName);
            Debug.Log("打印");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public void Initialized(string path, Texture2D texture2D)
    {
        gameObject.SetActive(true);
        ShowImage.texture = texture2D;
        currPath = path;
        this._texture2D = texture2D;
    }
}