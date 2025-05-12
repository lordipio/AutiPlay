using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button GamesButton;
    [SerializeField] private Button OptionButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private List<RectTransform> icons = new List<RectTransform>();
    private Dictionary<RectTransform, Vector2> targetPositions = new Dictionary<RectTransform, Vector2>();

    public float animationDuration = 0.2f;
    public float delayBetween = 0.01f;
    public float bounceAmount = 10f;
    public float bounceDuration = 0.1f;

    private void Awake()
    {


        GamesButton.onClick.AddListener(() => { AudioHandler.instance.PlayButtonSound(); UIHandler.instance.ActivateUIMenu(UIHandler.instance.gamesMenu); });
        OptionButton.onClick.AddListener(() => { AudioHandler.instance.PlayButtonSound(); UIHandler.instance.ActivateUIMenu(UIHandler.instance.gamesMenu); });
        ExitButton.onClick.AddListener(() => { AudioHandler.instance.PlayButtonSound(); Application.Quit(); });
    
    }

    private void OnEnable()
    {

        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransition()
    {
        canvasGroup.alpha = 0f;

        // صبر کن تا Layout کامل بشه
        for (int i = 0; i < 3; i++) yield return new WaitForEndOfFrame();

        icons.Clear();
        targetPositions.Clear();

        icons.Add(GamesButton.GetComponent<RectTransform>());
        icons.Add(OptionButton.GetComponent<RectTransform>());
        icons.Add(ExitButton.GetComponent<RectTransform>());

        foreach (var icon in icons)
        {
            targetPositions[icon] = icon.anchoredPosition;

            // موقعیت شروع پایین صفحه
            float offscreenY = -Screen.height - icon.rect.height;
            icon.anchoredPosition = new Vector2(icon.anchoredPosition.x, offscreenY);

            // اضافه کردن CanvasGroup برای fade-in
            CanvasGroup cg = icon.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = icon.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
        }

        canvasGroup.alpha = 1f;

        foreach (var icon in icons)
        {
            yield return StartCoroutine(MoveUp(icon));
        }
    }

    IEnumerator MoveUp(RectTransform icon)
    {
        Vector2 startPos = icon.anchoredPosition;
        Vector2 targetPos = targetPositions[icon];

        CanvasGroup cg = icon.GetComponent<CanvasGroup>();
        float elapsed = 0f;

        // حرکت اصلی + fade-in
        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            t = 1f - Mathf.Pow(1f - t, 3f); // ease-out

            icon.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            if (cg != null) cg.alpha = t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        icon.anchoredPosition = targetPos;
        if (cg != null) cg.alpha = 1f;

        // Bounce بالا
        Vector2 overShoot = targetPos + new Vector2(0, bounceAmount);
        Vector2 underShoot = targetPos - new Vector2(0, bounceAmount * 0.5f);

        float bounceElapsed = 0f;
        while (bounceElapsed < bounceDuration)
        {
            float t = bounceElapsed / bounceDuration;
            icon.anchoredPosition = Vector2.Lerp(targetPos, overShoot, t);
            bounceElapsed += Time.deltaTime;
            yield return null;
        }

        bounceElapsed = 0f;
        while (bounceElapsed < bounceDuration)
        {
            float t = bounceElapsed / bounceDuration;
            icon.anchoredPosition = Vector2.Lerp(overShoot, targetPos, t);
            bounceElapsed += Time.deltaTime;
            yield return null;
        }

        icon.anchoredPosition = targetPos;
    }
}
