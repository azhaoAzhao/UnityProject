using System.Collections.Generic;
/*
 * @Descripttion: 语法糖
 * @version: 
 * @Author: LiJiaHao
 * @Date: 2021-03-10 17:44:10
 * @LastEditors: LiJiaHao
 * @LastEditTime: 2021-05-18 11:18:03
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

public static class UITools
{
    #region Button按钮语法糖
    /// <summary>
    /// 添加按钮唯一单击事件
    /// </summary>
    /// <param name="button"></param>
    /// <param name="action"></param>
    public static void AddListener(this Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    /// <summary>
    /// 添加按钮不唯一单击事件
    /// </summary>
    /// <param name="button"></param>
    /// <param name="action"></param>
    public static void AddListenerNotOn(this Button button, UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    /// <summary>
    /// 修改按钮文本
    /// </summary>
    /// <param name="button"></param>
    public static void SetText(this Button button, string str)
    {
        var text = button.Find("Text").GetComponent<Text>();
        if (text != null)
        {
            text.text = str;
        }
    }
    #endregion
    #region Toggle开关语法糖
    /// <summary>
    /// Toggle添加唯一事件
    /// </summary>
    /// <param name="toggle"></param>
    /// <param name="action"></param>
    public static void AddListener(this Toggle toggle, UnityAction<bool> action)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(action);
    }

    /// <summary>
    /// Toggle添加不唯一事件
    /// </summary>
    /// <param name="toggle"></param>
    /// <param name="action"></param>
    public static void AddListenerNotOn(this Toggle toggle, UnityAction<bool> action)
    {
        toggle.onValueChanged.AddListener(action);
    }

    /// <summary>
    /// 删除所有事件
    /// </summary>
    /// <param name="toggle"></param>
    public static void RemoveAllListeners(this Toggle toggle) => toggle.onValueChanged.RemoveAllListeners();

    /// <summary>
    /// 修改Toggle文本
    /// </summary>
    /// <param name="button"></param>
    public static void SetText(this Toggle button, string str)
    {
        var text = button.Find("Label").GetComponent<Text>();
        if (text != null)
        {
            text.text = str;
        }
    }
    #endregion

    #region SetActive语法糖
    /// <summary>
    /// SetActive扩展
    /// </summary>
    /// <param name="t">所有继承MonobeHaviour的类都可以使用</param>
    /// <param name="s">value</param>
    /// <typeparam name="T">所有继承MonobeHaviour的类</typeparam>
    public static void SetActive(this MonoBehaviour t, bool s) => t.gameObject.SetActive(s);

    public static void SetActive(this Transform t, bool s) => t.gameObject.SetActive(s);
    #endregion

    #region SetParent语法糖
    /// <summary>
    /// SetParent扩展
    /// </summary>
    /// <param name="t">所有继承MonobeHaviour的类都可以使用</param>
    /// <param name="p">设置t的父级目标</param>
    /// <typeparam name="T">所有继承MonobeHaviour的类</typeparam>
    public static void SetParent(this MonoBehaviour t, Transform p)
    {
        t.transform.SetParent(p, false);
        t.SetActive(true);
    }

    public static void SetParent(this Transform t, Transform p)
    {
        t.transform.SetParent(p, false);
        t.SetActive(true);
    }

    public static void SetParent(this GameObject g, Transform p)
    {
        g.transform.SetParent(p, false);
        g.SetActive(true);
    }
    #endregion

    #region 杂项工具
    /// <summary>
    /// 查找
    /// </summary>
    /// <param name="t"></param>
    /// <param name="targetName"></param>
    /// <returns></returns>
    public static Transform Find(this MonoBehaviour t, string targetName) => t.transform.Find(targetName);

    /// <summary>
    /// 数字转字母
    /// </summary>
    public static string DigitalTransfromLetter(int i)
    {
        string Letter = "";
        switch (i)
        {
            case 1:
                Letter = "A";
                break;
            case 2:
                Letter = "B";
                break;
            case 3:
                Letter = "C";
                break;
            case 4:
                Letter = "D";
                break;
        }
        return Letter;
    }

    /// <summary>
    /// 颜色转换
    /// </summary>
    /// <param name="htmlString"></param>
    /// <returns></returns>
    public static Color TransformColor(string htmlString)
    {
        Color color;
        ColorUtility.TryParseHtmlString(htmlString, out color);
        return color;
    }

    /// <summary>
    /// 缩放图片
    /// </summary>
    /// <param name="image"></param>
    /// <param name="destHeight"></param>
    /// <param name="destWidth"></param>
    public static void ZoomImage(this Image image, float destHeight, float destWidth)
    {
        image.SetNativeSize();


        float width = 0, height = 0;
        //按比例缩放
        var sizeDelta = image.GetComponent<RectTransform>().sizeDelta;
        float sourWidth = sizeDelta.x;
        float sourHeight = sizeDelta.y;

        if (sourHeight > destHeight || sourWidth > destWidth)
        {

            if ((sourWidth * destHeight) > (sourHeight * destWidth))
            {
                width = destWidth;
                height = (destWidth * sourHeight) / sourWidth;
            }
            else
            {
                height = destHeight;
                width = (sourWidth * destHeight) / sourHeight;
            }

        }
        else
        {
            width = sourWidth;
            height = sourHeight;
        }

        image.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    /// <summary>
    /// 格式化json字符串
    /// </summary>
    /// <returns></returns>
    public static string ConvertJsonString(string str)
    {
        var json = new JsonSerializer().Deserialize(new JsonTextReader(new StringReader(str))).ToString();//去除斜杠

        return json;
    }

    /// <summary>
    /// 数据分页 页数没有0从1开始
    /// </summary>
    /// <param name="data">数据源</param>
    /// <param name="pageNumber">多少为一页</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Dictionary<int, List<T>> DataPage<T>(this List<T> data, int pageNumber)
    {
        Dictionary<int, List<T>> pageData = new Dictionary<int, List<T>>();

        if (data.Count == 0)
        {
            pageData.Add(1, new List<T>());
            return pageData;
        }

        int i = 1;//循环次数
        int page = 1;//当前页数
        foreach (var item in data)
        {
            if (pageData.ContainsKey(page))
            {
                pageData[page].Add(item);
                if (i == pageNumber)
                {
                    i = 0;
                    page++;
                }
            }
            else
            {
                pageData.Add(page, new List<T> { item });
            }
            i++;
        }

        return pageData;
    }

    /// <summary>
    /// 图片添加旋转
    /// </summary>
    /// <param name="image"></param>
    public static void ADDTurnAround(this Image image)
    {
        // image.gameObject.AddComponent<TurnAround>();
    }
    #endregion

    #region 读写json文件 地址可更改
    public static readonly string PATH = Application.streamingAssetsPath + "/Data/";
    /// <summary>
    /// 获取json字符串
    /// </summary>
    /// <param name="fileName">json文件名称</param>
    /// <returns>json字符串</returns>
    public static string GetJsonString(string fileName)
    {
        string json = default;
        string path = PATH + fileName + ".json";
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            return json;
        }
        else
        {
            Debug.LogError("Not Find JsonFile FileName:" + fileName);
            Debug.LogError("PATH" + path);
        }

        return json;
    }

    /// <summary>
    /// 写入本地json文件
    /// </summary>
    /// <param name="str">json</param>
    /// <param name="fileName">写入json文件的名称</param>
    public static void SetJson(string str, string fileName)
    {
        string path = PATH + fileName + ".json";
        if (File.Exists(path))
        {
            string json = UITools.ConvertJsonString(str);
            FileInfo file = new FileInfo(path);
            StreamWriter sw = file.CreateText();
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();//释放资源
        }
        else
        {
            Debug.LogError("Not Find JsonFile FileName:" + fileName);
            Debug.LogError("PATH" + path);
        }
    }


    #endregion
}

public interface IInitialized
{
    void Initialized();
}

public enum TouchDragType
{
    Up,
    Bottom,
    Left,
    Right
}
public enum TouchHorizontal
{
    Vertical,//垂直
    Horizontal//横向
}

public class DataPageCreateItem<T>
{
    public Dictionary<int, List<T>> Data;
    public Transform Content;
    public GameObject Item;
    public Action<T, GameObject> CallBack;
    public int APageItemNumber;//一页Item的数量

    private List<GameObject> Items = new List<GameObject>();


    /// <summary>
    /// 根据分页数据创建Item
    /// </summary>
    /// <param name="pageItemNumber"></param>
    public void CreatePateDataItem(int pageIndex)
    {
        if (Items.Count == 0)
        {
            CreateItem();
        }
        else
        {
            HideAllItem();
        }

        var data = Data[pageIndex];
        int i = 0;
        foreach (var item in data)
        {
            var prefab = Items[i];
            prefab.SetActive(true);
            if (CallBack != null)
            {
                CallBack(item, prefab);
            }
            i++;
        }
    }

    /// <summary>
    /// 创建Item
    /// </summary>
    private void CreateItem()
    {
        Item.SetActive(false);

        for (int i = 0; i < APageItemNumber; i++)
        {
            // var prefab = MonoTools.Instance.InstantiateGM(Item, Content);
            // Items.Add(prefab);
        }
    }
    /// <summary>
    /// 隐藏所有
    /// </summary>
    private void HideAllItem()
    {
        foreach (var item in Items)
        {
            item.SetActive(false);
        }
    }
}