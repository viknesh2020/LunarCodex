using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
   public LevelData levelData;
   public GameObject levelMenuUIObject;
   public RectTransform levelMenuUIParent;
   public Button proceedToGame;
   public GameObject levelCompleteUI;

   [Header("Level Complete Buttons")] 
   public Button homeButton;
   public Button retryButton;
   public Button nextLevelButton;
   
   private List<Button> levelUICardButtons = new List<Button>();
   private int currentRows;
   private int currentColumns;

   private int currentLevelId; // Store the ID of the level being played
   private List<LevelMenuUIItem> levelUIItems = new List<LevelMenuUIItem>(); 

   private void Awake()
   {
      levelCompleteUI.SetActive(false);
      GridValidation();
      PopulateLevelMenu();
      
      //Button event subscriptions, to avoid manually plugging them in.
      proceedToGame.onClick.AddListener(SwitchToGame);
      homeButton.onClick.AddListener(SetBackToMainMenu);
      retryButton.onClick.AddListener(RetryLevel);
      nextLevelButton.onClick.AddListener(NextLevel);
      
      // Subscribe to data loaded event to update level stars
      SaveLoadManager.OnDataLoaded += UpdateLevelMenuUI;
   }

   private void GridValidation()
   {
      var validMatrix = new List<LevelDataItem>();

      foreach(var data in levelData.levelDataItems)
      {
         var matrix = data.rows * data.columns;
         if (matrix % 2 == 0 && matrix > 0)
         {
            validMatrix.Add(data);
         }
      }
      levelData.levelDataItems = validMatrix.ToArray();
   }

   public void PopulateLevelMenu()
   {
      // Clear old UI items list if re-populating
      levelUIItems.Clear(); 
      levelUICardButtons.Clear();
      
      /*for (int i = 0; i < levelData.levelDataItems.Length; i++)
      {
         GameObject levelMenuCard = Instantiate(levelMenuUIObject, levelMenuUIParent.transform);
         levelMenuCard.GetComponent<LevelMenuUIItem>().levelNameText.text = levelData.levelDataItems[i].levelName;
         levelMenuCard.GetComponent<LevelMenuUIItem>().rowText.text = levelData.levelDataItems[i].rows.ToString();
         levelMenuCard.GetComponent<LevelMenuUIItem>().columnText.text = levelData.levelDataItems[i].columns.ToString();
         var starLevels = levelMenuCard.GetComponent<LevelMenuUIItem>().starIcons;
         foreach (var star in starLevels)
         {
            star.color = levelData.levelDataItems[i].normalColor;
         }
         
         levelUICardButtons.Add(levelMenuCard.GetComponent<Button>());
      }*/
      
      for (int i = 0; i < levelData.levelDataItems.Length; i++)
      {
         GameObject levelMenuCard = Instantiate(levelMenuUIObject, levelMenuUIParent.transform);
         
         // Get the UI item component
         LevelMenuUIItem uiItem = levelMenuCard.GetComponent<LevelMenuUIItem>();
         
         // Initialize it with data
         LevelDataItem data = levelData.levelDataItems[i];
         uiItem.Initialize(data.id, data.levelName, data.rows, data.columns, data.normalColor);
         
         // Store reference for updates
         levelUIItems.Add(uiItem);
         
         levelUICardButtons.Add(levelMenuCard.GetComponent<Button>());
      }
      
      // Update the UI with any loaded save data right after populating
      UpdateLevelMenuUI();
   }
   public void UpdateLevelMenuUI()
   {
      if (SaveLoadManager.Instance == null)
      {
         Debug.LogWarning("SaveLoadManager not ready, skipping UI update.");
         return;
      }

      foreach (LevelMenuUIItem uiItem in levelUIItems)
      {
         LevelSaveData loadedData = SaveLoadManager.Instance.GetLevelData(uiItem.levelId);
         uiItem.UpdateUI(loadedData); // Tell the UI item to update its stars
      }
   }
   
   private void SwitchToGame()
   {
      ScoreManager.Instance.ResetAllStats();
      CardManager.Instance.ResetLevelStats(); 
      Initialize.Instance.ProceedToGame();
      GridManager.Instance.SetGridMatrix(currentRows, currentColumns);
   }
   public void SetCurrentRowsAndColumns(int rows, int columns, int levelId)
   {
      currentRows = rows;
      currentColumns = columns;
      currentLevelId = levelId;
   }
   public void SetLevelComplete()
   {
      levelCompleteUI.SetActive(true);
      Debug.Log("LevelComplete from level manager");
      
      // --- ADDED: SAVE LOGIC ---
      int starRating = CalculateStarRating();
      
      LevelSaveData levelData = new LevelSaveData
      {
         levelId = currentLevelId,
         turnsCount = ScoreManager.Instance.TurnsCount,
         matchesCount = ScoreManager.Instance.matchesCount,
         score = ScoreManager.Instance.GetCurrentScore(),
         comboCount = ScoreManager.Instance.comboCount,
         starRating = starRating
      };

      // Send data to the manager, which will handle saving if score is new high
      SaveLoadManager.Instance.UpdateLevelData(levelData);
      // --- END SAVE LOGIC ---
   }
   
   private int CalculateStarRating()
   {
      int score = ScoreManager.Instance.GetCurrentScore();
      // int turns = ScoreManager.Instance.TurnsCount;
        
      // Example: 3 stars for score > 150, 2 for > 75, 1 for any score > 0
      if (score > 150) return 3;
      if (score > 75) return 2;
      if (score > 0) return 1;
      return 0; // No stars
   }

   public void SetBackToMainMenu()
   {
      levelCompleteUI.SetActive(false);
      ScoreManager.Instance.ResetAllStats();
      CardManager.Instance.ResetLevelStats();
      CardManager.Instance.ReturnAllCardsToPool();
      Initialize.Instance.BackToMainMenu();
   }

   public void RetryLevel()
   {
      levelCompleteUI.SetActive(false);
      ScoreManager.Instance.ResetAllStats();
      CardManager.Instance.ResetCardCount();
      CardManager.Instance.ResetLevelStats();
      CardManager.Instance.ReturnAllCardsToPool();
      SwitchToGame();
   }

   public void NextLevel()
   {
      levelCompleteUI.SetActive(false);
      CardManager.Instance.ReturnAllCardsToPool();
   }
   
   private void OnDestroy()
   {
      // Unsubscribe
      SaveLoadManager.OnDataLoaded -= UpdateLevelMenuUI;
   }
}