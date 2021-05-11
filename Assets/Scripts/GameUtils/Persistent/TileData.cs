using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public static void SaveTileData(GameTile[] gameTiles, string fileName = "tileDataPersistent")
    {
        TileModelWrapper tileModelWrapper = new TileModelWrapper();
        tileModelWrapper.Init(gameTiles);

        DataSaver.saveData(tileModelWrapper, fileName);

    }

    public static bool LoadTileData(
        GameTile[] gameTiles, 
        TileNumberPool tileNumberPool,
        RectTransform parrentForNumberTile,
        ColorTilePalette colorTilePalette,
        string fileName = "tileDataPersistent")
    {

        TileModelWrapper tileModelWrapper = DataSaver.loadData<TileModelWrapper>(fileName);
       
        TileModel[] tileModels = tileModelWrapper.tileModels;
        
        if (tileModels == null)
        {
            return false;
        }


        if (tileModels.Length != gameTiles.Length)
        {
            return false;
        }

        for (int i = 0; i < tileModels.Length; i++)
        {
            gameTiles[i].TileState = tileModels[i].tileState;

            if (tileModels[i].num != -1)
            {
                var go = tileNumberPool.GetRandom();
                go.transform.SetParent(parrentForNumberTile);
                gameTiles[i].SetTileNumber(go,tileModels[i].num, colorTilePalette);
            }
        }

        return true;
    }
}
