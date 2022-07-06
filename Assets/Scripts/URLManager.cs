using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class URLManager : EventTrigger
{
    public string URL { get; set; }

    private EventTrigger trigger;

    private void Awake()
    {
        trigger = GetComponent<EventTrigger>();
    }

    public override void OnPointerClick(PointerEventData data)
    {
        Application.OpenURL(URL);
    }
}
