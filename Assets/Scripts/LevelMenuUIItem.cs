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

   private void OnEnable()
   {
      _thisButton = GetComponent<Button>();
      _thisButton.onClick.AddListener(SetGridDataForGame);
   }

   private void SetGridDataForGame()
   {
      int rows = int.Parse(rowText.text);
      int columns = int.Parse(columnText.text);
      
      GridManager.Instance.SetGridMatrix(rows, columns);
   }

   private void OnDisable()=> _thisButton.onClick.RemoveListener(SetGridDataForGame);
   
}
