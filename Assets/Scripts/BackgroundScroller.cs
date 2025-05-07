using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    public RawImage background;
    public Vector2 backgroundDimension;

    public float scrollSpeed = 0.1f;

    private void OnEnable()
    {
        // float width = Screen.width;


        float height = Screen.height;

        float heightScale = height / backgroundDimension.y;

        // float widthScale = width / backgroundDimension.x;

        background.rectTransform.sizeDelta = new Vector2(backgroundDimension.x * heightScale, height);

        // background.rectTransform.sizeDelta = new Vector2(backgroundDimension.x * heightScale, background.rectTransform.sizeDelta.x);

    }

    void Update()
    {
        Rect uvRect = background.uvRect;
        uvRect.x += scrollSpeed * Time.deltaTime;
        background.uvRect = uvRect;
    }
}
