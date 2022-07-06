using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClipsManager : ContentManager
{
    [Obsolete]
    new public IEnumerator ShowContent()
    {
        string twitchURL = String.Format($"https://api.twitch.tv/helix/clips?broadcaster_id={TwitchApiDataAuthetication.broadcaster_id}&first=10");
        TwitchAPI twitchApi = new TwitchAPI(twitchURL);
        yield return twitchApi.SetRequestHeader();

        JSONNode twitchInfo = twitchApi.GetTwitchInfo();

        int clipsCount = twitchInfo["data"].Count;

        for (int prefabCounter = 0; prefabCounter < clipsCount; prefabCounter++)
        {
            var currentClipGameObject = Instantiate(prefabToInstantiate, contentList.transform);
            var currentClipImage = currentClipGameObject.GetComponent<RawImage>();
            var currentClipName = currentClipImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
      
            currentClipGameObject.name = twitchInfo["data"][prefabCounter]["title"];

            currentClipName.text = twitchInfo["data"][prefabCounter]["title"];
            currentClipName.text += "\nVIEW COUNT: " + twitchInfo["data"][prefabCounter]["view_count"];
            currentClipName.text += "\nCREATED BY: " + twitchInfo["data"][prefabCounter]["creator_name"];

            currentClipGameObject.transform.GetChild(4).GetComponent<URLManager>().URL = (twitchInfo["data"][prefabCounter]["url"]);

            var imageURL = twitchInfo["data"][prefabCounter]["thumbnail_url"];
            yield return twitchApi.ShowOneImageFromURL(imageURL, currentClipImage);
        }

        if (clipsCount > 0)
        {
            var currentButtonMoreClips = Instantiate(buttonMore, contentList.transform);
            currentButtonMoreClips.transform.GetChild(1).GetComponent<URLManager>().URL = (string.Format($"https://www.twitch.tv/{TwitchAPIManager.instance.Nickname}/clips?filter=clips&range=all"));
        }

        IsReadyToShow = true;
        TwitchAPIManager.instance.ShowMainInformationView();

    }
}
