using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchingGamesCategoryMenu : MonoBehaviour
{

    [SerializeField] Button educationButton;

    [SerializeField] Button farmButton;

    [SerializeField] Button homeApplianceButton;

    [SerializeField] Button humanBodyButton;

    [SerializeField] Button natureButton;

    [SerializeField] Button fruitsButton;

    [SerializeField] Button animalsButton;

    [SerializeField] Button backButton;



    private void Awake()
    {
        educationButton.onClick.AddListener(()=> { MatchingGameCategoryData.MatchingGameCategory = "Education"; SceneManager.LoadScene("MatchingGame"); });
        farmButton.onClick.AddListener(()=>{MatchingGameCategoryData.MatchingGameCategory = "Farm"; SceneManager.LoadScene("MatchingGame"); });
        homeApplianceButton.onClick.AddListener(()=>{MatchingGameCategoryData.MatchingGameCategory = "Home Appliances"; SceneManager.LoadScene("MatchingGame");});
        humanBodyButton.onClick.AddListener(()=>{ MatchingGameCategoryData.MatchingGameCategory = "Human Body"; SceneManager.LoadScene("MatchingGame"); });
        natureButton.onClick.AddListener(()=>{MatchingGameCategoryData.MatchingGameCategory = "Nature"; SceneManager.LoadScene("MatchingGame");});
        fruitsButton.onClick.AddListener(()=>{MatchingGameCategoryData.MatchingGameCategory = "Fruits"; SceneManager.LoadScene("MatchingGame");});
        animalsButton.onClick.AddListener(()=>{ MatchingGameCategoryData.MatchingGameCategory = "Animals"; SceneManager.LoadScene("MatchingGame"); });

        backButton.onClick.AddListener(() => { UIHandler.instance.ActivateUIMenu(UIHandler.instance.gamesMenu); });
    }


    //public void OnEducationButtonClicked()
    //{
    //    onMatchingGamesCategorySelected?.Invoke("Education");
    //}

    //public void OnFarmButtonClicked()
    //{
    //    onMatchingGamesCategorySelected?.Invoke("Farm");
    //}

    //public void OnHomeApplianceButtonClicked()
    //{
    //    onMatchingGamesCategorySelected?.Invoke("Home Appliances");

    //}

    //public void OnHumanBodyButtonClicked()
    //{
    //    onMatchingGamesCategorySelected?.Invoke("Human Body");

    //}

    //public void OnNatureButtonClicked()
    //{
    //    onMatchingGamesCategorySelected?.Invoke("Nature");

    //}

    //public void OnFruitsButtonClicked()
    //{
    //    onMatchingGamesCategorySelected?.Invoke("Fruits");

    //}

    //public void OnAnimalButtonClicked()
    //{
    //    onMatchingGamesCategorySelected?.Invoke("Animals");

    //}

    //public void OnMatchingCategoryMenuBackButtonClicked()
    //{
    //    onMatchingCategoryMenuBackButtonClickedEvent?.Invoke();
    //}
}
