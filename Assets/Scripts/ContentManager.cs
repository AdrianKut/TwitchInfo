using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ContentManager :MonoBehaviour
{
    [SerializeField]
    protected GameObject prefabToInstantiate;

    [SerializeField]
    protected GameObject contentList;

    [SerializeField]
    protected GameObject emptyPrefab;

    [SerializeField]
    protected GameObject buttonMore;

    public bool IsReadyToShow { get; set; } = false;

    public void ShowContent() { }
    
    protected GameObject currentGameObject;
    protected RawImage currentImage;
    protected TextMeshProUGUI currentName;

    public void InstantiateContent()
    {
        currentGameObject = Instantiate(prefabToInstantiate, contentList.transform);
        currentImage = currentGameObject.GetComponent<RawImage>();
        currentName = currentImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}
