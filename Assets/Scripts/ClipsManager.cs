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
            InstantiateContent();

            currentGameObject.name = twitchInfo["data"][prefabCounter]["title"];

            currentName.text = twitchInfo["data"][prefabCounter]["title"];
            currentName.text += "\nVIEW COUNT: " + twitchInfo["data"][prefabCounter]["view_count"];
            currentName.text += "\nCREATED BY: " + twitchInfo["data"][prefabCounter]["creator_name"];

            currentGameObject.transform.GetChild(4).GetComponent<URLManager>().URL = (twitchInfo["data"][prefabCounter]["url"]);

            var imageURL = twitchInfo["data"][prefabCounter]["thumbnail_url"];
            yield return twitchApi.ShowOneImageFromURL(imageURL, currentImage);
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
