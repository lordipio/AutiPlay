using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesMenu : MonoBehaviour
{
    public float animationDuration = 0.2f;
    public float delayBetween = 0.05f;
    public float bounceAmount = -50f;
    public float bounceDuration = 0.1f;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] public Button PatternsButton;
    [SerializeField] public Button MatchingButton;
    [SerializeField] public Button SortingButton;
    [SerializeField] public Button BackButton;

    List<RectTransform> icons = new List<RectTransform>();
    Dictionary<RectTransform, Vector2> targetPositions = new Dictionary<RectTransform, Vector2>();

    private void Awake()
    {
        MatchingButton.onClick.AddListener(OnMatchingButtonClicked);
        SortingButton.onClick.AddListener(OnSortingButtonClicked);
        PatternsButton.onClick.AddListener(OnPatternsButtonClicked);
        BackButton.onClick.AddListener(OnBackButtonClicked);
    }

    public void OnPatternsButtonClicked()
    {
        StartCoroutine(StartNewLevel("PatternGame"));
    }

    public void OnMatchingButtonClicked()
    {
        AudioHandler.instance.PlayButtonSound();
        UIHandler.instance.ActivateUIMenu(UIHandler.instance.matchingGamesCategoryMenu);
    }

    public void OnSortingButtonClicked()
    {
        StartCoroutine(StartNewLevel("SortingGame"));
    }

    public void OnBackButtonClicked()
    {
        AudioHandler.instance.PlayButtonSound();
        UIHandler.instance.ActivateUIMenu(UIHandler.instance.mainMenu);
    }

    IEnumerator StartNewLevel(string scene)
    {
        AudioHandler.instance.PlayButtonSound();

        while (true)
        {
            if (!AudioHandler.instance.generalAudioSource.isPlaying)
                break;

            yield return null;
        }

        SceneManager.LoadScene(scene);
    }

    private void OnEnable()
    {
        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransition()
    {
        canvasGroup.alpha = 0f;
        
        for (int i = 0; i < 3; i++) yield return new WaitForEndOfFrame();

        icons.Clear();
        targetPositions.Clear();
        icons.Add(MatchingButton.GetComponent<RectTransform>());
        icons.Add(SortingButton.GetComponent<RectTransform>());
        icons.Add(PatternsButton.GetComponent<RectTransform>());
        icons.Add(BackButton.GetComponent<RectTransform>());

        foreach (var icon in icons)
        {
            targetPositions[icon] = icon.anchoredPosition;
            float offscreenY = -Screen.height - icon.rect.height;
            icon.anchoredPosition = new Vector2(icon.anchoredPosition.x, offscreenY);
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

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            t = 1f - Mathf.Pow(1f - t, 3f);
            icon.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            if (cg != null) cg.alpha = t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        icon.anchoredPosition = targetPos;

        if (cg != null) cg.alpha = 1f;

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
