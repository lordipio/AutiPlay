using System;
using System.Collections;
using UnityEngine;

public class AdjustCamera : MonoBehaviour
{
    static public AdjustCamera instance;
    public bool isPortrait = false;

    private float lastAspect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
            Destroy(instance);
    }

    void Start()
    {
        StartCoroutine(SetOrientationAndWait(ScreenOrientation.LandscapeLeft));
    }

    void Update()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;

        if (Mathf.Abs(currentAspect - lastAspect) > 0.01f)
        {
            lastAspect = currentAspect;
            HandleCameraAdjustment();
        }
    }

    void HandleCameraAdjustment()
    {
        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleWidth = windowAspect / targetAspect;
        Camera cam = Camera.main;

        if (scaleWidth < 1.0f)
        {
            cam.orthographicSize = 10f / scaleWidth; 
        }

        else
        {
            cam.orthographicSize = 10f;
        }
    }

    public void UpdateAspect()
    {
        lastAspect = (float)Screen.width / (float)Screen.height;
        HandleCameraAdjustment();
    }


    public IEnumerator SetOrientationAndWait(ScreenOrientation orientation, Action onDone = null)
    {
        Screen.orientation = orientation;
        yield return new WaitUntil(() => Screen.orientation == orientation);
        onDone?.Invoke();
        lastAspect = (float)Screen.width / (float)Screen.height;
        HandleCameraAdjustment();
    }




}
