using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesBackButtonUI : MonoBehaviour
{
    [HideInInspector] public Action onGamesBackButtonClicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonUIClicked);
    }

    public void OnBackButtonUIClicked()
    {
        SceneManager.LoadScene("Menu");
        // onGamesBackButtonClicked?.Invoke();
    }

}
