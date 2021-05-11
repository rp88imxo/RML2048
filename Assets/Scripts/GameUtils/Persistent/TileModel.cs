using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileModel
{

    public TileState tileState;
    public int num;
}

[System.Serializable]
public class TileModelWrapper
{
    public TileModel[] tileModels;

    public void Init(GameTile[] gameTiles)
    {
        int totalTiles = gameTiles.Length;
        tileModels = new TileModel[totalTiles];

        for (int i = 0; i < totalTiles; i++)
        {
            tileModels[i] = new TileModel();
            tileModels[i].num = gameTiles[i].Number;
            tileModels[i].tileState = gameTiles[i].TileState;
        }
    }


}