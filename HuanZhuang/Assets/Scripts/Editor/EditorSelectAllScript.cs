using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class EditorSelectAllScript : EditorWindow
{

    private List<GameObject> Item = new List<GameObject>();
    private void OnGUI()
    {
        if (GUILayout.Button("查找当前场景所有脚本"))
        {
            GetCurrSceneAllScript();
        }
        if (Item.Count == 0) return;
        foreach (var item in Item)
            EditorGUILayout.ObjectField(item.name, item, typeof(GameObject));
    }


    /// <summary>
    /// 获取当前场景所有脚本
    /// </summary>
    private void GetCurrSceneAllScript()
    {
        Item.Clear();

        string sceneName = SceneManager.GetActiveScene().name;
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));

        int index = 0;

        foreach (GameObject item in objects)
        {
            index++;
            EditorUtility.DisplayCancelableProgressBar("查找脚本引用", "查找中", index / objects.Length);
            if (item.scene.name == sceneName && item.GetComponent<MonoBehaviour>())
            {
                Item.Add(item);
            }
        }
        EditorUtility.ClearProgressBar(); //关闭进度条
    }
}
