using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviourSingleton<GridManager>
{
   [SerializeField] private Transform gridParent;
   [SerializeField] private Camera orthoCam;
   [SerializeField] private GameObject cardGo;
   [Range(0.1f, 2f)]
   [SerializeField] private float padding;
   
   private List<GameObject> _cards = new List<GameObject>();
   public void SetGridMatrix(int rows, int columns)
   {
      float xMid = (columns - 1) * padding * 0.5f;
      float yMid = (rows - 1) * padding * 0.5f;

      for (int r = 0; r < rows; r++)
      {
         for (int c = 0; c < columns; c++)
         {
            GameObject newCard = Instantiate(cardGo, gridParent);
            newCard.transform.localPosition = new Vector3(c * padding - xMid, r * padding - yMid, 0);
            _cards.Add(newCard);
         }
      }
   }

   public void DestroyCards()
   {
      foreach(GameObject card in _cards) 
      {
         Destroy(card);
      }
      _cards.Clear();
   }
}
