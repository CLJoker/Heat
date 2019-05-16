using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SA;

public class LoadAvatarOnStart : MonoBehaviour {

    public RawImage avatar;
    public Texture2D temp;

    // Use this for initialization
    void Start () {
        byte[] avatarData = MultiplayerManager.singleton.GetMRef().localPlayer.print.avatarData;
        if (avatarData != null)
        {
            Texture2D texture = temp;
            texture.LoadImage(avatarData);
            texture.Apply();
            avatar.texture = texture;
        }
    }
}
