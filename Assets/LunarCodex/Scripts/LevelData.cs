using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData/LevelDataScriptableObject", order = 1)]
public class LevelData : ScriptableObject
{
    public LevelDataItem[] levelDataItems;
}

[Serializable]
public class LevelDataItem
{
    public int id;
    public string levelName;
    public int rows;
    public int columns;
    public Color normalColor;
}