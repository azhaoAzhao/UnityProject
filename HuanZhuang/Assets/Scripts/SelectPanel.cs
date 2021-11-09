using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class SelectPanel : Singleton<SelectPanel>
{
    public Transform Pool;
    public SelectItem Item;
    public UIClick Back;


    private Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

    private void Start()
    {
        Back.AddListener(() => { gameObject.SetActive(false); });
    }

    public void Open()
    {
        DestroyPoolChild();
        load();
        CreateItem();
    }

    private void CreateItem()
    {
        foreach (var item in images)
        {
            var selectItem = Instantiate(Item, Pool, false);
            selectItem.gameObject.SetActive(true);
            selectItem.Initialized(item.Key, item.Value);
        }
    }

    /// <summary>
    /// 删除池子类
    /// </summary>
    private void DestroyPoolChild()
    {
        foreach (Transform item in Pool)
        {
            Destroy(item.gameObject);
        }
    }


    /// <summary>
    /// 加载文件夹内图片
    /// </summary>
    private void load()
    {
        images.Clear();

        List<string> filePaths = new List<string>();

        string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
        string[] ImageType = imgtype.Split('|');

        for (int i = 0; i < ImageType.Length; i++)
        {
            //获取Application.dataPath文件夹下所有的图片路径  
            string[] dirs = Directory.GetFiles((Application.streamingAssetsPath + "/Icons/"), ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                filePaths.Add(dirs[j]);
            }
        }

        for (int i = 0; i < filePaths.Count; i++)
        {
            Texture2D tx = new Texture2D(315, 450);
            tx.LoadImage(getImageByte(filePaths[i]));
            images.Add(filePaths[i], tx);
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
}