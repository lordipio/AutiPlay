using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MatchingGameGuide : GeneralGuide
{
    public static MatchingGameGuide instance;

    Coroutine startCoroutine;

    bool firstRunGuide = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        else
            Destroy(instance);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();


        MatchingGameHandler.instance.iconsFirstSpawnAction += InitGuid;

        // StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.LandscapeLeft, InitGuid));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitGuid()
    {
        if (startCoroutine == null)
            startCoroutine = StartCoroutine(StartGuid());
    }

    IEnumerator StartGuid()
    {
        if (firstRunGuide)
        {
            yield return new WaitForSeconds(1f);
            firstRunGuide = false;
        }

        guideCursor.SetActive(true);
        foreach (MatchingIcon topMatchingIcon in MatchingGameHandler.instance.topMatchingIcon)
            foreach (MatchingIcon buttomMatchingIcon in MatchingGameHandler.instance.buttomMatchingIcon)
                if (topMatchingIcon.iconIndex == buttomMatchingIcon.iconIndex)
                { 
                    yield return StartCoroutine(MoveCursor(topMatchingIcon.gameObject.transform.position, buttomMatchingIcon.gameObject.transform.position, 12f));
                    // yield return new WaitForSeconds(0.5f);
                    continue;
                }
        guideCursor.SetActive(false);
        startCoroutine = null;
    }


}
