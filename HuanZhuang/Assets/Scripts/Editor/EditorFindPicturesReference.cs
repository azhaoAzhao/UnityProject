using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 查找图片引用
/// </summary>
public class EditorFindPicturesReference : EditorWindow
{
    private Texture2D Source;

    private List<GameObject> Prefab = new List<GameObject>();

    private void OnGUI()
    {
        Source = (Texture2D) EditorGUILayout.ObjectField("Sprite", Source, typeof(Texture2D));
        if (GUILayout.Button("查找引用"))
            FindPicturesReference();

        if (Prefab.Count == 0) return;
        foreach (var item in Prefab)
        {
            EditorGUILayout.ObjectField("GameObject", item, typeof(GameObject));
        }
    }

    /// <summary>
    /// 查找图片引用
    /// </summary>
    private void FindPicturesReference()
    {
        if (Source == null)
        {
            EditorUtility.DisplayDialog("提示信息", "图片引用不能为空", "确定");
            return;
        }

        Prefab.Clear();
        Image[] images = Resources.FindObjectsOfTypeAll<Image>();
        float index = 0f;
        foreach (var item in images)
        {
            index++;
            EditorUtility.DisplayCancelableProgressBar("查找所有Image组件", "匹配Image.Sprite是否符合当前引用图片", index / images.Length);
            if (item.sprite.name != Source.name) continue;
            Prefab.Add(item.gameObject);
        }

        RawImage[] rawImages = Resources.FindObjectsOfTypeAll<RawImage>();
        float index1 = 0f;
        foreach (var item in rawImages)
        {
            index++;
            EditorUtility.DisplayCancelableProgressBar("查找所有Image组件", "匹配Image.Sprite是否符合当前引用图片", index1 / images.Length);
            if (item.texture.name != Source.name) continue;
            Prefab.Add(item.gameObject);
        }

        EditorUtility.ClearProgressBar(); //关闭进度条
    }
}