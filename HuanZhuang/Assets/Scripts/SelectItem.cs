using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public RawImage RawImage;
    public PrintPanel panel;

    private Texture2D _texture2D;
    private string Path;

    private void Start()
    {
        RawImage.GetComponent<Button>().onClick.AddListener(() => { panel.Initialized(Path, _texture2D); });
    }

    public void Initialized(string path, Texture2D texture2D)
    {
        RawImage.texture = texture2D;
        _texture2D = texture2D;
        Path = path;
    }
}