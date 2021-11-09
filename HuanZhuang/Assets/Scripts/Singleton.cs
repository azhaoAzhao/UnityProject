using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T mInstance;
    public static T Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = FindObjectOfType(typeof(T)) as T; // 获得场景中有T组件的the first active loaded gameobject of this type
                if (null == mInstance)
                {
                    GameObject singleton = new GameObject(typeof(T).ToString());
                    mInstance = singleton.AddComponent<T>(); // 实例化T的对象
                    DontDestroyOnLoad(singleton);
                }
            }
            return mInstance;
        }
        
    }
}