using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameDataModel
{
    public bool hasPreviousState;
    public int bestScore;
    public int savedPalette;

    public GameDataModel()
    {
        hasPreviousState = false;
        bestScore = 0;
        savedPalette = -1;
    }
}
