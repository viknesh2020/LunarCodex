using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviourSingleton<GridManager>
{
   [SerializeField] private Transform gridParent;
   [SerializeField] private Camera orthoCam;
   [Range(0.1f, 2f)]
   [SerializeField] private float padding;
   
   public void SetGridMatrix(int rows, int columns)
   {
      CardManager.Instance.ReturnAllCardsToPool();
      float xMid = (columns - 1) * padding * 0.5f;
      float yMid = (rows - 1) * padding * 0.5f;
      
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
            cardComponent.transform.localPosition = new Vector3(c * padding - xMid, r * padding - yMid, 0);
            
            //Add CardComponent to the list
            CardManager.Instance.cards.Add(cardComponent);
            cardComponent.SetCardValue(cardValuesList[shuffledIndex]);
         }
      }
      CardManager.Instance.SetTotalCardsCount();
   }
}