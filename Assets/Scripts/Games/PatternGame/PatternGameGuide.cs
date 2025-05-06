using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PatternGameGuide : GeneralGuide
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static PatternGameGuide instance;

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


        PatternGameHandler.instance.iconsFirstSpawnAction += InitGuid;

        // StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.LandscapeLeft, InitGuid));
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

        foreach (PatternIcon patternIcon in PatternGameHandler.instance.patternIcons)
            foreach (PatternIcon patternIconHolder in PatternGameHandler.instance.patternIconHolders)
                if (patternIcon.iconIndex == patternIconHolder.iconIndex)
                {
                    yield return StartCoroutine(MoveCursor(patternIcon.gameObject.transform.position, patternIconHolder.gameObject.transform.position, 12f));
                    // yield return new WaitForSeconds(0.5f);
                    continue;
                }
        guideCursor.SetActive(false);
        startCoroutine = null;
    }

}
