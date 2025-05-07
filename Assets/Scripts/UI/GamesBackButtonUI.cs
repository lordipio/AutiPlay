using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesBackButtonUI : MonoBehaviour
{
    [HideInInspector] public Action onGamesBackButtonClicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Button backButton;

    [SerializeField] private Button guideButton;


    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonUIClicked);
        guideButton.onClick.AddListener(OnGuideButtonUIClicked);
    }

    public void OnBackButtonUIClicked()
    {
        StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.Portrait, ()=> { SceneManager.LoadScene("Menu"); } ));

        // onGamesBackButtonClicked?.Invoke();
    }

    public void OnGuideButtonUIClicked()
    {
        if (MatchingGameGuide.instance)
            MatchingGameGuide.instance.InitGuid();

        if (PatternGameGuide.instance)
            PatternGameGuide.instance.InitGuid();

        if (SortingGameGuide.instance)
            SortingGameGuide.instance.InitGuid();
        // SceneManager.LoadScene("Menu");
        // onGamesBackButtonClicked?.Invoke();
    }

}
