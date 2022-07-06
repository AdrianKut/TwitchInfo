using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideosManager : ContentManager
{
    [Obsolete]
    new public IEnumerator ShowContent()
    {
        string twitchURL = String.Format($"https://api.twitch.tv/helix/videos?user_id={TwitchApiDataAuthetication.broadcaster_id}&first=10");
        TwitchAPI twitchApi = new TwitchAPI(twitchURL);
        yield return twitchApi.SetRequestHeader();

        JSONNode twitchInfo = twitchApi.GetTwitchInfo();
        int videosCounter = twitchInfo["data"].Count;

        for (int prefabCounter = 0; prefabCounter < videosCounter; prefabCounter++)
        {
            var currentVideoGameObject = Instantiate(prefabToInstantiate, contentList.transform);
            var currentVideoImage = currentVideoGameObject.GetComponent<RawImage>();
            var currentVideoName = currentVideoImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            currentVideoGameObject.name = twitchInfo["data"][prefabCounter]["title"];

            currentVideoName.text = twitchInfo["data"][prefabCounter]["title"];
            currentVideoName.text += "\nDURATION: " + twitchInfo["data"][prefabCounter]["duration"];
            string createdDate = twitchInfo["data"][prefabCounter]["created_at"];
            currentVideoName.text += "\nCREATED ON: " + createdDate.Remove(10).ToString().Replace("-", ".");
            currentVideoGameObject.transform.GetChild(4).GetComponent<URLManager>().URL = (twitchInfo["data"][prefabCounter]["url"]);

            string imageURL = twitchInfo["data"][prefabCounter]["thumbnail_url"];
            imageURL = imageURL.ToString().Replace("%{width}x%{height}.jpg", "480x272.jpg");

            yield return twitchApi.ShowOneImageFromURL(imageURL, currentVideoImage);
        }

        if (videosCounter > 0)
        {
            var currentButtonMoreClips = Instantiate(buttonMore, contentList.transform);
            currentButtonMoreClips.transform.GetChild(1).GetComponent<URLManager>().URL = (string.Format($"https://www.twitch.tv/{TwitchAPIManager.instance.Nickname}/videos?filter=archives"));
        }

        IsReadyToShow = true;
        TwitchAPIManager.instance.ShowMainInformationView();
    }
}
