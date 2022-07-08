using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmotesManager : ContentManager
{
    [Obsolete]
    new public IEnumerator ShowContent()
    {
        string twitchURL = String.Format($"https://api.twitch.tv/helix/chat/emotes?broadcaster_id={TwitchApiDataAuthetication.broadcaster_id}");
        TwitchAPI twitchApi = new TwitchAPI(twitchURL);
        yield return twitchApi.SetRequestHeader();

        JSONNode twitchInfo = twitchApi.GetTwitchInfo();

        int emoteCount = twitchInfo["data"].Count;
        for (int prefabCounter = 0; prefabCounter < emoteCount; prefabCounter++)
        {
            InstantiateContent();

            string emoteName = twitchInfo["data"][prefabCounter]["name"];
            currentName.text = emoteName;

            var imageURL = twitchInfo["data"][prefabCounter]["images"]["url_4x"];
            yield return twitchApi.ShowOneImageFromURL(imageURL, currentImage);
        }

        IsReadyToShow = true;
        TwitchAPIManager.instance.ShowMainInformationView();
    }
}
