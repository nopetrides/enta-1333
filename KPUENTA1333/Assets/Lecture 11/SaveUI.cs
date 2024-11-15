using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class SaveUI : MonoBehaviour
{


    private void Start()
    {
        _exampleBuilding = new SavedBuildingData();
        _exampleBuilding.CurrentHp = 37;
        _exampleBuilding.BuildingLevel = 2;
        _exampleBuilding.KindOfType = BuildingType.Defensive;
        _exampleBuilding.OwnerId = 2;
        _exampleBuilding.Position = new Vector3(40, 0, 20);
        Debug.Log(_exampleBuilding.ToString());
        /*
SavedBuildingData
UnityEngine.Debug:Log (object)
SaveUI:Start () (at Assets/Lecture 11/SaveUI.cs:36)
         */
        Debug.Log(JsonUtility.ToJson(_exampleBuilding));
        /*
{"CurrentHp":37,"BuildingLevel":2,"OwnerId":2,"Position":{"x":40.0,"y":0.0,"z":20.0},"KindOfType":3}
UnityEngine.Debug:Log (object)
SaveUI:Start () (at Assets/Lecture 11/SaveUI.cs:43)
         */

        _allSaveData = new AllSaveData();
        _allSaveData._buildings.Add(_exampleBuilding);
        _allSaveData._buildings.Add(new SavedBuildingData());
        _allSaveData._buildings.Add(_exampleBuilding);
        _allSaveData._buildings.Add(_exampleBuilding);

    }

    #region SaveHpPlayerPrefs

    private SaveHpToText _savehpToTextData = new SaveHpToText(7);
    private const string SaveHpKey = "SavedHp";

    public void ButtonSaveHpToPlayerPrefsAsInt()
    {
        PlayerPrefs.SetInt(SaveHpKey, _savehpToTextData.CurrentHp);
        PlayerPrefs.Save();
    }

    public void ButtonLoadHpFromPlayerPrefsAsInt()
    {
        int hp = PlayerPrefs.GetInt(SaveHpKey);
        Debug.Log($"Saved data was: {hp}");
        _savehpToTextData.CurrentHp = hp;
    }
    #endregion

    #region Save Building
    private SavedBuildingData _exampleBuilding;

    public void ButtonSaveSingleBuildingToPlayerPrefs()
    {
        PlayerPrefs.SetString(SaveHpKey, JsonUtility.ToJson(_exampleBuilding));
        PlayerPrefs.Save();
    }

    public void ButtonLoadSingleBuildingFromPlayerPrefs()
    {
        string dataAsString = PlayerPrefs.GetString(SaveHpKey);
        SavedBuildingData building = JsonUtility.FromJson<SavedBuildingData>(dataAsString);
        Debug.Log("Successfully loaded building" + building.ToString());
    }

    #endregion

    #region Save All

    private const string AllDataKey = "AllSaveData";
    private AllSaveData _allSaveData;

    // All Platforms Save 2.0 - free unity store asset for cross platform save files

    public void ButtonSaveAll()
    {
        string saveAsString = JsonUtility.ToJson(_allSaveData);
        
        // to player prefs
        PlayerPrefs.SetString(AllDataKey, saveAsString);
        PlayerPrefs.Save();
        // to file system
        string filePath = Path.Combine(Application.persistentDataPath, "SaveData.txt");
        File.WriteAllText(filePath, saveAsString);

        Debug.Log("Successfully Saved The Game");
    }

    public void ButtonLoadAll()
    {
        string dataAsString = PlayerPrefs.GetString(AllDataKey);
        AllSaveData allSaveData = JsonUtility.FromJson<AllSaveData>(dataAsString);
        string filePath = Path.Combine(Application.persistentDataPath, "SaveData.txt");
        string fileData = File.ReadAllText(filePath);
        Debug.Log("Successfully loaded all save data: " + fileData);
    }

    /*
{
  "_buildings": [
    {
      "CurrentHp": 37,
      "BuildingLevel": 2,
      "OwnerId": 2,
      "Position": {
        "x": 40,
        "y": 0,
        "z": 20
      },
      "KindOfType": 3
    },
    {
      "CurrentHp": 0,
      "BuildingLevel": 0,
      "OwnerId": 0,
      "Position": {
        "x": 0,
        "y": 0,
        "z": 0
      },
      "KindOfType": 0
    },
    {
      "CurrentHp": 37,
      "BuildingLevel": 2,
      "OwnerId": 2,
      "Position": {
        "x": 40,
        "y": 0,
        "z": 20
      },
      "KindOfType": 3
    },
    {
      "CurrentHp": 37,
      "BuildingLevel": 2,
      "OwnerId": 2,
      "Position": {
        "x": 40,
        "y": 0,
        "z": 20
      },
      "KindOfType": 3
    }
  ]
}
     */

    #endregion
}
