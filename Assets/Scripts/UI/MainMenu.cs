using System;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    //[HideInInspector] public Action OnGamesButtonClickedEvent;

    //[HideInInspector] public Action OnOptionButtonClickedEvent;

    //[HideInInspector] public Action OnExitButtonClickedEvent;


    [SerializeField] private Button GamesButton;

    [SerializeField] private Button OptionButton;

    [SerializeField] private Button ExitButton;

    private void Awake()
    {
        GamesButton.onClick.AddListener(() => { UIHandler.instance.ActivateUIMenu(UIHandler.instance.gamesMenu); });
        OptionButton.onClick.AddListener(() => { UIHandler.instance.ActivateUIMenu(UIHandler.instance.gamesMenu); });
        ExitButton.onClick.AddListener(() => { Application.Quit(); });

    }

    //public void OnGamesButtonClicked()
    //{
    //    OnGamesButtonClickedEvent?.Invoke();
    //}

    //public void OnOptionButtonClicked()
    //{
    //    OnOptionButtonClickedEvent?.Invoke();
    //}

    //public void OnExitButtonClicked()
    //{
    //    OnExitButtonClickedEvent?.Invoke();
    //}
}
