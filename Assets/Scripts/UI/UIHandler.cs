using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MatchingGamesCategoryMenu;

public class UIHandler : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject gamesMenu;

    public GameObject matchingGamesCategoryMenu;


    public static UIHandler instance;

    [HideInInspector] public string matchingGameCategoryName = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(gameObject);




        SetupMainMenuPage();
    }


    void SetupMainMenuPage()
    {
        AudioHandler.instance.PlayMenuMusic();

        ScreenOrientationUtilities.SetPortrait();

        ActivateUIMenu(mainMenu);
    }


    public void DeactiveAll()
    {
        mainMenu.gameObject.SetActive(false);

        gamesMenu.gameObject.SetActive(false);

        matchingGamesCategoryMenu.gameObject.SetActive(false);
    }


    public void ActivateUIMenu(GameObject menuPage)
    {

        DeactiveAll();

        menuPage.SetActive(true);
    }

}
