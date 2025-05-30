﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchingGameHandler : MonoBehaviour
{
    public List<MatchingIcon> topMatchingIcon = new List<MatchingIcon>();
    public List<MatchingIcon> buttomMatchingIcon = new List<MatchingIcon>();
    public Action iconsFirstSpawnAction;
    public static MatchingGameHandler instance;

    [SerializeField] GameObject matchingIconGO;
    [SerializeField] ParticleSystem confettiTop;
    [SerializeField] ParticleSystem confettiButtom;
    [SerializeField] const int iconsCount = 4;

    List<LineRenderer> lines = new List<LineRenderer>();
    LineRenderer line;
    CentralInputHandler inputHandler;
    int selectedIconIndex = -1;
    int matchedIconsNumber = 0;
    bool isFirstRun = true;
    string matchingGameIconCategory = "";

    enum SelectedIconSide
    {
        top,
        bottom,
        none
    }
    SelectedIconSide selectedIconSide = SelectedIconSide.none;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(instance);
    }


    void Start()
    {
        AudioHandler.instance.PlayMatchingGameMusic();
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.5f;
        line.endWidth = 0.2f;
        line.enabled = false;
        matchingGameIconCategory = MatchingGameCategoryData.MatchingGameCategory;
        confettiTop = Instantiate<ParticleSystem>(confettiTop);
        confettiButtom = Instantiate<ParticleSystem>(confettiButtom);
        confettiTop.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        confettiButtom.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.LandscapeLeft, InitializeInputHandler));
        StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.LandscapeLeft, SpawnIcons));
    }

    void SpawnIcons()
    {
        InitializeInputHandler();
        matchedIconsNumber = 0;
        selectedIconIndex = -1;
        topMatchingIcon = new List<MatchingIcon>();
        buttomMatchingIcon = new List<MatchingIcon>();
        lines = new List<LineRenderer>();
        List<Vector2> Positions = SpawnUtility.GetUpDividedPositions(4);
        GameObject tempMatchingIconGO;
        MatchingIcon tempMatchingIcon;
        List<MatchingIcon> matchingIcons = new List<MatchingIcon>();
        Texture2D[] allTextures = SpawnUtility.LoadTextures(matchingGameIconCategory);
        List<int> texturesRandomSelection = SpawnUtility.RandomNumberGenerator(0, allTextures.Length - 1, iconsCount + 1);
        List<Texture2D> textures = new List<Texture2D>();

        foreach (int i in texturesRandomSelection)
        {
            textures.Add(allTextures[i]);
        }

        int temp = 0;

        foreach (Vector2 position in Positions)
        {
            tempMatchingIconGO = Instantiate<GameObject>(matchingIconGO, position, Quaternion.identity);
            tempMatchingIcon = tempMatchingIconGO.GetComponent<MatchingIcon>();
            Texture2D tex = textures[temp];
            tempMatchingIcon.spriteRenderer.sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f));
            tempMatchingIcon.SetKnobToTheTop();
            tempMatchingIcon.iconIndex = temp;
            topMatchingIcon.Add(tempMatchingIcon);
            tempMatchingIcon.onIconMouseCollided += OnTopIconCollided;
            temp++;
        }

        Positions = SpawnUtility.GetDownDividedPositions(4);
        temp = 0;
        texturesRandomSelection = SpawnUtility.RandomNumberGenerator(0, iconsCount, iconsCount + 1);

        foreach (Vector2 position in Positions)
        {
            tempMatchingIconGO = Instantiate<GameObject>(matchingIconGO, position, Quaternion.identity);
            tempMatchingIcon = tempMatchingIconGO.GetComponent<MatchingIcon>();
            tempMatchingIcon.SetKnobToTheButtom();
            tempMatchingIcon.spriteRenderer.sprite = topMatchingIcon[texturesRandomSelection[temp]].spriteRenderer.sprite;
            tempMatchingIcon.iconIndex = topMatchingIcon[texturesRandomSelection[temp]].iconIndex;
            buttomMatchingIcon.Add(tempMatchingIcon);
            tempMatchingIcon.onIconMouseCollided += OnButtomIconCollided;
            temp++;
        }

        StartCoroutine(IconsFadeIn(2f));

        if (isFirstRun)
        {
            iconsFirstSpawnAction?.Invoke();
            isFirstRun = false;
        }
    }


    void OnTopIconCollided(int iconIndex)
    {
        if (iconIndex == selectedIconIndex && selectedIconSide == SelectedIconSide.bottom)
        {
            MatchingIcon collidedMatchingIcon = null;

            foreach (MatchingIcon matchingIcon in buttomMatchingIcon)
                if (matchingIcon.iconIndex == iconIndex)
                    collidedMatchingIcon = matchingIcon;

            CreateNewLine(topMatchingIcon[iconIndex].holderKnob.transform.position, collidedMatchingIcon.holderKnob.transform.position);
            DeactiveIcon(topMatchingIcon[selectedIconIndex]);
            DeactiveIcon(collidedMatchingIcon);
            matchedIconsNumber++;
            AudioHandler.instance.PlaySelectSound();

            if (matchedIconsNumber >= iconsCount)
                StartCoroutine(RemoveObjects());

            selectedIconIndex = -1;
            line.enabled = false;
            return;
        }

        if (selectedIconIndex != iconIndex)
            AudioHandler.instance.PlaySelectSound();

        selectedIconIndex = iconIndex;
        selectedIconSide = SelectedIconSide.top;
        line.enabled = true;
        line.SetPosition(0, topMatchingIcon[iconIndex].holderKnob.transform.position);
    }

    void OnButtomIconCollided(int iconIndex)
    {
        MatchingIcon collidedMatchingIcon = null;

        foreach (MatchingIcon matchingIcon in buttomMatchingIcon)
            if (matchingIcon.iconIndex == iconIndex)
                collidedMatchingIcon = matchingIcon;

        if (iconIndex == selectedIconIndex && selectedIconSide == SelectedIconSide.top)
        {
            CreateNewLine(topMatchingIcon[selectedIconIndex].holderKnob.transform.position, collidedMatchingIcon.holderKnob.transform.position);
            DeactiveIcon(topMatchingIcon[selectedIconIndex]);
            DeactiveIcon(collidedMatchingIcon);
            matchedIconsNumber++;
            AudioHandler.instance.PlaySelectSound();
            if (matchedIconsNumber >= iconsCount)
                StartCoroutine(RemoveObjects());
            selectedIconIndex = -1;
            line.enabled = false;
            return;
        }

        if (selectedIconIndex != iconIndex)
            AudioHandler.instance.PlaySelectSound();

        selectedIconIndex = iconIndex;
        selectedIconSide = SelectedIconSide.bottom;
        line.enabled = true;
        line.SetPosition(0, collidedMatchingIcon.holderKnob.transform.position);
    }


    void CreateNewLine(Vector2 startPos, Vector2 endPos)
    {
        GameObject lineObj = new GameObject("Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        Color greenColor = Color.green;
        greenColor.a = 0.8f;
        lr.material.color = greenColor;
        lr.positionCount = 2;
        lr.startWidth = 0.5f;
        lr.endWidth = 0.5f;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        lr.enabled = true;
        lines.Add(lr);
    }


    void DeactiveIcon(MatchingIcon icon)
    {
        icon.circleCollider.enabled = false;
        icon.spriteRenderer.color = new Color(icon.spriteRenderer.color.r, icon.spriteRenderer.color.g, icon.spriteRenderer.color.b, 0.7f);
    }

    IEnumerator RemoveObjects()
    {
        confettiTop.Play();
        confettiButtom.Play();

        while (true)
        {
            if (!AudioHandler.instance.generalAudioSource.isPlaying)
            {
                AudioHandler.instance.PlayConfettiSound();
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(IconsFadeOut(1f));
    }


    IEnumerator IconsFadeOut(float alphaReductionSpeed)
    {
        while (true)
        {
            bool temp = false;

            if (topMatchingIcon.Count > 0)
                foreach (MatchingIcon matchingIcon in topMatchingIcon)
                {
                    if (matchingIcon.spriteRenderer.color.a <= 0)
                    {
                        matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, 0);
                        continue;
                    }
                    matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, matchingIcon.spriteRenderer.color.a - Time.deltaTime * alphaReductionSpeed);
                    temp = true;
                }

            if (buttomMatchingIcon.Count > 0)
                foreach (MatchingIcon matchingIcon in buttomMatchingIcon)
                {
                    if (matchingIcon.spriteRenderer.color.a <= 0)
                    {
                        matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, 0);
                        continue;
                    }
                    matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, matchingIcon.spriteRenderer.color.a - Time.deltaTime * alphaReductionSpeed);
                    temp = true;
                }

            if (lines.Count > 0)
                foreach (LineRenderer lineRenderer in lines)
                {
                    if (lineRenderer.startColor.a <= 0)
                    {
                        lineRenderer.startColor  = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, 0f);
                        lineRenderer.endColor = lineRenderer.startColor;
                        continue;
                    }
                    lineRenderer.startColor  = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, lineRenderer.startColor.a - Time.deltaTime * alphaReductionSpeed);
                    lineRenderer.endColor = lineRenderer.startColor;
                    temp = true;
                }

            MatchingGameGuide.instance.enabled = false;

            if (!temp)
                break;

            yield return null;
        }

        if (topMatchingIcon.Count > 0)
            foreach (MatchingIcon matchingIcon in topMatchingIcon)
            {
                GameObject.Destroy(matchingIcon.gameObject);
            }

        if (buttomMatchingIcon.Count > 0)
            foreach (MatchingIcon matchingIcon in buttomMatchingIcon)
            {
                GameObject.Destroy(matchingIcon.gameObject);
            }

        if (lines.Count > 0)
            foreach (LineRenderer lineRenderer in lines)
            {
                GameObject.Destroy(lineRenderer.gameObject);
            }

        topMatchingIcon = null;
        buttomMatchingIcon = null;
        lines = null;
        confettiTop.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        confettiButtom.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        LevelCounter.instance.LevelUp();
        SpawnIcons();
    }

    IEnumerator IconsFadeIn(float alphaReductionSpeed)
    {
        if (topMatchingIcon.Count > 0)
            foreach (MatchingIcon matchingIcon in topMatchingIcon)
                matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, 0f);
        
        if (buttomMatchingIcon.Count > 0)
            foreach (MatchingIcon matchingIcon in buttomMatchingIcon)
                matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, 0f);
        
        if (topMatchingIcon.Count > 0)
            foreach (MatchingIcon matchingIcon in topMatchingIcon)
                matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, 0f);
        
        while (true)
        {
            bool temp = false;

            if (topMatchingIcon.Count > 0)
                foreach (MatchingIcon matchingIcon in topMatchingIcon)
                {
                    if (matchingIcon.spriteRenderer.color.a >= 1f)
                    {
                        matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, 1);
                        continue;
                    }
                    matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, matchingIcon.spriteRenderer.color.a + Time.deltaTime * alphaReductionSpeed);
                    temp = true;
                }

            if (buttomMatchingIcon.Count > 0)
                foreach (MatchingIcon matchingIcon in buttomMatchingIcon)
                {
                    if (matchingIcon.spriteRenderer.color.a >= 1f)
                    {
                        matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, 1f);
                        continue;
                    }
                    
                    matchingIcon.spriteRenderer.color = new Color(matchingIcon.spriteRenderer.color.r, matchingIcon.spriteRenderer.color.g, matchingIcon.spriteRenderer.color.b, matchingIcon.spriteRenderer.color.a + Time.deltaTime * alphaReductionSpeed);
                    temp = true;
                }

            if (!temp)
                break;

            yield return null;
        }
    }

    void HandleTouchOrMouseInput()
    {
        Vector2 screenPos = Vector2.zero;
        bool isPressed = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            screenPos = touch.position;
        }

        else if (Input.GetMouseButtonDown(0))
        {
            screenPos = Input.mousePosition;
            isPressed = true;
        }

        if (!isPressed)
            return;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            MatchingIcon icon = hit.collider.GetComponent<MatchingIcon>();

            if (icon != null)
                icon.onIconMouseCollided?.Invoke(icon.iconIndex);
        }
    }





    private void InitializeInputHandler()
    {
        CentralInputHandler.Instance.OnPress += HandlePress;
        CentralInputHandler.Instance.OnRelease += HandleRelease;
        CentralInputHandler.Instance.OnDrag += HandleDrag;
        
    }
    private void OnDisable()
    {
        if (CentralInputHandler.Instance == null) return;
            CentralInputHandler.Instance.OnPress -= HandlePress;

        CentralInputHandler.Instance.OnRelease -= HandleRelease;
        CentralInputHandler.Instance.OnDrag -= HandleDrag;
    }

    void HandlePress(Vector2 worldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            MatchingIcon matchingIcon = hit.collider.GetComponent<MatchingIcon>();

            if (matchingIcon != null)
            {
                matchingIcon.onIconMouseCollided?.Invoke(matchingIcon.iconIndex);

                if (topMatchingIcon.Contains(matchingIcon))
                    selectedIconSide = SelectedIconSide.top;

                else
                    selectedIconSide = SelectedIconSide.bottom;

                line.enabled = true;
                line.SetPosition(0, matchingIcon.holderKnob.transform.position);
            }
        }
    }

    void HandleDrag(Vector2 worldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            MatchingIcon matchingIcon = hit.collider.GetComponent<MatchingIcon>();
            if (matchingIcon != null)
            {
                matchingIcon.onIconMouseCollided?.Invoke(matchingIcon.iconIndex);

                if (topMatchingIcon.Contains(matchingIcon))
                    selectedIconSide = SelectedIconSide.top;

                else
                    selectedIconSide = SelectedIconSide.bottom;

                line.enabled = true;
            }
        }

        if (selectedIconIndex != -1)
        {
            line.SetPosition(1, worldPos);
        }

        else
            line.enabled = false;
    }

    void HandleRelease()
    {
        line.enabled = false;
        selectedIconIndex = -1;
        selectedIconSide = SelectedIconSide.none;
    }



}

