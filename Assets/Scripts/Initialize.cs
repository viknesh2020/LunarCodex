using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Initialize : MonoBehaviourSingleton<Initialize>
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject gameMenu;
    
    [Header("Save/Load UI")]
    [SerializeField] private Button loadButton; 
    [SerializeField] private Button saveButton; 
    [SerializeField] private Button clearSaveButton; 

    private void Awake()
    {
        if(mainMenu != null) mainMenu.SetActive(true); //Make sure to check the reference is assigned and turn on the main menu. 
        if(levelMenu.activeSelf) levelMenu.SetActive(false); //turn off other menu items when initialize.
        if(gameMenu.activeSelf) gameMenu.SetActive(false);
        
        // Add listeners for save/load buttons
        if (clearSaveButton != null) clearSaveButton.onClick.AddListener(ClearSaveData);

        // Subscribe to the OnDataLoaded event to update button states
        SaveLoadManager.OnDataLoaded += UpdateMainMenuButtons;
        
        // Run initial check (in case SaveLoadManager loaded before this)
        UpdateMainMenuButtons(); 
        loadButton.onClick.AddListener(SaveLoadManager.Instance.LoadGame);
        saveButton.onClick.AddListener(SaveLoadManager.Instance.SaveGame);
    }

    public void NewGame()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        levelMenu.SetActive(false);
        if(gameMenu.activeSelf) gameMenu.SetActive(false);
    }

    public void ProceedToGame()
    {
        if(levelMenu.activeSelf) levelMenu.SetActive(false);
        gameMenu.SetActive(true);
    }

    public void BackToLevelMenu()
    {
        gameMenu.SetActive(false);
        levelMenu.SetActive(true);
        CardManager.Instance.DestroyCards();
    }

    public void LoadButtonClicked()
    {
        mainMenu.SetActive(false);
        levelMenu.SetActive(true);
        if(gameMenu.activeSelf) gameMenu.SetActive(false);
    }
    
    private void UpdateMainMenuButtons()
    {
        if (SaveLoadManager.Instance == null) return;

        bool saveFileExists = SaveLoadManager.Instance.DoesSaveFileExist();
        
        // The "Load" button might just be text, but we can disable it
        if (loadButton != null) 
        {
            loadButton.interactable = saveFileExists;
        }
        
        if (clearSaveButton != null)
        {
            clearSaveButton.interactable = saveFileExists;
        }
    }
    
    private void ClearSaveData()
    {
        Debug.Log("Clear Save button clicked.");
        SaveLoadManager.Instance.ClearSaveData();
    }
    
    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SaveLoadManager.OnDataLoaded -= UpdateMainMenuButtons;
    }
}
