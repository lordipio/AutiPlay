using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PatternGameHandler : MonoBehaviour
{
    [SerializeField] GameObject matchingIconGO;

    [SerializeField] const int iconsPlaceCount = 4;

    List<PatternIcon> patternIcons = new List<PatternIcon>();

    List<PatternIcon> patternIconHolders = new List<PatternIcon>();

    PatternIcon targetIcon;

    Coroutine collideCoroutine;

    Vector3 characterInitialPosition;

    int draggedIconIndex = -1; // -1 is when nothing is dragged

    int succesfulMatch = 0;

    [SerializeField] ParticleSystem confettiTop;
    
    [SerializeField] ParticleSystem confettiButtom;

    CentralInputHandler inputHandler;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        confettiTop = Instantiate<ParticleSystem>(confettiTop);
        confettiButtom = Instantiate<ParticleSystem>(confettiButtom);

        confettiTop.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        confettiButtom.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.LandscapeLeft, SpawnIcons));
    }


    private void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    draggedIconIndex = -1;
        //}

        //if (draggedIconIndex != -1)
        //{
        //    Vector2 inputPos = GetInputPosition();
        //    if (inputPos != Vector2.zero)
        //        patternIcons[draggedIconIndex].transform.position = Camera.main.ScreenToWorldPoint(inputPos);
        //}

        //HandleTouchOrClick();
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
        
        Texture2D texture = allTextures[Random.Range(0, allTextures.Length - 1)];

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

                tempPatternIconHolder.circleCollider.enabled = true;

                tempPatternIconHolder.spriteRenderer.sprite = sprite;

                tempPatternIconHolder.spriteRenderer.color = new Color(0f, 0f, 0f, 0.2f);

                patternIconHolders.Add(tempPatternIconHolder);

                tempPatternIcon.onIconMouseCollided += OnIconMouseCollided;

                tempPatternIconHolder.onIconCollided += OnIconHolderCollided;
            }


            tempPatternIcon.spriteRenderer.sprite = sprite;

            tempPatternIcon.spriteRenderer.color = new Color(1f, 1f, 1f, 1f - temp * 0.2f);

            patternIcons.Add(tempPatternIcon);

            temp++;
        }

        StartCoroutine(IconsFadeIn(2f));

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
                        draggedIconIndex = -1;
                        colliededPatternIcon.circleCollider.enabled = false;
                        otherCollider.gameObject.transform.position = icon.gameObject.transform.position;
                        succesfulMatch++;

                        if (succesfulMatch == 2)
                            if (collideCoroutine == null)
                                collideCoroutine = StartCoroutine(RemoveObjects());
                        return;
                    }


            }
        }
    }

    void OnIconMouseCollided(int iconIndex)
    {
        // draggedIconIndex = iconIndex;
    }


    IEnumerator RemoveObjects()
    {
        confettiTop.Play();
        confettiButtom.Play();

        yield return new WaitForSeconds(1f);

        StartCoroutine(IconsFadeOut(2f));




        yield return new WaitForSeconds(1f);

        confettiTop.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        confettiButtom.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        SpawnIcons();
    }

    IEnumerator IconsFadeOut(float alphaReductionSpeed)
    {


        while (true) 
        {
            bool temp = false;

            ;

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
        if (!CentralInputHandler.Instance)
        {
            print("FUCK!");
            // TEST2.transform.position = Vector2.zero;
        }
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
                draggedIconIndex = patternIcon.iconIndex;
                targetIcon = patternIcon;

                // targetIcon = null;

                // patternIcon.onIconMouseCollided?.Invoke(patternIcon.iconIndex); // TEST
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
                // if (patternIconHolders.Contains(patternIcon))
                // selectedIconIndex = patternIcon.iconIndex;
                patternIcon.onIconMouseCollided?.Invoke(patternIcon.iconIndex);
            }
        }

        if (draggedIconIndex != -1)
        {
            targetIcon.gameObject.transform.position = worldPos;
            print("BRUH!");
        }


    }

    void HandleRelease()
    {
        draggedIconIndex = -1;
        
        targetIcon = null;
        // draggedObject = null;
    }


}
