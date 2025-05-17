using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SortingGameGuide : GeneralGuide
{
    public static SortingGameGuide instance;
    public GameObject guideUIImage;

    Coroutine startCoroutine;
    bool firstRunGuide = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(instance);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        SortingGameHandler.instance.iconsFirstSpawnAction += InitGuid;
    }

    public void InitGuid()
    {
        if (startCoroutine == null)
            startCoroutine = StartCoroutine(StartGuid());
    }

    IEnumerator StartGuid()
    {
        if (firstRunGuide)
        {
            yield return new WaitForSeconds(1f);
            firstRunGuide = false;
        }

        StartCoroutine(FadeInGuidePassage(5f));
        guideUIImage.gameObject.SetActive(true);
        StartCoroutine(DrawAndFadeLine(SortingGameCharacter.instance.transform.position, SortingGameHandler.instance.targetIcon.gameObject.transform.position, 0.5f, 1f, 1.5f));
        yield return StartCoroutine(PulseScale(guideUIImage.gameObject.transform, 3f, 1.5f, 0.8f, 1f));
        StartCoroutine(FadeOutGuidePassage(5f));
        guideUIImage.gameObject.SetActive(false);
        guideCursor.SetActive(false);
        startCoroutine = null;
    }

    IEnumerator PulseScale(Transform target, float duration, float frequency, float minScale, float maxScale)
    {
        float elapsed = 0f;
        Vector3 originalScale = target.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = (Mathf.Sin(elapsed * frequency * Mathf.PI * 2) + 1f) / 2f;
            float scale = Mathf.Lerp(minScale, maxScale, t);
            target.localScale = originalScale * scale;
            yield return null;
        }

        target.localScale = originalScale;
    }

    IEnumerator DrawAndFadeLine(Vector3 pointA, Vector3 pointB, float fadeInDuration, float holdDuration, float fadeOutDuration)
    {
        GameObject lineObj = new GameObject("PathLine");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        Color baseColor = new Color(0.8f, 0.4f, 0.4f, 0f); // شروع با alpha صفر
        lr.positionCount = 2;
        lr.SetPosition(0, pointA);
        lr.SetPosition(1, pointB);
        lr.material = new Material(Shader.Find("Sprites/Default")); // پشتیبانی از شفافیت
        lr.startWidth = 0.4f;
        lr.endWidth = 0.1f;
        lr.startColor = baseColor;
        lr.endColor = baseColor;
        lr.sortingOrder = -1;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            Color c = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lr.startColor = c;
            lr.endColor = c;
            yield return null;
        }

        yield return new WaitForSeconds(holdDuration);
        elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            Color c = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lr.startColor = c;
            lr.endColor = c;
            yield return null;
        }

        Destroy(lineObj);
    }



}
