using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TwitchAPI
{
    private int broadcaster_id;
    private string twitchApiURL;

    private UnityWebRequest twitchInfoRequest;
    private JSONNode twitchInfo;

    public TwitchAPI(string twitchApiURL)
    {
        this.broadcaster_id = TwitchApiDataAuthetication.broadcaster_id;
        this.twitchApiURL = twitchApiURL;

        this.twitchInfoRequest = UnityWebRequest.Get(twitchApiURL);
    }


    [Obsolete]
    public IEnumerator SetRequestHeader()
    {
        twitchInfoRequest.SetRequestHeader("Authorization", TwitchApiDataAuthetication.BearerToken);
        twitchInfoRequest.SetRequestHeader("Client-Id", TwitchApiDataAuthetication.ClientID);

        yield return twitchInfoRequest.SendWebRequest();
        if (IsNetworkErrorOrIsHttpError())
        {
            Debug.LogError(twitchInfoRequest.error);
            yield break;
        }

        twitchInfo = JSON.Parse(twitchInfoRequest.downloadHandler.text);
    }

    [Obsolete]
    public bool IsNetworkErrorOrIsHttpError()
    {
        if (twitchInfoRequest.isNetworkError || twitchInfoRequest.isHttpError)
            return true;
        else
            return false;
    }

    public string ErrorText()
    {
        return twitchInfoRequest.error;
    }

    public JSONNode GetTwitchInfo()
    {
        return JSON.Parse(twitchInfoRequest.downloadHandler.text);
    }


    [Obsolete]
    public IEnumerator ShowOneImageFromURL(JSONNode imageURL, RawImage image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageURL);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

}