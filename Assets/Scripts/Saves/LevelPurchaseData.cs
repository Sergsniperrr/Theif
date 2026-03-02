using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelPurchaseData
{
    public bool[] Levels;
    
    public LevelPurchaseData(bool[] levels)
    {
        Levels = levels;
    }
}
