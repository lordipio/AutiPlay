using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BackgroundScroller : MonoBehaviour
{
    public RawImage background;
    public Vector2 backgroundDimension;
    public float scrollSpeed = 0.1f;

    private float lastAspect;

    private void Awake()
    {
        StartCoroutine(SetOrientationAndWait(ScreenOrientation.Portrait, SetBackgroundDimension));
    }

    void SetBackgroundDimension()
    {
        float height = Screen.height;
        float heightScale = height / backgroundDimension.y;
        background.rectTransform.sizeDelta = new Vector2(backgroundDimension.x * heightScale, height);
    }

    void Update()
    {
        Rect uvRect = background.uvRect;
        uvRect.x += scrollSpeed * Time.deltaTime;
        background.uvRect = uvRect;
    }


    public IEnumerator SetOrientationAndWait(ScreenOrientation orientation, Action onDone = null)
    {
        Screen.orientation = orientation;
        yield return new WaitUntil(() => Screen.orientation == orientation);
        onDone?.Invoke();
        lastAspect = (float)Screen.width / (float)Screen.height;
    }
}