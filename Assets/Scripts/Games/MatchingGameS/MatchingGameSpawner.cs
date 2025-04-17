using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class MatchingGameSpawner : MonoBehaviour
{

    [SerializeField] GameObject matchingIconGO;
    List<MatchingIcon> topMatchingIcon = new List<MatchingIcon>();
    List<MatchingIcon> buttomMatchingIcon = new List<MatchingIcon>();

    List<LineRenderer> lines = new List<LineRenderer>();

    int selectedIconIndex = -1;

    LineRenderer line;

    enum SelectedIconSide
    {
        top,
        bottom,
        none
    }

    SelectedIconSide selectedIconSide = SelectedIconSide.none;

    void Start()
    {

        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
        line.enabled = false;

        SpawnIcons();


      


    }



    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            line.enabled = false;

            selectedIconIndex = -1;

            selectedIconSide = SelectedIconSide.none;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        line.SetPosition(1, mousePos);

//#if UNITY_EDITOR
//        // استفاده از موس توی ادیتور
//        if (Input.GetMouseButtonDown(0))
//                {
//                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                    Collider2D hit = Physics2D.OverlapPoint(worldPoint);

//                    if (hit != null && hit == GetComponent<Collider2D>())
//                    {
//                        Debug.Log("Clicked on the circle!");
//                    }
//                }
//        #else
//                // استفاده از تاچ توی موبایل
//                if (Input.touchCount > 0)
//                {
//                    Touch touch = Input.GetTouch(0);

//                    if (touch.phase == TouchPhase.Began)
//                    {
//                        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
//                        Collider2D hit = Physics2D.OverlapPoint(worldPoint);

//                        if (hit != null && hit == GetComponent<Collider2D>())
//                        {
//                            Debug.Log("Touched the circle!");
//                        }
//                    }
//                }
//        #endif
    }

    void SpawnIcons()
    {
        selectedIconIndex = -1;

        topMatchingIcon = new List<MatchingIcon>();

        buttomMatchingIcon = new List<MatchingIcon>();

        lines = new List<LineRenderer>();



        List<Vector2> Positions = SpawnUtility.GetUpDividedPositions(4);

        GameObject tempMatchingIconGO;

        MatchingIcon tempMatchingIcon;

        List<MatchingIcon> matchingIcons = new List<MatchingIcon>();


        Texture2D[] allTextures = SpawnUtility.LoadTextures(SpawnUtility.IconTypes.Animals);

        List<int> texturesRandomSelection = SpawnUtility.RandomNumberGenerator(0, allTextures.Length - 1, 5);

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

            tempMatchingIcon.onIconCollided += OnTopIconCollided;

            temp++;

        }

        Positions = SpawnUtility.GetDownDividedPositions(4);

        temp = 0;

        texturesRandomSelection = SpawnUtility.RandomNumberGenerator(0, 4, 5);

        foreach (Vector2 position in Positions)
        {
            tempMatchingIconGO = Instantiate<GameObject>(matchingIconGO, position, Quaternion.identity);

            tempMatchingIcon = tempMatchingIconGO.GetComponent<MatchingIcon>();


            tempMatchingIcon.SetKnobToTheButtom();


            tempMatchingIcon.spriteRenderer.sprite = topMatchingIcon[texturesRandomSelection[temp]].spriteRenderer.sprite;

            tempMatchingIcon.iconIndex = topMatchingIcon[texturesRandomSelection[temp]].iconIndex;

            buttomMatchingIcon.Add(tempMatchingIcon);

            tempMatchingIcon.onIconCollided += OnButtomIconCollided;

            temp++;
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

            print("SEX");

        }


        selectedIconIndex = iconIndex;
        selectedIconSide = SelectedIconSide.top;

        line.enabled = true;

        line.SetPosition(0, topMatchingIcon[iconIndex].holderKnob.transform.position);

        print(iconIndex);
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
            print("SEX");
            StartCoroutine(RemoveObjects());
        }

        selectedIconIndex = iconIndex;
        selectedIconSide = SelectedIconSide.bottom;

        line.enabled = true;

        line.SetPosition(0, collidedMatchingIcon.holderKnob.transform.position);

        print(iconIndex);
    }


    void CreateNewLine(Vector2 startPos, Vector2 endPos)
    {
        GameObject lineObj = new GameObject("Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        lr.enabled = true;
        lines.Add(lr);
    }


    IEnumerator RemoveObjects()
    {
        yield return new WaitForSeconds(2f);
        
        if (topMatchingIcon.Count > 0)
            foreach(MatchingIcon matchingIcon in topMatchingIcon)
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


        yield return new WaitForSeconds(1f);
        SpawnIcons();
    }
}

