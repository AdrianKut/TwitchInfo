using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BadgesManager : ContentManager
{
    [Obsolete]
    new public IEnumerator ShowContent()
    {
        string twitchURL = String.Format($"https://api.twitch.tv/helix/chat/badges?broadcaster_id={TwitchApiDataAuthetication.broadcaster_id}");
        TwitchAPI twitchApi = new TwitchAPI(twitchURL);

        yield return twitchApi.SetRequestHeader();

        JSONNode twitchInfo = twitchApi.GetTwitchInfo();

        int indexOfIdSubscriber = 0;
        for (int i = 0; i < twitchInfo["data"].Count; i++)
        {
            if (twitchInfo["data"][i]["set_id"] == "subscriber")
            {
                indexOfIdSubscriber = i;
                break;
            }
        }

        int badgesCount = twitchInfo["data"][indexOfIdSubscriber]["versions"].Count;

        for (int prefabCounter = 0; prefabCounter < badgesCount; prefabCounter++)
        {
            if (twitchInfo["data"][indexOfIdSubscriber]["versions"][prefabCounter]["id"] > 1000)
                continue;

            InstantiateContent();

            // Name of Badges
            currentGameObject.name = twitchInfo["data"][indexOfIdSubscriber]["versions"][prefabCounter]["id"];

            var name = GetNameOfBadges(twitchInfo["data"][indexOfIdSubscriber]["versions"][prefabCounter]["id"]);
            currentName.text = name;

            var imageURL = twitchInfo["data"][indexOfIdSubscriber]["versions"][prefabCounter]["image_url_4x"];

            yield return twitchApi.ShowOneImageFromURL(imageURL, currentImage);
        }
        
        SortAllBadges();
        IsReadyToShow = true;
        TwitchAPIManager.instance.ShowMainInformationView();
    }

    private List<int> GetNumbersOfAllChildren(GameObject parentList)
    {
        List<int> allChildren = new List<int>();
        for (int i = 0; i < parentList.transform.childCount; i++)
            allChildren.Add(int.Parse(parentList.transform.GetChild(i).name));

        return allChildren;
    }

    public void SortAllBadges()
    {
        var tempIntNumberOfBadges = new List<int>();

        for (int i = 0; i < contentList.transform.childCount; i++)
            tempIntNumberOfBadges.Add(int.Parse(contentList.transform.GetChild(i).name));

        tempIntNumberOfBadges.Sort();

        for (int i = 0; i < contentList.transform.childCount; i++)
        {
            var currentBadge = contentList.transform.GetChild(i);
            for (int j = 0; j < tempIntNumberOfBadges.Count; j++)
            {
                if (tempIntNumberOfBadges[j] == int.Parse(currentBadge.name))
                    currentBadge.transform.SetSiblingIndex(j);
            }
        }

        var allChildren = GetNumbersOfAllChildren(contentList);
        if (Enumerable.SequenceEqual(tempIntNumberOfBadges, allChildren) == false)
            SortAllBadges();
    }

    private string GetNameOfBadges(string id)
    {
        if (int.Parse(id) < 12)
            return id + " Month";
        else
            return (float.Parse(id) / 12).ToString().Replace(",", ".") + " Year";
    }


}
