using UnityEngine;

public class PopUpEffect : MonoBehaviour
{
    [SerializeField]
    private float effectSpeed = 2f;

    [SerializeField]
    private Vector3 vector3 = new Vector3(0.7f,0.7f,0.7f);

    private RectTransform transfom;

    private void Start()
    {      
        PopUp();
    }

    private void OnEnable()
    {
        PopUp();
    }

    private void PopUp()
    {
        transfom = this.gameObject.GetComponent<RectTransform>();
        transfom.localScale = new Vector3(0, 0, 0);
        this.gameObject.LeanScale(vector3, 1f).setEaseOutQuart();

    }
}