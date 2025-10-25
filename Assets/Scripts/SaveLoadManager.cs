using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq; 
using System; 

[System.Serializable]
public class LevelSaveData
{
    public int levelId;
    public int turnsCount;
    public int matchesCount;
    public int score;
    public int comboCount;
    public int starRating; 
}

[System.Serializable]
public class GameSaveData
{
    public List<LevelSaveData> allLevelData;
}

public class SaveLoadManager : MonoBehaviourSingleton<SaveLoadManager>
{
    public static event Action OnDataLoaded;

    public GameSaveData gameSaveData;
    private const string SaveFileName = "saveData.json";

    public string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); 
    }

    public bool DoesSaveFileExist()
    {
        return File.Exists(SaveFilePath);
    }

    public void LoadGame()
    {
        if (DoesSaveFileExist())
        {
            try
            {
                string json = File.ReadAllText(SaveFilePath);
                gameSaveData = JsonUtility.FromJson<GameSaveData>(json);
                
                int loadedScore = gameSaveData.allLevelData[0].score;
                ScoreManager.Instance.SetScore(loadedScore);

                if (gameSaveData.allLevelData == null)
                {
                    gameSaveData.allLevelData = new List<LevelSaveData>();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load save data: {e.Message}. Creating new save.");
                InitializeNewSave();
            }
        }
        else
        {
            Debug.Log("No save file found. Initializing new save data.");
            InitializeNewSave();
        }
        OnDataLoaded?.Invoke();
    }

    private void InitializeNewSave()
    {
        gameSaveData = new GameSaveData
        {
            allLevelData = new List<LevelSaveData>()
        };
    }

    public void SaveGame()
    {
        try
        {
            string json = JsonUtility.ToJson(gameSaveData, true); 
            File.WriteAllText(SaveFilePath, json);
            Debug.Log($"Game data saved to {SaveFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save data: {e.Message}");
        }
    }

    public void ClearSaveData()
    {
        if (DoesSaveFileExist())
        {
            try
            {
                File.Delete(SaveFilePath);
                Debug.Log("Save file deleted.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save file: {e.Message}");
            }
        }

        /*InitializeNewSave();
        OnDataLoaded?.Invoke();*/
    }

    public LevelSaveData GetLevelData(int levelId)
    {
        if (gameSaveData == null || gameSaveData.allLevelData == null)
        {
            return null;
        }

        return gameSaveData.allLevelData.FirstOrDefault(level => level.levelId == levelId);
    }

    public void UpdateLevelData(LevelSaveData newLevelData)
    {
        if (gameSaveData == null)
        {
            InitializeNewSave();
        }

        LevelSaveData existingData = GetLevelData(newLevelData.levelId);

        if (existingData != null)
        {
            if (newLevelData.score > existingData.score)
            {
                gameSaveData.allLevelData.Remove(existingData);
                gameSaveData.allLevelData.Add(newLevelData);
                Debug.Log($"Updating high score for level {newLevelData.levelId}");
            }
            else
            {
                Debug.Log($"Keeping existing high score for level {newLevelData.levelId}");
                return; 
            }
        }
        else
        {
            gameSaveData.allLevelData.Add(newLevelData);
            Debug.Log($"Saving new entry for level {newLevelData.levelId}");
        }

        SaveGame();
        
        OnDataLoaded?.Invoke();
    }
}