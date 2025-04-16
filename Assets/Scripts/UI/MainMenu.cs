using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [HideInInspector] public Action OnGamesButtonClickedEvent;

    [HideInInspector] public Action OnOptionButtonClickedEvent;

    [HideInInspector] public Action OnExitButtonClickedEvent;


    public void OnGamesButtonClicked()
    {
        OnGamesButtonClickedEvent?.Invoke();
    }

    public void OnOptionButtonClicked()
    {
        OnOptionButtonClickedEvent?.Invoke();
    }

    public void OnExitButtonClicked()
    {
        OnExitButtonClickedEvent?.Invoke();
    }
}
