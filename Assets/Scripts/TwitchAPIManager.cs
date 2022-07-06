using SimpleJSON;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TwitchAPIManager : MonoBehaviour
{
    [Header("Loading View")]
    [SerializeField]
    private GameObject gameObjectLoadingView;

    [Space]
    [SerializeField]
    private TMP_InputField inputFieldNickname;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUIErrorInfo;

    [SerializeField]
    private Button buttonSearch;

    [Header("Main Information View")]
    [SerializeField]
    private GameObject gameObjectMainInformationView;
    [SerializeField]
    private RawImage avatar;
    [SerializeField]
    private TextMeshProUGUI username;
    [SerializeField]
    private TextMeshProUGUI textFieldAccountType;
    [SerializeField]
    private TextMeshProUGUI textFieldViewCount;
    [SerializeField]
    private TextMeshProUGUI textFieldCreatedAt;
    [SerializeField]
    private TextMeshProUGUI textFieldTotalFollows;
    [SerializeField]
    private TextMeshProUGUI textFieldTotalFollowers;

    [Header("Emotes")]
    [SerializeField]
    private EmotesManager emotesManager;
    [SerializeField]
    private GameObject gameObjectEmotesView;

    [Header("Badges")]
    [SerializeField]
    private BadgesManager badgesManager;
    [SerializeField]
    private GameObject gameObjectBadgesView;

    [Header("Clips")]
    [SerializeField]
    private ClipsManager clipsManager;
    [SerializeField]
    private GameObject gameObjectClipsView;

    [Header("Videos")]
    [SerializeField]
    private VideosManager videosManager;
    [SerializeField]
    private GameObject gameObjectVideosView;

    public string Nickname { get; private set; }
    [Obsolete]
    public void SearchByNickname()
    {
        StartCoroutine(Search());
    }

    public static TwitchAPIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void BackToMainView()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void BackToMainInformationView()
    {
        gameObjectEmotesView.SetActive(false);
        gameObjectBadgesView.SetActive(false);
        gameObjectClipsView.SetActive(false);
        gameObjectVideosView.SetActive(false);

        gameObjectMainInformationView.SetActive(true);
    }


    [Obsolete]
    IEnumerator JSONNOde(string twitchURL, TwitchAPI twitchAPI, JSONNode twitchInfo)
    {
        twitchURL = string.Format($"https://api.twitch.tv/helix/users/follows?from_id={TwitchApiDataAuthetication.broadcaster_id}");
        twitchAPI = new TwitchAPI(twitchURL);
        yield return twitchAPI.SetRequestHeader();
        twitchInfo = twitchAPI.GetTwitchInfo();
    }

    [Obsolete]
    private IEnumerator Search()
    {
        Nickname = inputFieldNickname.text;

        if (string.IsNullOrEmpty(Nickname) == false)
        {
            buttonSearch.interactable = false;

            string twitchURL = string.Format($"https://api.twitch.tv/helix/search/channels?query={Nickname}&first=1");
            TwitchAPI twitchAPI = new TwitchAPI(twitchURL);
            yield return twitchAPI.SetRequestHeader();

            if (twitchAPI.IsNetworkErrorOrIsHttpError())
            {
                textMeshProUGUIErrorInfo.text = twitchAPI.ErrorText();
                buttonSearch.interactable = true;
                yield break;
            }

            JSONNode twitchInfo = twitchAPI.GetTwitchInfo();

            if (twitchInfo["data"].Count > 0)
            {
                EventsManager.OnLoading?.Invoke();
                inputFieldNickname.text = "";

                gameObjectLoadingView.SetActive(false);

                //Username
                username.text = twitchInfo["data"][0]["display_name"];
                Nickname = username.text;

                //Avatar
                var imageURL = twitchInfo["data"][0]["thumbnail_url"];
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageURL);
                yield return request.SendWebRequest();
                avatar.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                TwitchApiDataAuthetication.broadcaster_id = (twitchInfo["data"][0]["id"]);

                //Total Follows
                twitchURL = string.Format($"https://api.twitch.tv/helix/users/follows?from_id={TwitchApiDataAuthetication.broadcaster_id}");
                twitchAPI = new TwitchAPI(twitchURL);
                yield return twitchAPI.SetRequestHeader();
                twitchInfo = twitchAPI.GetTwitchInfo();

                textFieldTotalFollows.text = "TOTAL FOLLOWS: " + twitchInfo["total"];

                //Total Followers
                twitchURL = string.Format($"https://api.twitch.tv/helix/users/follows?to_id={TwitchApiDataAuthetication.broadcaster_id}");
                twitchAPI = new TwitchAPI(twitchURL);
                yield return twitchAPI.SetRequestHeader();
                twitchInfo = twitchAPI.GetTwitchInfo();

                textFieldTotalFollowers.text = "TOTAL FOLLOWERS: " + GetValueToSplit(twitchInfo["total"]);


                //Account Type, View Count, Created At
                twitchURL = string.Format($"https://api.twitch.tv/helix/users?id={TwitchApiDataAuthetication.broadcaster_id}");
                twitchAPI = new TwitchAPI(twitchURL);
                yield return twitchAPI.SetRequestHeader();
                twitchInfo = twitchAPI.GetTwitchInfo();

                var accountType = twitchInfo["data"][0]["broadcaster_type"];
                textFieldAccountType.text = accountType != "" ? textFieldAccountType.text = "ACCOUNT TYPE: " + accountType.ToString().ToUpper().Replace("\"", "") : "ACCOUNT TYPE: " + "USER";

                textFieldViewCount.text = "VIEW COUNT: " + GetValueToSplit(twitchInfo["data"][0]["view_count"]);

                string createdDate = twitchInfo["data"][0]["created_at"];
                textFieldCreatedAt.text = "CREATED ON: " + createdDate.Remove(10).ToString().Replace("-", ".");

                StartCoroutine(emotesManager.ShowContent());
                StartCoroutine(badgesManager.ShowContent());
                StartCoroutine(clipsManager.ShowContent());
                StartCoroutine(videosManager.ShowContent());
            }
            else
            {
                ShowErrorMessage();
            }
        }
        else
        {
            ShowErrorMessage();
        }
    }

    private void ShowErrorMessage()
    {
        inputFieldNickname.text = "";
        textMeshProUGUIErrorInfo.text = "This username doesn't exist!";
        buttonSearch.interactable = true;
    }


    public void GameObjectToActivate(GameObject contentListToShow, GameObject gameObjectViewToShow)
    {
        gameObjectMainInformationView.SetActive(false);

        contentListToShow.transform.localPosition = new Vector3(contentListToShow.transform.localPosition.x, -5000, 0);
        contentListToShow.SetActive(true);
        gameObjectViewToShow.SetActive(true);
    }

    //From 123456 to 123 456 
    private string GetValueToSplit(string valueToChange)
    {
        string numbers = "";
        int counter = 3;
        foreach (var item in valueToChange.Reverse())
        {
            if (counter == 0)
            {
                counter = 3;
                numbers += " " + item;
            }
            else
                numbers += "" + item;

            counter--;
        }

        return String.Join("", numbers.Reverse().ToList());
    }

    public void ShowMainInformationView()
    {
        if (emotesManager.IsReadyToShow && badgesManager.IsReadyToShow && clipsManager.IsReadyToShow && videosManager.IsReadyToShow)
        {
            emotesManager.IsReadyToShow = false;
            badgesManager.IsReadyToShow = false;
            clipsManager.IsReadyToShow = false;
            videosManager.IsReadyToShow = false;

            gameObjectMainInformationView.SetActive(true);
            EventsManager.OnLoading?.Invoke();
        }
    }
}