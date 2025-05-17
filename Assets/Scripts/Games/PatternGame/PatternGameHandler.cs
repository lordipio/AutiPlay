using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class PatternGameHandler : MonoBehaviour
{
    public static PatternGameHandler instance;
    public Action iconsFirstSpawnAction;
    public List<PatternIcon> patternIcons = new List<PatternIcon>();
    public List<PatternIcon> patternIconHolders = new List<PatternIcon>();
    
    [SerializeField] GameObject matchingIconGO;
    [SerializeField] const int iconsPlaceCount = 4;
    [SerializeField] ParticleSystem confettiTop;
    [SerializeField] ParticleSystem confettiButtom;

    PatternIcon targetIcon;
    Coroutine collideCoroutine;
    Vector3 characterInitialPosition;
    int draggedIconIndex = -1; // -1 is when nothing is dragged
    int succesfulMatch = 0;
    CentralInputHandler inputHandler;
    bool isFirstRun = true;
    float[] iconsAlpha = {1f, 0.6f, 0.3f, 0.1f };

private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioHandler.instance.PlayPatternGameMusic();
        confettiTop = Instantiate<ParticleSystem>(confettiTop);
        confettiButtom = Instantiate<ParticleSystem>(confettiButtom);
        confettiTop.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        confettiButtom.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.LandscapeLeft, SpawnIcons));
    }

    private void SpawnIcons()
    {
        InitializeInputHandler();
        targetIcon = null;
        succesfulMatch = 0;
        List<Vector2> TopIconsPositions = SpawnUtility.GetUpDividedPositions(iconsPlaceCount);
        List<Vector2> ButtomIconsPositions = SpawnUtility.GetDownDividedPositions(iconsPlaceCount);
        GameObject tempPatternIconGO;
        GameObject tempPatternIconHolderGO;
        PatternIcon tempPatternIcon;
        PatternIcon tempPatternIconHolder;
        patternIcons = new List<PatternIcon>();
        patternIconHolders = new List<PatternIcon>();
        Texture2D[] allTextures = SpawnUtility.LoadTextures("Fruits");
        Texture2D texture = allTextures[UnityEngine.Random.Range(0, allTextures.Length - 1)];
        int temp = 0;
        List<int> emptyIcons = SpawnUtility.RandomNumberGenerator(0, 4, 2); // check for range
        Vector2 iconPosition;
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        
        foreach (Vector2 position in TopIconsPositions)
        {
            iconPosition = position;

            if (emptyIcons[0] == temp)
            {
                iconPosition = ButtomIconsPositions[1];
            }

            if (emptyIcons[1] == temp)
            {
                iconPosition = ButtomIconsPositions[2];
            }

            tempPatternIconGO = Instantiate<GameObject>(matchingIconGO, iconPosition, Quaternion.identity);
            tempPatternIcon = tempPatternIconGO.GetComponent<PatternIcon>();
            tempPatternIcon.spriteRenderer.sortingOrder = 1;
            tempPatternIcon.circleCollider.enabled = false;
            tempPatternIcon.circleCollider.layerOverridePriority = 0;

            if (iconPosition != position)
            {
                tempPatternIcon.circleCollider.enabled = true;
                tempPatternIcon.iconIndex = temp;
                tempPatternIconHolderGO = Instantiate<GameObject>(matchingIconGO, position, Quaternion.identity);
                tempPatternIconHolderGO.transform.localScale *= 1.3f;
                tempPatternIconHolderGO.tag = "Finish";
                tempPatternIconGO.tag = "MainCamera";
                tempPatternIconHolder = tempPatternIconHolderGO.GetComponent<PatternIcon>();
                tempPatternIconHolder.iconIndex = temp;
                tempPatternIconHolder.spriteRenderer.sortingOrder = 0;
                tempPatternIconHolder.circleCollider.layerOverridePriority = 1;
                tempPatternIconHolder.circleCollider.enabled = true;
                tempPatternIconHolder.spriteRenderer.sprite = sprite;
                tempPatternIconHolder.spriteRenderer.color = new Color(0f, 0f, 0f, 0.2f);
                patternIconHolders.Add(tempPatternIconHolder);
                tempPatternIconHolder.onIconCollided += OnIconHolderCollided;
            }

            tempPatternIcon.spriteRenderer.sprite = sprite;
            SetImportance(iconsAlpha[temp], tempPatternIcon);
            patternIcons.Add(tempPatternIcon);
            temp++;
        }

        StartCoroutine(IconsFadeIn(2f));

        if (isFirstRun)
        {
            iconsFirstSpawnAction?.Invoke();
            isFirstRun = false;
        }

    }

    void SetImportance(float alpha, PatternIcon icon)
    {
        icon.spriteRenderer.color = new Color(alpha, alpha, alpha, 1f); // آلفا ثابت، رنگ تغییر
    }

    void OnIconHolderCollided(Collider2D otherCollider, int currentIconIndex)
    {
        PatternIcon colliededPatternIcon = otherCollider.gameObject.GetComponent<PatternIcon>();

        if (colliededPatternIcon != null)
        {
            if (currentIconIndex == draggedIconIndex)
            {

                foreach (PatternIcon icon in patternIconHolders)
                    if (icon.iconIndex == draggedIconIndex)
                    {
                        AudioHandler.instance.PlaySelectSound();
                        draggedIconIndex = -1;
                        colliededPatternIcon.circleCollider.enabled = false;
                        otherCollider.gameObject.transform.position = icon.gameObject.transform.position;
                        otherCollider.layerOverridePriority = 1;
                        colliededPatternIcon.spriteRenderer.sortingOrder  = 0;
                        icon.circleCollider.enabled = false;
                        succesfulMatch++;

                        if (succesfulMatch == 2)
                            if (collideCoroutine == null)
                                collideCoroutine = StartCoroutine(RemoveObjects());

                        return;
                    }


            }
        }
    }

    IEnumerator RemoveObjects()
    {
        confettiTop.Play();
        confettiButtom.Play();
        AudioHandler.instance.PlayConfettiSound();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(IconsFadeOut(2f));
        confettiTop.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        confettiButtom.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        SpawnIcons();
        LevelCounter.instance.LevelUp();
    }

    IEnumerator IconsFadeOut(float alphaReductionSpeed)
    {
        while (true) 
        {
            bool temp = false;

            if (patternIcons.Count > 0)
                foreach (PatternIcon patternIcon in patternIcons)
                {
                    if (patternIcon.spriteRenderer.color.a <= 0)
                    {
                        patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, 0);
                        continue;
                    }

                    patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, patternIcon.spriteRenderer.color.a - Time.deltaTime * alphaReductionSpeed);
                    temp = true;
                }

            collideCoroutine = null;

            if (patternIconHolders.Count > 0)
                foreach (PatternIcon patternIcon in patternIconHolders)
                {
                    if (patternIcon.spriteRenderer.color.a <= 0)
                    {
                        patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, 0);
                        continue;
                    }
                        patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, patternIcon.spriteRenderer.color.a - Time.deltaTime * alphaReductionSpeed);
                        temp = true;
                }

            if (!temp)
                break;

            yield return null;
        }

        if (patternIcons.Count > 0)
            foreach (PatternIcon sortingIcon in patternIcons)
            {
                GameObject.Destroy(sortingIcon.gameObject);
            }

        collideCoroutine = null;

        if (patternIconHolders.Count > 0)
            foreach (PatternIcon patternIcon in patternIconHolders)
            {
                GameObject.Destroy(patternIcon.gameObject);
            }
    }


    IEnumerator IconsFadeIn(float alphaReductionSpeed)
    {
        Dictionary<PatternIcon, float> patternIconTargetAlpha = new Dictionary<PatternIcon, float>();
        Dictionary<PatternIcon, float> patternIconHolderTargetAlpha = new Dictionary<PatternIcon, float>();

        if (patternIcons.Count > 0)
            foreach (PatternIcon patternIcon in patternIcons)
            { 
                patternIconTargetAlpha.Add(patternIcon, patternIcon.spriteRenderer.color.a);
                patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, 0);
            }

        if (patternIconHolders.Count > 0)
            foreach (PatternIcon patternIcon in patternIconHolders)
            {
                patternIconHolderTargetAlpha.Add(patternIcon, patternIcon.spriteRenderer.color.a);
                patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, 0);
            }

        while (true)
        {
            bool temp = false;

            if (patternIcons.Count > 0)
                foreach (PatternIcon patternIcon in patternIcons)
                {
                    if (patternIcon.spriteRenderer.color.a >= patternIconTargetAlpha[patternIcon])
                    {
                        patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, patternIconTargetAlpha[patternIcon]);
                        continue;
                    }

                    patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, patternIcon.spriteRenderer.color.a + Time.deltaTime * alphaReductionSpeed);
                    temp = true;
                }


            if (patternIconHolders.Count > 0)
                foreach (PatternIcon patternIcon in patternIconHolders)
                {
                    if (patternIcon.spriteRenderer.color.a >= patternIconHolderTargetAlpha[patternIcon])
                    {
                        patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, patternIconHolderTargetAlpha[patternIcon]);
                        continue;
                    }

                    patternIcon.spriteRenderer.color = new Color(patternIcon.spriteRenderer.color.r, patternIcon.spriteRenderer.color.g, patternIcon.spriteRenderer.color.b, patternIcon.spriteRenderer.color.a + Time.deltaTime * alphaReductionSpeed);
                    temp = true;
                }

            if (!temp)
                break;

            yield return null;
        }
    }

    private void HandleTouchOrClick()
    {
        Vector2 screenPos = Vector2.zero;
        bool isPressed = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                screenPos = touch.position;
                isPressed = true;
            }
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
            PatternIcon icon = hit.collider.GetComponent<PatternIcon>();
            if (icon != null)
            {
                icon.onIconMouseCollided?.Invoke(icon.iconIndex);
            }
        }
    }

    private Vector2 GetInputPosition()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;

        if (Input.GetMouseButton(0))
            return Input.mousePosition;

        return Vector2.zero;
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
            PatternIcon patternIcon = hit.collider.GetComponent<PatternIcon>();
            if (patternIcon != null && patternIcons.Contains(patternIcon))
            {
                AudioHandler.instance.PlaySelectSound();
                draggedIconIndex = patternIcon.iconIndex;
                targetIcon = patternIcon;
            }
        }
    }

    void HandleDrag(Vector2 worldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            PatternIcon patternIcon = hit.collider.GetComponent<PatternIcon>();
            if (patternIcon != null)
            {
                patternIcon.onIconMouseCollided?.Invoke(patternIcon.iconIndex);
            }
        }

        if (draggedIconIndex != -1)
        {
            targetIcon.gameObject.transform.position = worldPos;
        }
    }

    void HandleRelease()
    {
        draggedIconIndex = -1;
        targetIcon = null;
    }


}
