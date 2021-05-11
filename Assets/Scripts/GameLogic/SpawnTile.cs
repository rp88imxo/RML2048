
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Linq;

public class SpawnTile : MonoBehaviour
{
    [SerializeField]
    RectTransform gridLayoutPanelForTiles;

    [SerializeField]
    TileNumberPool tileNumberPool;

    [SerializeField]
    RectTransform parentForTileNumber;

    [SerializeField]
    ColorTilePalette colorTilePalette;

    GameTile[] tiles;
    GameTile[] tilesPrevious;

    [SerializeField, Range(0f, 1f)]
    float chanceToSpawnNumber4 = 0.4f;

    [SerializeField]
    int xTotalTiles, yTotalTiles;

    int totalTilesWithnumber;

    bool hasAnyMove;



    void Start()
    {
        GameManager.Instance.onMainMenu += OnSaveGameTiles;

    }

    private void OnSaveGameTiles()
    {
        if (totalTilesWithnumber == tiles.Length)
        {
            hasAnyMove = HasAnyMoves();
        }
        else
        {
            hasAnyMove = true;
        }

        if (hasAnyMove)
        {
            TileData.SaveTileData(tiles);
        }
        GameDataPersistentManager.SetPreviousState(hasAnyMove);
        GameDataPersistentManager.SaveGameData();
    }

    private void OnApplicationQuit()
    {
        OnSaveGameTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        if (tiles == null)
        {
            tiles = gridLayoutPanelForTiles
                .GetComponentsInChildren<GameTile>();
        }

        System.Array.ForEach(tiles, x => x.ResetTileNumberAndGameTile());
        totalTilesWithnumber = 0;
    }

   public void BuildTiles()
    {

        if (tiles == null)
        {
            tiles = gridLayoutPanelForTiles
                .GetComponentsInChildren<GameTile>();
        }

        System.Array.ForEach(tiles, x => x.Init());


        for (int y = 0, i = 0; y < yTotalTiles; y++)
        {
            for (int x = 0; x < xTotalTiles; x++, i++)
            {
                if (x + 1 != xTotalTiles)
                {
                    tiles[i].SetRightLeft(tiles[i + 1]);
                }
                else
                {
                    tiles[i].SetRightLeft(null);
                }

                if (y + 1 != yTotalTiles)
                {
                    tiles[i].SetBottomTop(tiles[i + yTotalTiles]);
                }
                else
                {
                    tiles[i].SetBottomTop(null);
                }

            }
        }

    }

    public void LoadPreviousState()
    {
        TileData.LoadTileData(tiles, tileNumberPool, parentForTileNumber, colorTilePalette);
    }

    public void SpawnNumber()
    {

        if (totalTilesWithnumber == tiles.Length)
        {
            return;
        }

        float randProc = Random.Range(0f, 1f);
        int posIndex = Random.Range(0, tiles.Length);
        int numberInTile;

        while (tiles[posIndex].TileState != TileState.Empty)
        {
            posIndex = Random.Range(0, tiles.Length);
        }

        TileNumber go;

        if (randProc <= chanceToSpawnNumber4)
        {
            Debug.Log("Number 4");
            numberInTile = 4;

        }
        else
        {
            Debug.Log("Number 2");
            numberInTile = 2;

        }

        go = tileNumberPool.GetRandom();
        go.transform.SetParent(parentForTileNumber);
        // Instantiate(tileNumberPrefab, parentForTileNumber);

        tiles[posIndex].TileState = TileState.Filled;
        tiles[posIndex].SetTileNumber(go, numberInTile, colorTilePalette);

        totalTilesWithnumber++;


    }


    int RecalculateTotalNumberTiles()
    {
        int t = 0;

        for (int i = 0; i < tiles.Length; i++)
        {
            
            if (tiles[i].TileState == TileState.Filled)
            {
                t++;
            }
        }

        return t;
    }

    public bool MoveTiles(KeyCode moveKey)
    {
        bool hasMoved = false;
        hasAnyMove = true;

        switch (moveKey)
        {
            case KeyCode.W:
                hasMoved = MoveUpDown(true);
                break;
            case KeyCode.S:
                hasMoved = MoveUpDown(false);
                break;
            case KeyCode.A:
                hasMoved = MoveLeftRight(true);
                break;
            case KeyCode.D:
                hasMoved = MoveLeftRight(false);
                break;
            default:
                break;
        }


        totalTilesWithnumber = RecalculateTotalNumberTiles();

        if (hasMoved)
        {
            SpawnNumber();
            GameManager.Instance.OnMoveCountChanged();
        }

        if (totalTilesWithnumber == tiles.Length)
        {
            hasAnyMove = HasAnyMoves();
        }

        return hasAnyMove;
    }


    bool HasAnyMoves()
    {
        bool hasMove = false;

        for (int y = 0; y < yTotalTiles; y++)
        {
            for (int x = 0; x < xTotalTiles; x++)
            {
                hasMove |= tiles[y * xTotalTiles + x].CheckForAvailableMove();
            }
        }

        return hasMove;
    }


    /// <summary>
    /// Move all columns up or down, if we pressed W so we need to start moving from top, and vice versa
    /// </summary>
    /// <param name="startFromTop">Should we move array from top?</param>
    bool MoveUpDown(bool startFromTop)
    {
        int columnCount = xTotalTiles;
        bool hasMoved = false;

        if (startFromTop)
        {
            for (int x = 0; x < columnCount; x++)
            {
                for (int y = 0; y < yTotalTiles; y++)
                {
                    if (tiles[y * columnCount + x].TileState == TileState.Filled)
                    {
                        hasMoved |= tiles[y * columnCount + x].MoveUpDown(startFromTop);
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < columnCount; x++)
            {
                for (int y = yTotalTiles - 1; y >= 0; y--)
                {
                    if (tiles[y * columnCount + x].TileState == TileState.Filled)
                    {
                        hasMoved |= tiles[y * columnCount + x].MoveUpDown(startFromTop);
                    }
                }
            }
        }

        return hasMoved;
    }

    bool MoveLeftRight(bool startFromLeft)
    {
        int rowCount = yTotalTiles;
        bool hasMoved = false;

        if (startFromLeft)
        {
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < xTotalTiles; x++)
                {
                    if (tiles[y * xTotalTiles + x].TileState == TileState.Filled)
                    {
                        hasMoved |= tiles[y * xTotalTiles + x].MoveLeftRight(startFromLeft);
                    }
                }
            }
        }
        else
        {
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = xTotalTiles - 1; x >= 0; x--)
                {
                    if (tiles[y * xTotalTiles + x].TileState == TileState.Filled)
                    {
                       hasMoved |= tiles[y * xTotalTiles + x].MoveLeftRight(startFromLeft);
                    }
                }
            }
        }

        return hasMoved;
    }

}
