﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class KSIDataValidator{

    /// <summary>
    /// Checks the Inputted Data to See if it is Valid
    /// </summary>
    /// <returns>True if Valid; False Otherwise</returns>
    public static bool CheckData()
    {

        //If the Base File Path is Invalid
        if (!CheckBaseFilePath())
        {

            return false;

        }

        //If the Use Folder Settings are Invalid
        if(KSIData.useFolders && !CheckUseFolder())
        {

            return false;

        }

        //If the Keyword Handling is Invalid
        if(!CheckKeywordHandling())
        {

            return false;

        }

        //If the Scene File Path is Invalid
        if(!CheckSceneFilePath())
        {

            return false;

        }

        //If the Image File Path is Invalid
        if(!CheckImageFilePath())
        {

            return false;

        }

        //If the Custom Name Settings are Invalid
        if(KSIData.useCustomNames && !CheckCustomNames())
        {

            return false;

        }
        
        //All of the Data Is Correct
        return true;

    }

    /// <summary>
    /// Checks the Validation of the Base File Path Set by the User
    /// </summary>
    /// <returns>True if Valid; False Otherwise</returns>
    private static bool CheckBaseFilePath()
    {

        //Check if the File Path Field is Empty
        if (KSIData.baseFilePath == "")
        {

            //Display Error and Return False
            KSIUI.DisplayDialog("Error: File Path Empty", "The File Path Field is Empty! Cannot Continue Import!", "Okay");
            return false;

        }

        //Check if the File Path Specified Doesn't Exist
        if (!Directory.Exists(KSIData.baseFilePath))
        {

            //Display Error and Return False
            KSIUI.DisplayDialog("Error: File Path Does Not Exist",
                "The File Path Specified for Import Does Not Exist! Cannot Continue Import!",
                "Okay");
            return false;

        }

        //If the File Path Specified Does Not End With a Slash
        if (!KSIData.baseFilePath.EndsWith("/") &&
            !KSIData.baseFilePath.EndsWith("\\"))
        {

            string temp = KSIData.baseFilePath + "/";
            KSIData.baseFilePath = temp;
            Debug.Log("Added Slash to the End of FilePath");

        }

        return true;

    }

    /// <summary>
    /// Checks the Validation of the Settings Under 'Use Folder'
    /// </summary>
    /// <returns>True if Valid or if 'Use Folders' is No Longer True; False Otherwise</returns>
    private static bool CheckUseFolder()
    {

        //If the XML Folder Field is Empty
        if (KSIData.useFolders && KSIData.xmlFolder == "")
        {

            //Let the User Choose if They Want to Return to Fix the Empty Field or Not Use Sub-Folders
            if (KSIUI.DisplayDialog("Error: Use Folders True But XML Folder Empty",
                "You Have Set 'Use Folders' to True Yet the XML Folder Field is Empty. Do You Want to Use Folders?\n\nYes: Return to Importer\nNo: Continue Import",
                "Yes", "No"))
            {

                return false;

            }
            else
            {

                KSIData.useFolders = false;
                return true;

            }

        }

        //If The Slashes Were Not Typed Out
        if (!KSIData.xmlFolder.EndsWith("/") &&
           !KSIData.xmlFolder.EndsWith("\\"))
        {

            string temp = KSIData.xmlFolder + "/";
            KSIData.xmlFolder = temp;
            Debug.Log("Added Slash to End of XML Folder");

        }

        return true;

    }

    /// <summary>
    /// Checks the Validation of the Keywords Set in the Importer
    /// </summary>
    /// <returns>True if Valid; False Otherwise</returns>
    private static bool CheckKeywordHandling()
    {

        //Check if There Are Keywords in List
        if (KSIData.keywordList.Count == 0)
        {

            //Display Error and Return False
            KSIUI.DisplayDialog("Error: No Keywords",
                "There Are No Keywords Specified in the Importer! Cannot Continue Import!",
                "Okay");
            return false;

        }

        //A Flag Set by the User
        //Does the User Want to Just Continue With Import When an Invalid Handler is Found
        bool ignoreHandlers = false;

        //Make Sure All Keywords Have a Valid Keyword and Valid Handler
        foreach (var word in KSIData.keywordList)
        {

            //If There is No Keyword in Field
            if (word.keyword == "")
            {

                //Display Error and Return False
                KSIUI.DisplayDialog("Error: Not All Keywords Valid",
                    "Not All Keyword Fields Have Words Specified! Cannot Continue Import!",
                    "Okay");
                return false;

            }

            //If the Handler is Set to NONE
            if (word.handler == ImportHandler.NONE && !ignoreHandlers)
            {

                //Give User a Chance to Continue or Quit Import
                if (!KSIUI.DisplayDialog("Error: Not All Handlers Valid",
                    "Not All Keyword Fields Have a Valid Handler Type Specified! Would You Like to Continue Import?\n\n" +
                    "NOTE: Any Handlers Specified as 'NONE' Will Only Save the Files, They Will Not Be Created In Scene " +
                    "nor Have Any Components Added To Them!\n\nYes: Continue Import\nNo: Stop Import", "Yes", "No"))
                {

                    return false;

                }
                else
                {

                    ignoreHandlers = true;

                }

            }

        }

        return true;

    }

    /// <summary>
    /// Checks Validation of the Scene File Path Set By User
    /// </summary>
    /// <returns>True if Valid; False Otherwise</returns>
    private static bool CheckSceneFilePath()
    {

        Debug.Log("Scene File Path Before Check: " + KSIData.sceneFilePath);

        //If the Scene File Path to Import To is Empty
        if (KSIData.sceneFilePath == "")
        {

            if (KSIUI.DisplayDialog("Error: Scene File Path is Empty",
                "The Scene File Path Field is Empty! Do You Want to Save Everything to the 'Assets' Folder?\n\nYes: Save to 'Assets' Folder\nNo: Return to Importer",
                "Yes", "No"))
            {

                KSIData.sceneFilePath = "Assets/";

            }
            else
            {

                return false;

            }

        }

        //Else Add the Data Path to the File Path
        else
        {

            //Scene File Path Not In Relation to Project
            if(!KSIData.sceneFilePath.StartsWith("Assets/") && !KSIData.sceneFilePath.StartsWith("Assets\\"))
            {

                char[] separators = { '/', '\\' };

                string[] sceneSplit = KSIData.sceneFilePath.Split(separators);
                List<string> splitList = new List<string>();

                //Convert sceneSplit to List
                foreach (var directory in sceneSplit)
                {

                    splitList.Add(directory);

                }

                int assetIndex = 0;

                //If Assets Folder was Included in String, Where is it?
                for(int i = 0; i < splitList.Count; i++)
                {

                    if(splitList[i] == "Assets")
                    {

                        assetIndex = i;
                        Debug.Log("Found Assets At Index " + i);

                    }

                }

                //If We Found Assets Directory
                if(assetIndex != 0)
                {

                    //Remove Everything Up to Asset Directory
                    for(int i = 0; i < assetIndex; i++)
                    {

                        splitList.RemoveAt(0);


                    }

                }

                string newPath = "";

                for(int i = 0; i < splitList.Count; i++)
                {

                    newPath += splitList[i] + "/";

                }

                KSIData.sceneFilePath = newPath;

            }
            else
            {

                if(KSIData.sceneFilePath[KSIData.sceneFilePath.Length - 1] != '/' &&
                    KSIData.sceneFilePath[KSIData.sceneFilePath.Length - 1] != '\\')
                {

                    string temp = KSIData.sceneFilePath + "/";
                    KSIData.sceneFilePath = temp;

                }

            }

        }

        return true;

    }

    /// <summary>
    /// Checks the Validaiton of the Image File Path
    /// </summary>
    /// <returns>True if Valid; False Otherwise</returns>
    private static bool CheckImageFilePath()
    {

        Debug.Log("Image File Path Before Check: " + KSIData.imageFilePath);

        //If the Scene File Path to Import To is Empty
        if (KSIData.imageFilePath == "")
        {

            if (KSIUI.DisplayDialog("Error: Image File Path is Empty",
                "The Image File Path Field is Empty! Do You Want to Save Everything to the 'Assets' Folder?\n\nYes: Save to 'Assets' Folder\nNo: Return to Importer",
                "Yes", "No"))
            {

                KSIData.imageFilePath = Application.dataPath;

            }
            else
            {

                return false;

            }

        }

        //Else Add the Data Path to the File Path
        else
        {

            //Scene File Path Not In Relation to Project
            if (!KSIData.imageFilePath.StartsWith("Assets/") && !KSIData.imageFilePath.StartsWith("Assets\\"))
            {

                char[] separators = { '/', '\\' };

                string[] sceneSplit = KSIData.imageFilePath.Split(separators);
                List<string> splitList = new List<string>();

                //Convert sceneSplit to List
                foreach (var directory in sceneSplit)
                {

                    splitList.Add(directory);

                }

                int assetIndex = 0;

                //If Assets Folder was Included in String, Where is it?
                for (int i = 0; i < splitList.Count; i++)
                {

                    if (splitList[i] == "Assets")
                    {

                        assetIndex = i;

                    }

                }

                //If We Found Assets Directory
                if (assetIndex != 0)
                {

                    //Remove Everything Up to Asset Directory
                    for (int i = 0; i < assetIndex; i++)
                    {

                        splitList.RemoveAt(0);

                    }

                }

                string newPath = "";

                for (int i = 0; i < splitList.Count; i++)
                {

                    newPath += splitList[i] + "/";

                }

                KSIData.imageFilePath = newPath;

            }
            else
            {

                if (KSIData.imageFilePath[KSIData.imageFilePath.Length - 1] != '/' &&
                    KSIData.imageFilePath[KSIData.imageFilePath.Length - 1] != '\\')
                {

                    string temp = KSIData.imageFilePath + "/";
                    KSIData.imageFilePath = temp;

                }

            }

        }

        return true;

    }

    /// <summary>
    /// Checks the Validation of the Custom Name Settings
    /// </summary>
    /// <returns>True if Valid or if Custom Names is No Longer True; False Otherwise</returns>
    private static bool CheckCustomNames()
    {

        //If the Custom Scene Name Field is Empty
        if (KSIData.useCustomNames && KSIData.customSceneName == "")
        {

            switch (KSIUI.DisplayDialog("Error: Use Custom Names True But Custom Image Name Empty",
                "You Have Set 'Use Custom Names' to True Yet the Custom Scene Name Field is Empty.\n\nDo You Want to:\nAdd a Custom Scene Name (Return)\nTurn Off 'Use Custom Names' (Turn Off)\nNot Use Custom Scene Names (Don't Use)",
                "Return", "Turn Off", "Don't Use"))
            {

                case 0:
                    return false;
                case 1:
                    KSIData.useCustomNames = false;
                    return true;
                case 2:
                    return true;
                default:
                    Debug.Log("Unrecognized Operation");
                    break;

            }

        }

        //If the Custom Image Name Field is Empty
        if (KSIData.useCustomNames && KSIData.customImageName == "")
        {

            switch(KSIUI.DisplayDialog("Error: Use Custom Names True But Custom Image Name Empty",
                "You Have Set 'Use Custom Names' to True Yet the Custom Image Name Field is Empty.\n\nDo You Want to:\nAdd a Custom Image Name (Return)\nTurn Off 'Use Custom Names' (Turn Off)\nNot Use Custom Image Names (Don't Use)",
                "Return", "Turn Off", "Don't Use"))
            {

                case 0:
                    return false;
                case 1:
                    KSIData.useCustomNames = false;
                    return true;
                case 2:
                    return true;
                default:
                    Debug.Log("Unrecognized Operation");
                    break;

            }

        }

        return true;

    }

}
