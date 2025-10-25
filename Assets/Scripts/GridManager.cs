using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviourSingleton<GridManager>
{
   [SerializeField] private Transform gridParent;
   [SerializeField] private Camera orthoCam;
   
   [Header("Grid Padding (World Units)")]
   [SerializeField] private float paddingTop = 0.5f;
   [SerializeField] private float paddingBottom = 0.5f;
   [SerializeField] private float paddingLeft = 0.5f;
   [SerializeField] private float paddingRight = 0.5f;
   
   [Header("Card Sizing")]
   [SerializeField] private float cardBaseSize = 1f;
   [Range(0.1f, 1f)]
   [SerializeField] private float cardSizeFactor = 0.9f;

   public void SetGridMatrix(int rows, int columns)
   {
      CardManager.Instance.ReturnAllCardsToPool();

      float camHalfHeight = orthoCam.orthographicSize;
      float camHalfWidth = camHalfHeight * orthoCam.aspect;
      
      float availableWidth = (camHalfWidth * 2f) - paddingLeft - paddingRight;
      float availableHeight = (camHalfHeight * 2f) - paddingTop - paddingBottom;
      
      // Calculate the size of each "cell" (card + spacing)
      float cellSpaceX = availableWidth / columns;
      float cellSpaceY = availableHeight / rows;
      
      float cellSpacing = Mathf.Min(cellSpaceX, cellSpaceY);
      
      float cardActualSize = cellSpacing * cardSizeFactor;
      float cardScale = cardActualSize / cardBaseSize;
      
      // Calculate the total width/height of the grid (from center of first card to center of last card)
      float gridWidth = (columns - 1) * cellSpacing;
      float gridHeight = (rows - 1) * cellSpacing;
      
      float xMid = gridWidth * 0.5f;
      float yMid = gridHeight * 0.5f;
      
      // Calculate the offset needed to center the entire grid based on manual padding
      float xCenterOffset = (paddingLeft - paddingRight) * 0.5f;
      float yCenterOffset = (paddingBottom - paddingTop) * 0.5f;
      
      gridParent.position = new Vector3(xCenterOffset, yCenterOffset, 0);
      
      int numberOfPairs = (rows * columns)/2;
      List<int> cardValuesList = CardManager.Instance.GeneratePairedCardValues(numberOfPairs);

      for (int r = 0; r < rows; r++)
      {
         for (int c = 0; c < columns; c++)
         {
            int shuffledIndex = r * columns + c;
            if(shuffledIndex >=cardValuesList.Count) break;
            
            //Object pooling for better performance. Avoided destroying and instantiating multiple
            //gameobjects, which increases the GC spike.
            CardComponent cardComponent = CardManager.Instance.GetCardFromPool();
            cardComponent.transform.SetParent(gridParent);
            
            cardComponent.transform.localPosition = new Vector3(c * cellSpacing - xMid, 
                                                                  r * cellSpacing - yMid, 0);
            
            cardComponent.transform.localScale = new Vector3(cardScale, cardScale, 1f);
            
            //Add CardComponent to the list
            CardManager.Instance.cards.Add(cardComponent);
            cardComponent.SetCardValue(cardValuesList[shuffledIndex]);
         }
      }
      CardManager.Instance.SetTotalCardsCount();
   }
}