using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LCPrinter;
using UnityEngine.Serialization;

public class WinCamera : MonoBehaviour
{
    WebCamTexture tex;
    public Image WebCam;
    public MeshRenderer ma;
    public UIClick saveButton;
    public UIClick selectButton;
    public UIClick OpenSavePanel;
    public UIClick SvaeANDPrint; //保存并打印

    public RawImage bgimage_02;

    public RawImage Save_Image; //保存的照片

    public GameObject game_UI; //UI

    public SelectPanel SelectPanel;

    void Start()
    {
        //开启协程，获取摄像头图像数据
        StartCoroutine(OpenCamera());
        saveButton.AddListener(SaveImage);
        selectButton.AddListener(OpenSelect);
        OpenSavePanel.AddListener(() => { FilmingFace.Instance.Open(); });
        SvaeANDPrint.AddListener(() => { StartCoroutine(SavesANDPrint()); });
    }

    /// <summary>
    /// 打开查看界面
    /// </summary>
    public void OpenSelect()
    {
        SelectPanel.gameObject.SetActive(true);
        SelectPanel.Open();
    }

    IEnumerator OpenCamera()
    {
        //等待用户允许访问
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        //如果用户允许访问，开始获取图像        
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            try
            {
                //先获取设备
                WebCamDevice[] device = WebCamTexture.devices;
                string deviceName = device[0].name;
                //然后获取图像
                tex = new WebCamTexture(deviceName, 1080, 1920, 60);
                //将获取的图像赋值
                ma.material.mainTexture = tex;
                bgimage_02.texture = ma.material.mainTexture;
                GameManager.Instance.InTimePhoto = TextureToTexture2D(ma.material.mainTexture);
                bgimage_02.color = new Color(255f, 255f, 255f, 255);
                //开始实施获取
                tex.Play();
            }
            catch (Exception e)
            {
                Debug.LogError("出现问题：" + e.Message);
                throw;
            }
        }
    }

    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }

    IEnumerator SavesANDPrint()
    {
        game_UI.SetActive(false);
        Camera.main.Render(); //渲染
        string path = Application.streamingAssetsPath + "/Icons/" + Time.time + ".png";

        ScreenShotFile(path);
        yield return new WaitForSecondsRealtime(0.5f);
        game_UI.SetActive(true);

        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');

        string currFIleName = System.IO.Path.GetFileNameWithoutExtension(path); //当前扩展名称
        for (int i = 0;
            i < ImageType.Length;
            i++)
        {
            //获取Application.dataPath文件夹下所有的图片路径  
            string[] dirs = Directory.GetFiles((Application.streamingAssetsPath + "/Icons/"), ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(dirs[j]);
                if (fileNameWithoutExtension == currFIleName)
                {
                    Texture2D tx = new Texture2D(503, 503);
                    tx.LoadImage(getImageByte(dirs[j]));
                    ImagePrinat(tx);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 保存并打印
    /// </summary>
    private void SaveANDPrint()
    {
        game_UI.SetActive(false);
        Camera.main.Render(); //渲染
        string path = Application.streamingAssetsPath + "/Icons/" + Time.time + ".png";

        ScreenShotFile(path);

        game_UI.SetActive(true);

        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');

        string currFIleName = System.IO.Path.GetFileNameWithoutExtension(path); //当前扩展名称
        for (int i = 0;
            i < ImageType.Length;
            i++)
        {
            //获取Application.dataPath文件夹下所有的图片路径  
            string[] dirs = Directory.GetFiles((Application.streamingAssetsPath + "/Icons/"), ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(dirs[j]);
                if (fileNameWithoutExtension == currFIleName)
                {
                    Texture2D tx = new Texture2D(503, 503);
                    tx.LoadImage(getImageByte(dirs[j]));
                    ImagePrinat(tx);
                    break;
                }
            }
        }
    }

    /// <summary>  
    /// 根据图片路径返回图片的字节流byte[]  
    /// </summary>  
    /// <param name="imagePath">图片路径</param>  
    /// <returns>返回的字节流</returns>  
    private static byte[] getImageByte(string imagePath)
    {
        FileStream files = new FileStream(imagePath, FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();
        return imgByte;
    }

    private void SaveImage()
    {
        //在上一段代码中加入该方法
        // Save(tex);

        StartCoroutine(Saves());
    }


    public void ImagePrinat(Texture2D texture2D)
    {
        try
        {
            var be = texture2D.EncodeToPNG();
            Print.PrintTexture(be, 1, GameManager.Instance.Configuration.printName);
            Debug.Log("打印");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    IEnumerator Saves()
    {
        game_UI.SetActive(false);
        string path = Application.streamingAssetsPath + "/Icons" + "/" + Time.time + ".png";
        ScreenShotFile(path);
        yield return new WaitForSecondsRealtime(0.5f);

        game_UI.SetActive(true);
    }

    public void Save(WebCamTexture t)
    {
        game_UI.SetActive(false);
        string path = Application.streamingAssetsPath + "/Icons" + "/" + Time.time + ".png";
        ScreenShotFile(path);

        if (Directory.Exists(path))
        {
            game_UI.SetActive(true);
        }


        return;
        game_UI.SetActive(false);
        Screenshots.Instance.Save(ScreenshotsSave, t); //截屏必须通过摄像机选然后才可以 所以这里写回调

        void ScreenshotsSave(Texture2D t2d)
        {
            game_UI.SetActive(true);
            //编码
            Save_Image.texture = t2d;
            Save_Image.gameObject.SetActive(true);
            Save_Image.transform.DOScale(Vector3.zero, 1f).OnComplete(() =>
            {
                Save_Image.gameObject.SetActive(false);
                Save_Image.transform.localScale = Vector3.one;
            }); //展示自己后隐藏并且还原大小
            JudgementOrCreate("/Icons");
            ScreenShotFile(Application.streamingAssetsPath + "/Icons" + "/" + Time.time + ".png");
        }
    }

    /// <summary>
    /// UnityEngine自带截屏Api，只能截全屏
    /// </summary>
    /// <param name="fileName">文件名</param>
    public void ScreenShotFile(string fileName)
    {
        ScreenCapture.CaptureScreenshot(fileName); //截图并保存截图文件
        Debug.Log(string.Format("截取了一张图片: {0}", fileName));

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); //刷新Unity的资产目录
#endif
    }


    /// <summary>
    /// 判断是否有存储头像文件夹，如果没有，创建一个空文档。本机地址，相对路径。
    /// </summary>
    void JudgementOrCreate(string path)
    {
        if (!Directory.Exists(Application.streamingAssetsPath + path))
        {
            Debug.Log(Application.streamingAssetsPath);
            Directory.CreateDirectory(Application.streamingAssetsPath + path);

            print("文件夹不存在,创建");
        }
    }
}