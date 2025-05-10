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
        // ScreenOrientationUtilities.SetPortrait();
        StartCoroutine(SetOrientationAndWait(ScreenOrientation.Portrait, SetBackgroundDimension));
    }

    private void OnEnable()
    {


    }

    void SetBackgroundDimension()
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


    public IEnumerator SetOrientationAndWait(ScreenOrientation orientation, Action onDone = null)
    {
        Screen.orientation = orientation;

        yield return new WaitUntil(() => Screen.orientation == orientation);

        onDone?.Invoke();

        lastAspect = (float)Screen.width / (float)Screen.height;
    }
}