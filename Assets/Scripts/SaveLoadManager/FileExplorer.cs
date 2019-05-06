using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.FB;

public class FileExplorer : MonoBehaviour {

    [HideInInspector]
    public string path;
    public RawImage avatar;

	public void OpenImage()
    {
        path = FileBrowser.OpenSingleFile("", "", "png", "jpg");
        SaveAvatar();
    }

    private void SaveAvatar()
    {
        WWW www = new WWW("file:///" + path);
        avatar.texture = www.texture;
    }
}
