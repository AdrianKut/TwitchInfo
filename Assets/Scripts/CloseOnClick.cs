using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOnClick : MonoBehaviour
{
    public void DestroyOnClick()
    {
        this.transform.LeanScale(new Vector3(0, 0, 0), .25f).setEaseOutQuart();
        Destroy(this.gameObject,1f);
    }

}
