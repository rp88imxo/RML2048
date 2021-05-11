using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataPersistentManager
{
    static GameDataModel gameDataModel;

    public static bool HasPreviosState => gameDataModel.hasPreviousState;
    public static int BestScore => gameDataModel.bestScore;
    public static int SavedPaletteIndex => gameDataModel.savedPalette;

    public static void CopyGameDataModel(GameDataModel gameDataMod)
    {
        if (gameDataModel != null)
        {
            gameDataMod.bestScore = gameDataModel.bestScore;
            gameDataMod.savedPalette = gameDataModel.savedPalette;
            gameDataMod.hasPreviousState = gameDataModel.hasPreviousState;
        }
    }

    static GameDataPersistentManager()
    {
        LoadGameData();
    }
    
    public static void LoadGameData(string path = "GameData")
    {
        gameDataModel = DataSaver.loadData<GameDataModel>(path);
        if (gameDataModel == null)
        {
            gameDataModel = new GameDataModel();
        }
    }

    public static void SaveGameData(string path = "GameData")
    {
        if (gameDataModel != null)
        {
            DataSaver.saveData(gameDataModel, path);
        }
    }

    public static void SetNewBestScore(int bestScore)
    {
        gameDataModel.bestScore = bestScore;
    }

    public static void SetNewPalette(int paletteIndex)
    {
        gameDataModel.savedPalette = paletteIndex;
    }

    public static void SetPreviousState(bool hasPrevState)
    {
        gameDataModel.hasPreviousState = hasPrevState;
    }


}
