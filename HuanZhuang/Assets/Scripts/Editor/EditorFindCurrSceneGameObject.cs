using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

/// <summary>
/// 查找GameObject引用的脚本
/// </summary>
public class EditorFindCurrSceneGameObject : EditorWindow
{
    private MonoScript Source;
    private List<GameObject> _list = new List<GameObject>();

    private void OnGUI()
    {
        Source = (MonoScript) EditorGUILayout.ObjectField("Script", Source, typeof(MonoScript));

        if (GUILayout.Button("查找"))
            FindCurrSceneGameobject();

        if (_list.Count == 0) return;
        foreach (var item in _list)
            EditorGUILayout.ObjectField(item.name, item, typeof(GameObject));
    }

    /// <summary>
    /// 查找Gameobject
    /// </summary>
    private void FindCurrSceneGameobject()
    {
        if (Source == null)
        {
            EditorUtility.DisplayDialog("提示信息", "脚本引用不能为空", "确定");
            return;
        }

        _list.Clear(); //每次查找都要清空
        string sceneName = SceneManager.GetActiveScene().name;
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        float index = 0;
        foreach (GameObject item in objects)
        {
            index++;
            EditorUtility.DisplayCancelableProgressBar("查找脚本引用", "查找中", index / objects.Length);
            if (item.GetComponent(Source.GetClass()) != null && item.scene.name == sceneName)
                _list.Add(item);
        }

        EditorUtility.ClearProgressBar(); //关闭进度条


        if (_list.Count == 0)
            EditorUtility.DisplayDialog("提示信息", "当前场景未查找到引用", "确定");
    }
}