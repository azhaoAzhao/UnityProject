using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 李家豪 用了我的代码这个地方加上我的名字没啥问题吧
/// </summary>
public class EditorTools : EditorWindow
{
    private UnityEngine.Object Source;
    private List<GameObject> Sources = new List<GameObject>();
    private Event SendEvent;

    [MenuItem("Tools/ShowTools")]
    public static void ShowMenu()
    {
        //创建窗口
        GetWindow<EditorTools>("菜单", true);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("当前场景查找脚本引用"))
            GetWindow<EditorFindCurrSceneGameObject>("场景查找脚本引用", true);
        if (GUILayout.Button("查找当前场景所有脚本"))
            GetWindow<EditorSelectAllScript>("场景所有脚本", true);
        if (GUILayout.Button("查找预制体脚本引用(消耗内存过大慎用)"))
            GetWindow<EditorFindPrefabGameObject>("Prefab查找脚本引用", true);
        if (GUILayout.Button("查找所有图片引用"))
            GetWindow<EditorFindPicturesReference>("查找所有图片引用", true);
        if (GUILayout.Button("查找鼠标选中图片引用次数"))
            GetWindow<EditorMouseSelectImageFindPicturesReference>("查找鼠标选中图片引用次数", true);
    }
}