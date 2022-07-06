using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
