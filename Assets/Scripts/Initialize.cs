using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;

    private void Awake()
    {
        if(mainMenu != null) mainMenu.SetActive(true); //Make sure to check the reference is assigned and turn on the main menu. 
        if(levelMenu.activeSelf) levelMenu.SetActive(false); //turn off other menu items when initialize.
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
    }
}
