using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSelectedView : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectMainInformationView;

    [SerializeField]
    private GameObject gameObjectToShow;

    [SerializeField]
    private GameObject contentListToShow;

    private Button button;
    
    private void Awake()
    {
        button = this.gameObject.GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ShowView);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }


    public void ShowView()
    {
        gameObjectMainInformationView.SetActive(false);

        contentListToShow.transform.localPosition = new Vector3(contentListToShow.transform.localPosition.x, -5000, 0);
        contentListToShow.SetActive(true);
        gameObjectToShow.SetActive(true);
    }

}
