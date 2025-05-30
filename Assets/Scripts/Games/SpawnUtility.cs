﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SpawnUtility
{
    public enum IconTypes
    {
        Animals,
        Education,
        Farm,
        Fruits,
        HomeAppliances,
        HumanBody,
        Nature
    }

    public static List<Vector2> GetUpDividedPositions(int partsPerHalf = 5, 
                                                      float topOffsetLeft = 0.5f,
                                                      float topOffsetRight = 0.5f,
                                                      float topOffsetUp = 0.5f)
    {
        List<Vector2> dividedPositions = new List<Vector2>();
        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight * Camera.main.aspect;
        float camX = Camera.main.transform.position.x;
        float camY = Camera.main.transform.position.y;
        float halfHeight = camHeight / 2f;
        float usableTopWidth = camWidth - (topOffsetLeft + topOffsetRight);
        float sectionTopWidth = usableTopWidth / partsPerHalf;
        float topY = camY + (halfHeight / 2f) + topOffsetUp;
        float topLeftX = camX - camWidth / 2f + topOffsetLeft;

        for (int i = 0; i < partsPerHalf; i++)
        {
            float x = topLeftX + sectionTopWidth * (i + 0.5f);
            dividedPositions.Add(new Vector2(x, topY));
        }

        return dividedPositions;
    }


    public static List<Vector2> GetDownDividedPositions(int partsPerHalf = 5,
                                                        float bottomOffsetLeft = 0.5f,
                                                        float bottomOffsetRight = 0.5f,
                                                        float bottomOffsetDown = 0.5f)
    {
        List<Vector2> dividedPositions = new List<Vector2>();
        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight * Camera.main.aspect;
        float camX = Camera.main.transform.position.x;
        float camY = Camera.main.transform.position.y;
        float halfHeight = camHeight / 2f;
        float usableBottomWidth = camWidth - (bottomOffsetLeft + bottomOffsetRight);
        float sectionBottomWidth = usableBottomWidth / partsPerHalf;
        float bottomY = camY - (halfHeight / 2f) - bottomOffsetDown;
        float bottomLeftX = camX - camWidth / 2f + bottomOffsetLeft;

        for (int i = 0; i < partsPerHalf; i++)
        {
            float x = bottomLeftX + sectionBottomWidth * (i + 0.5f);
            dividedPositions.Add(new Vector2(x, bottomY));
        }

        return dividedPositions;

    }

    public static Texture2D[] LoadTextures(IconTypes iconType)
    {
        string iconTypeFolderName = "";
        
        switch(iconType)
        {
            case IconTypes.Animals:
                iconTypeFolderName = "Animals";
                break;

            case IconTypes.Farm:
                iconTypeFolderName = "Farm";
                break;

            case IconTypes.Nature:
                iconTypeFolderName = "Nature";
                break;

            case IconTypes.HomeAppliances:
                iconTypeFolderName = "Home Appliance";
                break;

            case IconTypes.HumanBody:
                iconTypeFolderName = "Human Body";
                break;

            case IconTypes.Fruits:
                iconTypeFolderName = "Fruits";
                break;

            case IconTypes.Education:
                iconTypeFolderName = "Education";
                break;
        }

        return Resources.LoadAll<Texture2D>("Icons/" + iconTypeFolderName);
    }


    public static Texture2D[] LoadTextures(string iconCategory)
    {
        if (iconCategory == "") { MonoBehaviour.print("icon category string is empty!"); }
        
        return Resources.LoadAll<Texture2D>("Icons/" + iconCategory);
    }


    public static List <int> RandomNumberGenerator(int min, int max, int length) // with no repetition
    { 
        List<int> pool = Enumerable.Range(min, max).ToList(); // عددهای 0 تا 9
        return pool.OrderBy(x => Random.value).Take(length).ToList();
    }

}
