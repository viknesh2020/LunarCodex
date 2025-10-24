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
   
   private List<Button> levelUICardButtons = new List<Button>();
   private int currentRows;
   private int currentColumns;

   private void Awake()
   {
      GridValidation();
      PopulateLevelMenu();
      proceedToGame.onClick.AddListener(SwitchToGame);
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
      for (int i = 0; i < levelData.levelDataItems.Length; i++)
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
      }
   }
   
   private void SwitchToGame()
   {
      Initialize.Instance.ProceedToGame();
      GridManager.Instance.SetGridMatrix(currentRows, currentColumns);
   }
   public void SetCurrentRowsAndColumns(int rows, int columns)
   {
      currentRows = rows;
      currentColumns = columns;
   }
   
}