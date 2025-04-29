using UnityEngine;


public class HoleTester : MonoBehaviour
{
    public Material transitionMaterial;
    public Transform target; // آبجکت مورد نظر
    public Camera mainCamera;
    public float transitionTime = 2f;
    private float currentRadius = 0.3f;
    private bool closing = true;

    void Update()
    {
        MakeTransition();
    }

    void MakeTransition()
    {
        if (transitionMaterial == null) return;

        // نسبت عرض به ارتفاع صفحه
        float aspect = (float)Screen.width / Screen.height;
        transitionMaterial.SetFloat("_Aspect", aspect);

        // بقیه کدت که Center و Radius رو آپدیت میکنی
        Vector3 screenPos = mainCamera.WorldToViewportPoint(target.position);
        transitionMaterial.SetVector("_Center", new Vector4(screenPos.x, screenPos.y, 0, 0));

        if (transitionMaterial == null || target == null || mainCamera == null) return;

        //// آپدیت مرکز سوراخ
        //Vector3 screenPos = mainCamera.WorldToViewportPoint(target.position);
        //transitionMaterial.SetVector("_Center", new Vector4(screenPos.x, screenPos.y, 0, 0));

        // آپدیت اندازه سوراخ
        float delta = Time.deltaTime / transitionTime;
        if (closing)
            currentRadius -= delta;
        else
            currentRadius += delta;

        currentRadius = Mathf.Clamp01(currentRadius);
        transitionMaterial.SetFloat("_Radius", currentRadius);

        if (Input.GetKeyDown(KeyCode.Space))
            closing = !closing;
    }
}
