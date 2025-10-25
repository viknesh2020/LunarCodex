using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class CardManager : MonoBehaviourSingleton<CardManager>
{
    public List<GameObject> cards = new List<GameObject>();
    public int rows;
    public int columns;
    
    [Header("Card Settings")]
    public GameObject cardPrefab;

    [Header("Scoring")]
    public int baseMatchScore = 10;
    public int comboBonus = 20;

    private List<CardComponent> cardPool = new List<CardComponent>();

    private List<CardComponent> currentlyFlippedCards = new List<CardComponent>();
    private CardComponent lastMatchedCard = null;
    private int score = 0;
    private int comboCount = 0;
    private int cardCount = 0;

    public void SetTotalCardsCount()
    {
        cardCount = cards.Count;
    }

    public void DestroyCards()
    {
        foreach(GameObject card in cards) 
        {
            Destroy(card);
        }
        cards.Clear();
    }
    
    public CardComponent GetCardFromPool()
    {
        CardComponent cardToUse;

        if (cardPool.Count > 0)
        {
            cardToUse = cardPool[0];
            cardPool.RemoveAt(0);
        }
        else
        {
            GameObject cardObject = Instantiate(cardPrefab, transform);
            cardToUse = cardObject.GetComponent<CardComponent>();
        }

        cardToUse.gameObject.SetActive(true);
        return cardToUse;
    }

    public void ReturnCardToPool(CardComponent card)
    {
        card.gameObject.SetActive(false);
        cardPool.Add(card);
    }

    public List<int> GeneratePairedCardValues(int numberOfPairs)
    {
        List<int> cardValues = new List<int>();

        for (int i = 0; i < numberOfPairs; i++)
        {
            cardValues.Add(i);
            cardValues.Add(i);
        }

        var rng = new System.Random();
        return cardValues.OrderBy(item => rng.Next()).ToList();
    }
    
    public void OnCardTapped(CardComponent tappedCard)
    {
        if (tappedCard.isMatched || currentlyFlippedCards.Contains(tappedCard))
        {
            return;
        }

        currentlyFlippedCards.Add(tappedCard);
        
        if (currentlyFlippedCards.Count == 2)
        {
            CardComponent card1 = currentlyFlippedCards[0];
            CardComponent card2 = currentlyFlippedCards[1];

            if (card1.cardValue == card2.cardValue)
            {
                Debug.Log($"Match Found! Value: {card1.cardValue}");
                ProcessSuccessfulMatch(card1, card2);
            }
            else
            {
                Debug.Log("No Match.");
                StartCoroutine(card1.GetComponent<CardComponent>().CardNotMatched());
                StartCoroutine(card2.GetComponent<CardComponent>().CardNotMatched());
                comboCount = 0;
                lastMatchedCard = null;
            }

            currentlyFlippedCards.Clear();
        }
    }

    private void ProcessSuccessfulMatch(CardComponent card1, CardComponent card2)
    {
        card1.GetComponent<CardComponent>().CardMatched();
        card2.GetComponent<CardComponent>().CardMatched();
        
        ScoreManager.Instance.SetMatchesCount();
        cardCount = cardCount-2;    //Check if all cards are turned and matched.
        
        if (cardCount <= 0)
        {
           LevelManager.Instance.SetLevelComplete();
           AudioManager.Instance.PlaySound(AudioManager.SoundType.LevelComplete);
        }
        
        int currentMatchScore = baseMatchScore;

        if (lastMatchedCard != null)
        {
            comboCount++;
            ScoreManager.Instance.SetComboCount(comboCount);
            currentMatchScore += comboBonus * comboCount;
            Debug.Log($"COMBO x{comboCount}! Bonus: +{comboBonus * comboCount}");
        }
        else
        {
            comboCount = 0;
            ScoreManager.Instance.ResetComboCount();
        }
        
        score += currentMatchScore;
        Debug.Log($"Score: {score}");
        ScoreManager.Instance.SetScore(score);
        lastMatchedCard = card1;
    }
    
    public void ResetCardCount() => cardCount = 0;
}