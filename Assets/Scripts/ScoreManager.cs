using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
{
    [Header("Score UI Elements")] public TMP_Text turnsText;
    public TMP_Text matchesText;
    public TMP_Text scoreText;
    public TMP_Text comboText;
    [HideInInspector] public int TurnsCount; 
    [HideInInspector] public int matchesCount;
    [HideInInspector] public int comboCount;
    [HideInInspector] public int currentMatchScore;

    private void Awake()
    {
        ResetAllStats();
    }
    public int GetCurrentScore()
    {
        return currentMatchScore;
    }
    
    public void ResetAllStats()
    {
        ResetTurnsCount();
        ResetMatchesCount();
        ResetComboCount();
        ResetScore();
    }

    public void SetTurnsCount()
    {
        TurnsCount++;
        turnsText.text = TurnsCount.ToString();
    }

    public void LoadTurnsCount(int turnsCount)
    {
        turnsText.text = turnsCount.ToString();
        TurnsCount = turnsCount;
    }

    public void ResetTurnsCount()
    {
        TurnsCount = 0;
        turnsText.text = TurnsCount.ToString();
    }

    public void SetMatchesCount()
    {
        matchesCount++;
        matchesText.text = matchesCount.ToString();
    }

    public void LoadMatchesCount(int mtcsCount)
    {
        matchesText.text = matchesCount.ToString();
        matchesCount = mtcsCount;
    }

    public void ResetMatchesCount()
    {
        matchesCount = 0;
        matchesText.text = matchesCount.ToString();
    }

    public void SetScore(int score)
    {
        Debug.Log("Received Score: " + score);
        currentMatchScore = score;
        scoreText.text = score.ToString();
        Debug.Log("Score in string" +scoreText.text);
    }

    public void ResetScore()
    {
        currentMatchScore = 0;
        scoreText.text = currentMatchScore.ToString();
    }

    public void SetComboCount(int combo)
    {
        comboCount = combo;
        comboText.text = comboCount.ToString();
    }

    public void ResetComboCount()
    {
        comboCount = 0;
        comboText.text = comboCount.ToString();
    }

}
