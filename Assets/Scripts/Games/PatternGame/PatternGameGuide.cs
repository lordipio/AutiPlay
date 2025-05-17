using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PatternGameGuide : GeneralGuide
{
    public static PatternGameGuide instance;

    Coroutine startCoroutine;
    bool firstRunGuide = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        StartCoroutine(FadeInGuidePassage(5f));
        guideCursor.SetActive(true);

        foreach (PatternIcon patternIcon in PatternGameHandler.instance.patternIcons)
            foreach (PatternIcon patternIconHolder in PatternGameHandler.instance.patternIconHolders)
                if (patternIcon.iconIndex == patternIconHolder.iconIndex)
                {
                    yield return StartCoroutine(MoveCursor(patternIcon.gameObject.transform.position, patternIconHolder.gameObject.transform.position, 12f));
                    // yield return new WaitForSeconds(0.5f);
                    continue;
                }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOutGuidePassage(5f));
        guideCursor.SetActive(false);
        startCoroutine = null;
    }

}
