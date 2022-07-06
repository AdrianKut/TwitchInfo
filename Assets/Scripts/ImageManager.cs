using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectToShowOnMiddle;

    private GameObject gameObjectParent;
    private RawImage rawImage;

    private void Awake()
    {
        rawImage = this.gameObject.GetComponent<RawImage>();
    }

    void Start()
    {
        if (gameObjectParent == null)
        {
            gameObjectParent = GameObject.Find("Canvas").gameObject;
        }
    }

    public void IncreaseSize()
    {
        var currentImageGameObject = Instantiate(gameObjectToShowOnMiddle);
        currentImageGameObject.transform.SetParent(gameObjectParent.transform, false);
        currentImageGameObject.gameObject.GetComponent<RawImage>().texture = rawImage.texture;
        currentImageGameObject.transform.localScale = new Vector3(0, 0, 0);

        var scaleImage = rawImage.transform.localScale * 4;
        currentImageGameObject.transform.LeanScale(new Vector3(scaleImage.x, scaleImage.y, scaleImage.z), .25f).setEaseOutQuart();
    }
}
