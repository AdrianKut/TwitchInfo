using System.Text;
using TMPro;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private TextMeshProUGUI textLoading;
    private StringBuilder stringBuilder;

    private bool canShowLoading = false;
    
    private float timer = 1f;
    private float secToShowNextDot = 1f;
    private short numberOfDots = 3;
    private void Awake()
    {
        textLoading = GetComponent<TextMeshProUGUI>();
        stringBuilder = new StringBuilder();
    }

    private void Start()
    {
        EventsManager.OnLoading.AddListener(Show);
    }

    private void Show()
    {
        canShowLoading = canShowLoading == true ? false : true;

        if (canShowLoading)
        {
            stringBuilder.Clear();
            ShowText("Loading");
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void ShowText(string text)
    {
        textLoading.text = text + " " + stringBuilder.Append('.', 1);
    }
    
    void Update()
    {
        if (canShowLoading)
        {
            if (timer <= 0f)
            {
                timer = secToShowNextDot;

                if (stringBuilder.Length == numberOfDots)
                {
                    stringBuilder.Clear();
                    ShowText("Loading");
                }
                else
                    ShowText("Loading");
            }
            else
                timer -= Time.deltaTime;
        }
    }

}
