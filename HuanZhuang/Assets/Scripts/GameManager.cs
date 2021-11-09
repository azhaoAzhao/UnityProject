using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public RawImage showClothes; //展示衣服
    public Transform Pool;
    public Pictures Item;
    public Transform CheckArea;
    public RawImage Face; //脸部
    public Transform FacePosstion;

    [HideInInspector] public Texture2D InTimePhoto;

    [HideInInspector] public Texture2D CurrTexture2D;

    public Configuration Configuration;
    public Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

    // Start is called before the first frame update
    void Start()
    {
        #region 创建json使用 其他情况不使用

        // Configuration = new Configuration();
        // Configuration.printName = "FX DocuPrint M115 b";
        // Configuration.defaultCloth = "检查官";
        // Configuration.imagePath = new Dictionary<string, Posstion>();
        // Configuration.imagePath.Add("律师袍", new Posstion(22, 260, 0));
        // Configuration.imagePath.Add("法袍", new Posstion(22, 260, 0));
        // Configuration.imagePath.Add("检查官", new Posstion(22, 260, 0));
        // Configuration.imagePath.Add("警察服装", new Posstion(22, 260, 0));
        // UITools.SetJson(JsonConvert.SerializeObject(Configuration), "config");
        // return;

        #endregion

        string json = UITools.GetJsonString("config");
        Configuration = JsonConvert.DeserializeObject<Configuration>(json);

        LoadClothes();
        CreateItem();

        SelectTexture(images[Configuration.defaultCloth], Configuration.defaultCloth);
    }

    public void SetFacialImages()
    {
        Face.texture = CurrTexture2D;
        Face.color = new Color(255f, 255f, 255f, 255f);
    }

    private void CreateItem()
    {
        foreach (var Value in images)
        {
            var item = Instantiate(Item, Pool, false);
            item.SetActive(true);
            item.Initialized(SelectTexture, Value.Value, Value.Key);
        }
    }

    private void SelectTexture(Texture2D texture2D, string name)
    {
        showClothes.texture = texture2D;
        SetCheckArea(Configuration.imagePath[name].PosstionTOVectro3());
    }

    private void SetCheckArea(Vector3 posstion)
    {
        FacePosstion.localPosition = posstion;
    }

    private void LoadClothes()
    {
        Dictionary<string, string> filePaths = new Dictionary<string, string>();

        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');

        for (int i = 0; i < ImageType.Length; i++)
        {
            //获取Application.dataPath文件夹下所有的图片路径  
            string[] dirs = Directory.GetFiles((Application.streamingAssetsPath + "/Pictures/"), ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(dirs[j]);
                filePaths.Add(fileNameWithoutExtension, dirs[j]);
            }
        }

        foreach (var item in filePaths)
        {
            Texture2D tx = new Texture2D(315, 450);
            tx.LoadImage(getImageByte(item.Value));
            images.Add(item.Key, tx);
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

    private void Update()
    {
        DateTime dt = new DateTime(2021, 10, 28);
        //TODO 正式版要删除 测试时间截止到2021-10-23
#if UNITY_EDITOR
        return;
#endif
        if (DateTime.Now > dt)
        {
            //大于我设置的时间直接让他退出 防止狗比不给钱
            Application.Quit();
        }
    }
}

public class Configuration
{
    public string defaultCloth;
    public string printName;
    public Dictionary<string, Posstion> imagePath; //衣服名称+识别坐标
}

public class Posstion
{
    public float x;
    public float y;
    public float z;

    public Posstion(float x, float y, float z)
    {
        this.x = x;
        this.z = z;
        this.y = y;
    }

    public Vector3 PosstionTOVectro3()
    {
        return new Vector3(x, y, z);
    }
}