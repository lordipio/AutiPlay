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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AdjustCamera.instance.SetOrientationAndWait(ScreenOrientation.LandscapeLeft, SpawnIcons));
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            draggedIconIndex = -1;
        }


        if (draggedIconIndex != -1)
        {
            print("pattern icons: " + draggedIconIndex);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            patternIcons[draggedIconIndex].transform.position = mousePos;
        }

    }

    private void SpawnIcons()
    {
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
        draggedIconIndex = iconIndex;
    }


    IEnumerator RemoveObjects()
    {
        yield return new WaitForSeconds(2f);

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

        yield return new WaitForSeconds(1f);

        SpawnIcons();
    }
}
