using UnityEngine;

public class AdjustCamera : MonoBehaviour
{

    private float lastAspect;

    void Start()
    {
        lastAspect = (float)Screen.width / (float)Screen.height;
        HandleCameraAdjustment();
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
        // نسبت پایه‌ای: افقی
        float targetAspect = 16f / 9f; // یا هر نسبتی که براش طراحی کردی
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleWidth = windowAspect / targetAspect;

        Camera cam = Camera.main;

        if (scaleWidth < 1.0f)
        {
            cam.orthographicSize = 10f / scaleWidth; // 5f مقدار پایه
        }
        else
        {
            cam.orthographicSize = 10f; // مقدار پایه
        }
    }



}
