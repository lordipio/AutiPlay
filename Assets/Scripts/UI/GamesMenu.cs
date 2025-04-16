using System;
using UnityEngine;

public class GamesMenu : MonoBehaviour
{
    [HideInInspector] public Action OnPatternsButtonClickedEvent;

    [HideInInspector] public Action OnMatchingButtonClickedEvent;

    [HideInInspector] public Action OnSortingButtonClickedEvent;

    [HideInInspector] public Action OnBackButtonClickedEvent;


    public void OnPatternsButtonClicked()
    {
        OnPatternsButtonClickedEvent?.Invoke();
    }

    public void OnMatchingButtonClicked()
    {
        OnMatchingButtonClickedEvent?.Invoke();
    }

    public void OnSortingButtonClicked()
    {
        OnSortingButtonClickedEvent?.Invoke();
    }

    public void OnBackButtonClicked()
    {
        OnBackButtonClickedEvent?.Invoke();
    }
}
