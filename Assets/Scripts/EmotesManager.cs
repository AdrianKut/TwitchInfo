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
            var currentEmoteGameObject = Instantiate(prefabToInstantiate, contentList.transform);
            var currentEmoteImage = currentEmoteGameObject.GetComponent<RawImage>();
            var currentEmoteName = currentEmoteImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            string emoteName = twitchInfo["data"][prefabCounter]["name"];
            currentEmoteName.text = emoteName;


            var imageURL = twitchInfo["data"][prefabCounter]["images"]["url_4x"];
            yield return twitchApi.ShowOneImageFromURL(imageURL, currentEmoteImage);
        }

        IsReadyToShow = true;
        TwitchAPIManager.instance.ShowMainInformationView();
    }
}
