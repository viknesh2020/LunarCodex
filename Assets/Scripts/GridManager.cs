using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviourSingleton<GridManager>
{
   [SerializeField] private Transform gridParent;
   [SerializeField] private Camera orthoCam;
   [SerializeField] private GameObject cardGo;
   [Range(0.1f, 2f)]
   [SerializeField] private float padding;
   
   public void SetGridMatrix(int rows, int columns)
   {
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
            
            GameObject newCard = Instantiate(cardGo, gridParent);
            newCard.transform.localPosition = new Vector3(c * padding - xMid, r * padding - yMid, 0);
            CardManager.Instance.cards.Add(newCard);
            
            CardComponent cardComponent = newCard.GetComponent<CardComponent>();
            cardComponent.SetCardValue(cardValuesList[shuffledIndex]);
         }
      }
   }
}
