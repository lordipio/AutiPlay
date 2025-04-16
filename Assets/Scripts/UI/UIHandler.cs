using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    
    [SerializeField] GamesMenu gamesMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenu = Instantiate(mainMenu);

        gamesMenu = Instantiate(gamesMenu);

        gamesMenu.gameObject.SetActive(false);


        mainMenu.OnGamesButtonClickedEvent += OnGamesButtonClicked;
        mainMenu.OnExitButtonClickedEvent += OnExitButtonClicked;
        mainMenu.OnOptionButtonClickedEvent += OnOptionButtonClicked;

        gamesMenu.OnBackButtonClickedEvent += OnBackButtonClicked;
        gamesMenu.OnMatchingButtonClickedEvent += OnMatchingButtonClicked;
        gamesMenu.OnPatternsButtonClickedEvent += OnPatternsButtonClicked;
        gamesMenu.OnSortingButtonClickedEvent += OnSortingButtonClicked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPatternsButtonClicked()
    {
    }

    void OnMatchingButtonClicked()
    {
    }

    void OnSortingButtonClicked()
    {
    }

    void OnBackButtonClicked()
    {
        mainMenu.gameObject.SetActive(true);

        gamesMenu.gameObject.SetActive(false);
    }

/// 
    void OnGamesButtonClicked()
    {
        mainMenu.gameObject.SetActive(false);
        
        gamesMenu.gameObject.SetActive(true);

    }

    void OnOptionButtonClicked()
    {
    }

    void OnExitButtonClicked()
    {
    }
}
