using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 鼠标选中图片查找引用
/// </summary>
public class EditorMouseSelectImageFindPicturesReference : EditorWindow
{
    private List<GameObject> _list = new List<GameObject>();
    private Dictionary<string, Data> spriteInfos = new Dictionary<string, Data>();

    private void OnGUI()
    {
        if (GUILayout.Button("确定选中图片"))
        {
            spriteInfos.Clear(); //每次选中后要清空一次
            foreach (var item in Selection.objects)
            {
                if (item is Texture2D)
                {
                    Texture2D t2d = item as Texture2D;
                    spriteInfos.Add(t2d.name, new Data(t2d.name, t2d, 0));
                }
            }

            FindReferenceCount();
        }

        if (spriteInfos.Count == 0) return;
        foreach (var item in spriteInfos)
        {
            EditorGUILayout.ObjectField("Sprite", item.Value.sprite, typeof(Sprite));
            EditorGUILayout.LabelField(item.Value.name + "引用次数:" + item.Value.referenceCount);
        }
    }

    private void FindReferenceCount()
    {
        if (spriteInfos.Count == 0)
        {
            EditorUtility.DisplayDialog("提示信息", "未选中任何Texture2D/图片", "确定");
            return;
        }

        Image[] images = Resources.FindObjectsOfTypeAll<Image>();
        float index = 0f;
        foreach (var item in images)
        {
            index++;
            EditorUtility.DisplayProgressBar("查找Image组件引用", "检查图片引用信息", index / images.Length);
            if (spriteInfos.ContainsKey(item.sprite.name))
            {
                spriteInfos[item.sprite.name].referenceCount++;
            }
        }

        EditorUtility.ClearProgressBar();
        //检查RawImage组件信息
        RawImage[] rawImages = Resources.FindObjectsOfTypeAll<RawImage>();
        float index1 = 0f;
        foreach (var item in rawImages)
        {
            index1++;
            EditorUtility.DisplayProgressBar("查找RawImage组件引用", "检查图片引用信息", index1 / images.Length);
            if (spriteInfos.ContainsKey(item.texture.name))
            {
                spriteInfos[item.texture.name].referenceCount++;
            }
        }

        EditorUtility.ClearProgressBar();
    }

    public class Data
    {
        public string name; //图片名字
        public Texture2D sprite; //图片
        public int referenceCount; //引用次数

        public Data(string name, Texture2D sprite, int referenceCount)
        {
            this.name = name;
            this.sprite = sprite;
            this.referenceCount = referenceCount;
        }
    }
}