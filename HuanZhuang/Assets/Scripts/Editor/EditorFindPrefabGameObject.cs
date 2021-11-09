using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 查找GameObject引用的脚本
/// </summary>
public class EditorFindPrefabGameObject : EditorWindow
{
    private MonoScript Source;
    private List<GameObject> _list = new List<GameObject>();

    private void OnGUI()
    {
        Source = (MonoScript)EditorGUILayout.ObjectField("Script", Source, typeof(MonoScript));
        if (GUILayout.Button("查找Prefab脚本引用"))
            FindAllPrefabReference();

        if (_list.Count == 0) return;
        foreach (var item in _list)
        {
            EditorGUILayout.ObjectField("GameObject", item, typeof(GameObject));
        }
    }

    /// <summary>
    /// 查找所有脚本引用
    /// </summary>
    private void FindAllPrefabReference()
    {
        if (Source == null)
        {
            EditorUtility.DisplayDialog("提示信息", "脚本引用不能为空", "确定");
            return;
        }

        _list.Clear();

        GameObject[] prefabs = Resources.FindObjectsOfTypeAll<GameObject>();
        float index = 0f;
        foreach (var item in prefabs)
        {
            index++;
            EditorUtility.DisplayProgressBar("获取预制体……", "查找引用中……", index / prefabs.Length);
            if (item.scene.name == null && item.GetComponent(Source.GetClass()) != null)
            {
                _list.Add(item);
            }
        }

        EditorUtility.ClearProgressBar();
    }
}