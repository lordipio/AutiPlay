using System;
using System.Collections;
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
        AudioHandler.instance.PlayButtonSound();

        StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.Portrait, ()=> { SceneManager.LoadScene("Menu"); } ));

        // onGamesBackButtonClicked?.Invoke();
    }

    public void OnGuideButtonUIClicked()
    {
        StartCoroutine(StartNewLevel());
        // SceneManager.LoadScene("Menu");
        // onGamesBackButtonClicked?.Invoke();
    }

    IEnumerator StartNewLevel()
    {
        AudioHandler.instance.PlayButtonSound();
        while(true)
        {
            if (!AudioHandler.instance.generalAudioSource.isPlaying)
                break;
            yield return null;
        }
        if (MatchingGameGuide.instance)
            MatchingGameGuide.instance.InitGuid();

        if (PatternGameGuide.instance)
            PatternGameGuide.instance.InitGuid();

        if (SortingGameGuide.instance)
            SortingGameGuide.instance.InitGuid();
    }

}
