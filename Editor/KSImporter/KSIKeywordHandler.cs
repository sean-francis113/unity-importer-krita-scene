﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KSIKeywordHandler{

    public string keyword;
    public ImportHandler handler;

    /// <summary>
    /// Initialize a KeywordHandler Object
    /// </summary>
    public KSIKeywordHandler()
    {

        keyword = "";
        handler = ImportHandler.NONE;

    }

    /// <summary>
    /// Initialize a KeywordHandler Object
    /// </summary>
    /// <param name="word">The Keyword</param>
    /// <param name="t">The Handler Type</param>
    public KSIKeywordHandler(string word, ImportHandler t)
    {

        keyword = word;
        handler = t;

    }

    public static void AddKeyword(int indexToChange)
    {

        int i = 0;

        if (KSIData.keywordList.Count == 0 || indexToChange == KSIData.keywordList.Count)
        {

            //New Null Reference is Added
            KSIData.keywordList.Add(new KSIKeywordHandler());

        }
        else if ((indexToChange < 0 &&
               KSIUI.DisplayDialog("Error: Index Out of Range (Small)", "Provided Index is Too Small! Would You Like to Add a Keyword at the First Index?", "Add Keyword", "Do Not Add Keyword")))
        {

            //New Null Reference is Added At End of List
            KSIData.keywordList.Add(new KSIKeywordHandler());

            //Get the Last Index of the List
            i = KSIData.keywordList.Count - 1;

            //While We Can Still Move Up the List
            while (i > 0)
            {

                //Temporarily Grab Object at i
                KSIKeywordHandler temp = KSIData.keywordList[i];
                //Temporarily Grab Object Above i
                KSIKeywordHandler aboveTemp = KSIData.keywordList[i - 1];

                //Move the Object Above i Down
                KSIData.keywordList[i] = aboveTemp;
                //Move the Object at i Up
                KSIData.keywordList[i - 1] = temp;

                i--;

            }

            KSIUI.DisplayKeywordList();

        }
        else if ((indexToChange > KSIData.keywordList.Count - 1 &&
                KSIUI.DisplayDialog("Error: Index Out of Range (Large)", "Provided Index is Too Large! Would You Like to Add a Keyword at the Last Index?", "Add Keyword", "Do Not Add Keyword")) ||
            (indexToChange < KSIData.keywordList.Count - 1))
        {

            //New Null Reference is Added At End of List
            KSIData.keywordList.Add(new KSIKeywordHandler());

            //Get the Last Index of the List
            i = KSIData.keywordList.Count - 1;

            //While We Can Still Move Up the List
            while (i > indexToChange)
            {

                //Temporarily Grab Object at i
                KSIKeywordHandler temp = KSIData.keywordList[i];
                //Temporarily Grab Object Above i
                KSIKeywordHandler aboveTemp = KSIData.keywordList[i - 1];

                //Move the Object Above i Down
                KSIData.keywordList[i] = aboveTemp;
                //Move the Object at i Up
                KSIData.keywordList[i - 1] = temp;

                i--;

            }

            KSIUI.DisplayKeywordList();

        }

    }

    public static void RemoveKeyword(int indexToChange)
    {

        if (KSIData.keywordList.Count == 0)
        {

            KSIUI.DisplayDialog("Error: No List", "There Are No Entries In the List! Cannot Remove Anything!", "Okay");
            return;

        }

        if ((indexToChange > KSIData.keywordList.Count - 1 &&
                KSIUI.DisplayDialog("Error: Index Out of Range (Large)", "Provided Index is Too Large! Would You Like to Remove the Keyword at the Last Index?", "Remove Keyword", "Do Not Remove Keyword")))
        {

            int i = KSIData.keywordList.Count - 1;

            KSIData.keywordList.RemoveAt(i);
            KSIUI.DisplayKeywordList();

        }
        else if ((indexToChange < 0 &&
                KSIUI.DisplayDialog("Error: Index Out of Range (Small)", "Provided Index is Too Small! Would You Like to Remove the Keyword at the First Index?", "Remove Keyword", "Do Not Remove Keyword")))
        {

            int i = 0;

            KSIData.keywordList.RemoveAt(i);
            KSIUI.DisplayKeywordList();

        }
        else
        {

            KSIData.keywordList.RemoveAt(indexToChange);
            KSIUI.DisplayKeywordList();

        }

    }

    public static string TypeToStr(ImportHandler type)
    {

        return type.ToString();

    }

    public static ImportHandler StrToType(string str)
    {

        switch (str.ToUpper())
        {

            case "NONE":
                return ImportHandler.NONE;
            case "PLATFORM":
                return ImportHandler.PLATFORM;
            case "GROUND":
                return ImportHandler.GROUND;
            case "SCENERY":
                return ImportHandler.SCENERY;
            default:
                return ImportHandler.NONE;

        }

    }

}

/// <summary>
/// Enumerator Holding All Keyword Handler Types 
/// </summary>
public enum ImportHandler
{

    /// <summary>
    /// Only Save the Image. Do Not Add to Scene or Add Components to Image
    /// </summary>
    NONE,
    /// <summary>
    /// Add to Scene, Add a Collider(Box), Add a Platform Effector, Add Platform Layer to Image (If Platform Layer Exists) 
    /// </summary>
    PLATFORM,
    /// <summary>
    /// Add to Scene, Add a Collider(Box), Add Ground Layer to Image (If Ground Layer Exists)
    /// </summary>
    GROUND,
    /// <summary>
    /// Add to Scene, Add Scenery Layer to Image (If Scenery Layer Exists)
    /// </summary>
    SCENERY
    //<Template Creator Will Add User Created Handlers Below Here>

};