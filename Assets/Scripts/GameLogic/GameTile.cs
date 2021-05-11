using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{

    public GameTile right, left, bottom, top;

    TileState tileState;

    TileNumber tileNumber;

    public int Number 
    { 
        get 
        {
            if (tileNumber)
            {
                return tileNumber.Number;
            }
            return -1;
        } 
    } 

    public TileState TileState
    {
        get
        {
            return tileState;
        }
        set
        {
            tileState = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init()
    {
        tileState = TileState.Empty;
    }



    public void ResetTileNumberAndGameTile()
    {
        if (tileNumber)
        {
            tileNumber.ResetState();
            tileNumber = null;
        }

        tileState = TileState.Empty;
    }


    public void DestroyTileNumber()
    {
        if (tileNumber)
        {
            tileNumber.RecycleSelf();
            tileNumber = null;
        }
        
        tileState = TileState.Empty;
    }

    public void SetTileNumber(TileNumber tileNum, int numberInTile, ColorTilePalette tilePalette)
    {
        tileNumber = tileNum;
        tileNumber.Init(numberInTile, this, tilePalette);

        //Debug.Log(transform.position);

       // tileNumber.transform.position = transform.position;
        //tileNumber.transform.rotation = Quaternion.identity;
       // tileNumber.transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(transform.position);
        }
    }

    public void SetRightLeft(GameTile right)
    {
        this.right = right;
        if (right != null)
        {
            right.left = this;
        }
       
    }

    public void SetBottomTop(GameTile bottom)
    {
        this.bottom = bottom;

        if (bottom != null)
        {
            bottom.top = this;
        }
        
    }

    public bool MoveLeftRight(bool isLeft)
    {
        if (isLeft)
        {
            GameTile tempTile = this;

            while (tempTile.left != null)
            {
                tempTile = tempTile.left;

                if (tempTile.TileState == TileState.Filled)
                {
                    if (tempTile.tileNumber.Number == tileNumber.Number)
                    {
                        tempTile.tileNumber.AddNewNumber(tileNumber.Number);
                        
                        GameManager.Instance.OnScoreChanged(tempTile.tileNumber.Number);
                        DestroyTileNumber();

                        return true;
                    }
                    else
                    {
                        GameTile rightTempTile = tempTile.right;
                        if (rightTempTile != this)
                        {
                            rightTempTile.tileNumber = tileNumber;
                            tileNumber = null;
                            rightTempTile.tileNumber.UpdateNewPosition(rightTempTile);

                            rightTempTile.TileState = TileState.Filled;
                            tileState = TileState.Empty;

                            return true;
                        }

                        return false;
                    }
                }

                else
                {
                    if (tempTile.left == null)
                    {
                        tempTile.tileNumber = tileNumber;
                        tileNumber = null;
                        tempTile.tileNumber.UpdateNewPosition(tempTile);

                        tempTile.TileState = TileState.Filled;
                        tileState = TileState.Empty;
                        return true;
                    }
                }
            }
        }
        else
        {
            GameTile tempTile = this;

            while (tempTile.right != null)
            {
                tempTile = tempTile.right;

                if (tempTile.TileState == TileState.Filled)
                {
                    if (tempTile.tileNumber.Number == tileNumber.Number)
                    {
                        tempTile.tileNumber.AddNewNumber(tileNumber.Number);

                        GameManager.Instance.OnScoreChanged(tempTile.tileNumber.Number);

                        DestroyTileNumber();
                        return true;

                    }
                    else
                    {
                        GameTile leftTempTile = tempTile.left;
                        if (leftTempTile != this)
                        {
                            leftTempTile.tileNumber = tileNumber;
                            tileNumber = null;
                            leftTempTile.tileNumber.UpdateNewPosition(leftTempTile);

                            leftTempTile.TileState = TileState.Filled;
                            tileState = TileState.Empty;

                            return true;
                        }

                        return false;
                    }
                }
                else
                {
                    if (tempTile.right == null)
                    {
                        tempTile.tileNumber = tileNumber;
                        tileNumber = null;
                        tempTile.tileNumber.UpdateNewPosition(tempTile);

                        tempTile.TileState = TileState.Filled;
                        tileState = TileState.Empty;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool CheckForAvailableMove()
    {
        bool hasAnyMoves = false;


        if (left)
        {
            hasAnyMoves |= left.tileNumber.Number == tileNumber.Number;
        }
        if (top)
        {
            hasAnyMoves |= top.tileNumber.Number == tileNumber.Number;
        }
        if (right)
        {
            hasAnyMoves |= right.tileNumber.Number == tileNumber.Number;
        }
        if (bottom)
        {
            hasAnyMoves |= bottom.tileNumber.Number == tileNumber.Number;
        }

        return hasAnyMoves;
    }

    public bool MoveUpDown(bool isUp)
    {
        if (isUp)
        {
            GameTile tempTile = this;
            while (tempTile.top != null)
            {
                tempTile = tempTile.top;
                if (tempTile.TileState == TileState.Filled)
                {
                    if (tempTile.tileNumber.Number == tileNumber.Number)
                    {
                        tempTile.tileNumber.AddNewNumber(tileNumber.Number);

                        GameManager.Instance.OnScoreChanged(tempTile.tileNumber.Number);

                        DestroyTileNumber();
                        return true;
                    }
                    else
                    {
                        GameTile bottomTempTile = tempTile.bottom;
                        if (bottomTempTile != this)
                        {
                            bottomTempTile.tileNumber = tileNumber;
                            tileNumber = null;
                            bottomTempTile.tileNumber.UpdateNewPosition(bottomTempTile);

                            bottomTempTile.tileState = TileState.Filled;
                            tileState = TileState.Empty;

                            return true;
                        }

                        return false;
                    }
                }
                else
                {
                    if (tempTile.top == null)
                    {
                        tempTile.tileNumber = tileNumber;
                        tileNumber = null;
                        tempTile.tileNumber.UpdateNewPosition(tempTile);

                        tempTile.tileState = TileState.Filled;
                        tileState = TileState.Empty;

                        return true;
                    }
                }
            }
            
        }
        else
        {
            GameTile tempTile = this;
            while (tempTile.bottom != null)
            {
                tempTile = tempTile.bottom;
                if (tempTile.TileState == TileState.Filled)
                {
                    if (tempTile.tileNumber.Number == tileNumber.Number)
                    {
                        tempTile.tileNumber.AddNewNumber(tileNumber.Number);
                        
                        GameManager.Instance.OnScoreChanged(tempTile.tileNumber.Number);

                        DestroyTileNumber();
                        return true;
                    }
                    else
                    {
                        GameTile topTempTile = tempTile.top;
                        if (topTempTile != this)
                        {
                            topTempTile.tileNumber = tileNumber;
                            tileNumber = null;
                            topTempTile.tileNumber.UpdateNewPosition(topTempTile);

                            topTempTile.TileState = TileState.Filled;
                            tileState = TileState.Empty;

                            return true;
                        }
                        return false;
                    }
                }
                else
                {
                    if (tempTile.bottom == null)
                    {
                        tempTile.tileNumber = tileNumber;
                        tileNumber = null;
                        tempTile.tileNumber.UpdateNewPosition(tempTile);

                        tempTile.TileState = TileState.Filled;
                        tileState = TileState.Empty;
                        return true;
                    }
                }
            }
        }

        return false;
    }



}
