using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuUIItem : MonoBehaviour
{
   public TMP_Text levelNameText;
   public TMP_Text rowText;
   public TMP_Text columnText;
   public List<Image> starIcons = new List<Image>();
   
   private Button _thisButton;
   
   [Header("Star Colors")]
   public Color starAchievedColor = Color.yellow; // The color for an earned star
   
   [HideInInspector] public int levelId; // Will be set by LevelManager
   private Color starNormalColor; // The default "un-earned" star color

   private void OnEnable()
   {
      _thisButton = GetComponent<Button>();
      _thisButton.onClick.AddListener(SetGridDataForGame);
   }
   
   public void Initialize(int id, string levelName, int rows, int columns, Color normalColor)
   {
      levelId = id;
      levelNameText.text = levelName;
      rowText.text = rows.ToString();
      columnText.text = columns.ToString();
      starNormalColor = normalColor; // Store the default color
        
      // Set initial star color to default
      foreach (var star in starIcons)
      {
         star.color = starNormalColor;
      }
   }
   
   public void UpdateUI(LevelSaveData data)
   {
      if (data == null)
      {
         // No save data for this level, set all stars to normal color
         foreach (var star in starIcons)
         {
            star.color = starNormalColor;
         }
      }
      else
      {
         // We have save data, apply the star rating
         for (int i = 0; i < starIcons.Count; i++)
         {
            // If index is less than rating (e.g., i=0,1 for 2 stars), color it.
            if (i < data.starRating)
            {
               starIcons[i].color = starAchievedColor;
            }
            else
            {
               starIcons[i].color = starNormalColor;
            }
         }
      }
   }

   private void SetGridDataForGame()
   {
      int rows = int.Parse(rowText.text);
      int columns = int.Parse(columnText.text);
      
      LevelManager.Instance.SetCurrentRowsAndColumns(rows, columns, levelId);
      CardManager.Instance.rows = rows;
      CardManager.Instance.columns = columns;
   }

   private void OnDisable()=> _thisButton.onClick.RemoveListener(SetGridDataForGame);
   
}
