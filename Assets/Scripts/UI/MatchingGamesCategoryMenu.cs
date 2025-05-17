using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchingGamesCategoryMenu : MonoBehaviour
{
    public float animationDuration = 0.05f;
    public float delayBetween = 0.01f;
    public float bounceAmount = 10f;
    public float bounceDuration = 0.1f;

    [SerializeField] Button educationButton;
    [SerializeField] Button farmButton;
    [SerializeField] Button homeApplianceButton;
    [SerializeField] Button humanBodyButton;
    [SerializeField] Button natureButton;
    [SerializeField] Button fruitsButton;
    [SerializeField] Button animalsButton;
    [SerializeField] Button backButton;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] ScrollRect scrollRect;

    private List<RectTransform> buttons = new List<RectTransform>();
    private Dictionary<RectTransform, Vector2> targetPositions = new Dictionary<RectTransform, Vector2>();

    private void Awake()
    {
        educationButton.onClick.AddListener(() => { StartCoroutine(LoadCategoryHandler("Education"));});
        farmButton.onClick.AddListener(() =>{ StartCoroutine(LoadCategoryHandler("Farm"));});
        homeApplianceButton.onClick.AddListener(() =>{ StartCoroutine(LoadCategoryHandler("Home Appliances"));});
        humanBodyButton.onClick.AddListener(() =>{ StartCoroutine(LoadCategoryHandler("Human Body"));});
        natureButton.onClick.AddListener(() =>{ StartCoroutine(LoadCategoryHandler("Nature"));});
        fruitsButton.onClick.AddListener(() =>{ StartCoroutine(LoadCategoryHandler("Fruits"));});
        animalsButton.onClick.AddListener(() =>{ StartCoroutine(LoadCategoryHandler("Animals"));});
        backButton.onClick.AddListener(() =>{ AudioHandler.instance.PlayButtonSound(); UIHandler.instance.ActivateUIMenu(UIHandler.instance.gamesMenu);});
    }

    IEnumerator LoadCategoryHandler(string category)
    {
        AudioHandler.instance.PlayButtonSound();
        while (true) 
        {
            if (!AudioHandler.instance.generalAudioSource.isPlaying)
                break;

            yield return null;
        }

        LoadCategory(category);
    }

    private void OnEnable()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        StartCoroutine(StartTransition());
    }

    void LoadCategory(string category)
    {
        MatchingGameCategoryData.MatchingGameCategory = category;
        SceneManager.LoadScene("MatchingGame");
    }

    IEnumerator StartTransition()
    {
        canvasGroup.alpha = 0f;

        for (int i = 0; i < 3; i++) yield return new WaitForEndOfFrame();

        buttons.Clear();
        targetPositions.Clear();
        Button[] buttonArray = { backButton, animalsButton, educationButton, farmButton, homeApplianceButton, humanBodyButton, natureButton, fruitsButton };

        foreach (var btn in buttonArray)
        {
            var rect = btn.GetComponent<RectTransform>();
            buttons.Add(rect);
            targetPositions[rect] = rect.anchoredPosition;
            float offscreenY = -Screen.height - rect.rect.height;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, offscreenY);

            var cg = btn.GetComponent<CanvasGroup>();

            if (cg == null) cg = btn.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 0f;
        }

        canvasGroup.alpha = 1f;

        foreach (var rect in buttons)
        {
            yield return StartCoroutine(MoveUp(rect));
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
            t = 1f - Mathf.Pow(1f - t, 3f); // ease-out
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
