﻿using System.Collections;
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

        if (MatchingGameHandler.instance.topMatchingIcon.Count > 0 && MatchingGameHandler.instance.buttomMatchingIcon.Count > 0)
        foreach (MatchingIcon topMatchingIcon in MatchingGameHandler.instance.topMatchingIcon)
            foreach (MatchingIcon buttomMatchingIcon in MatchingGameHandler.instance.buttomMatchingIcon)
                if (topMatchingIcon.iconIndex == buttomMatchingIcon.iconIndex)
                { 
                    if (topMatchingIcon && buttomMatchingIcon)
                        yield return StartCoroutine(MoveCursor(topMatchingIcon.gameObject.transform.position, buttomMatchingIcon.gameObject.transform.position, 12f));
                    
                    continue;
                }

        StartCoroutine(FadeOutGuidePassage(5f));
        guideCursor.SetActive(false);
        startCoroutine = null;
    }


}
