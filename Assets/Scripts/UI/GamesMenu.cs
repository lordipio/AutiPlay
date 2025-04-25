using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GamesMenu : MonoBehaviour
{
    //[HideInInspector] public Action OnPatternsButtonClickedEvent;

    //[HideInInspector] public Action OnMatchingButtonClickedEvent;

    //[HideInInspector] public Action OnSortingButtonClickedEvent;

    //[HideInInspector] public Action OnBackButtonClickedEvent;


    [SerializeField] public Button PatternsButton;

    [SerializeField] public Button MatchingButton;

    [SerializeField] public Button SortingButton;

    [SerializeField] public Button BackButton;

    private void Awake()
    {
        PatternsButton.onClick.AddListener(OnPatternsButtonClicked);
        MatchingButton.onClick.AddListener(OnMatchingButtonClicked);
        SortingButton.onClick.AddListener(OnSortingButtonClicked);
        BackButton.onClick.AddListener(OnBackButtonClicked);

    }


    public void OnPatternsButtonClicked()
    {

    }

    public void OnMatchingButtonClicked()
    {
        UIHandler.instance.ActivateUIMenu(UIHandler.instance.matchingGamesCategoryMenu);
    }

    public void OnSortingButtonClicked()
    {
        SceneManager.LoadScene("SortingGame");
    }

    public void OnBackButtonClicked()
    {
        UIHandler.instance.ActivateUIMenu(UIHandler.instance.mainMenu);
    }
}
