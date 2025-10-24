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
      Debug.Log($"SetGridMatrix: rows {rows}, columns {columns}");

      float halfLength = (columns - 1) * padding * 0.5f;
      float halfHeight = (rows - 1) * padding * 0.5f;

      for (int row = 0; row < rows; row++)
      {
         for (int col = 0; col < columns; col++)
         {
            GameObject newCard = Instantiate(cardGo, gridParent);
            newCard.transform.localPosition = new Vector3(col * padding - halfLength, row * padding - halfHeight, 0);
            
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
