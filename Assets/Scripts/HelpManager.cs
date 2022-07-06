using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectHelpView;

    [SerializeField]
    private Button buttonShowHelpView;

    [SerializeField]
    private Button buttonHideHelpView;

    private void OnEnable()
    {
        buttonShowHelpView.onClick.AddListener(ShowHelpView);
        buttonHideHelpView.onClick.AddListener(HideHelpView);
    }

    private void OnDisable()
    {
        buttonShowHelpView.onClick.RemoveAllListeners();
        buttonHideHelpView.onClick.RemoveAllListeners();
    }

    public void ShowHelpView()
    {
        gameObjectHelpView.transform.localScale = new Vector3(0, 0, 0);
        gameObjectHelpView.transform.LeanScale(new Vector3(1, 1, 1), .25f).setEaseOutQuart();

        gameObjectHelpView.SetActive(true);
        buttonShowHelpView.interactable = false;
    }

    public void HideHelpView()
    {
        gameObjectHelpView.SetActive(false);
        buttonShowHelpView.interactable = true;
    }
}
