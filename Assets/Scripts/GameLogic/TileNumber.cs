using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class TileNumber : MonoBehaviour, IPoolable
{
    TileNumberPool originPool;

    public ObjectPool OriginPool
    {
        get => originPool;
        set
        {
            if (originPool == null)
            {
                originPool = value as TileNumberPool;
            }
            else
            {
                Debug.LogError("Attempt to redefine the origin pool of the object");
            }
        }
    }


    [SerializeField]
    TextMeshProUGUI textNumber;

    [SerializeField, Range(0.1f, 20f)]
    float animScaleSpeed = 20f;

    [SerializeField]
    Vector3 scaleAnim = new Vector3(1.25f, 1.25f, 1f);

    Action OnNumberChanged;

    int number;
    //GameTile parrentTile;

    public int Number => number;

    Image image;

    static ColorTilePalette tilePalette;

    Vector3 defaultScale = Vector3.one;
    Vector3 startInitScale = new Vector3(0.1f, 0.1f, 1f);
    Vector3 newPos, oldPos;

    bool isScaleAnimOnNewNumber, isMoveAnim, isInitScaleAnim;
    
    float scaleAnimTimer, scaleAnimTimerBack, moveAnimTimer, scaleInitAnimTimer;

    int NumberWithEvent
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            OnNumberChanged.Invoke();
        }
    }

    public void AddNewNumber(int num)
    {
        Debug.Assert(num == number, "Numbers you are trying to add is different!");

        isScaleAnimOnNewNumber = true;


        NumberWithEvent += num;
    }

    

    public void Init(int num, GameTile parrentTile, ColorTilePalette colorTilePalette)
    {
        if (tilePalette == null)
        {
            tilePalette = colorTilePalette;
        }
        OnNumberChanged += OnNumberHasChanged;
       
        NumberWithEvent = num;
        
        transform.position = parrentTile.transform.position;
        transform.localScale = startInitScale;

        isInitScaleAnim = true;
    }

    internal void RecycleSelf()
    {
        OnNumberChanged -= OnNumberHasChanged;
        Recycle();
    }

    public void ResetStateNoDestroy()
    {
        OnNumberChanged -= OnNumberHasChanged;

        isScaleAnimOnNewNumber = isMoveAnim = isInitScaleAnim = default;
        scaleAnimTimer = scaleAnimTimerBack = moveAnimTimer = scaleInitAnimTimer = default;
    }

    public void ResetState()
    {
        OnNumberChanged -= OnNumberHasChanged;

        tilePalette = null;
        Recycle();
    }

    void OnNumberHasChanged()
    {
        textNumber.text = number.ToString();

        image.color = tilePalette.GetColor(number);

    }

    // Start is called before the first frame update
    void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isScaleAnimOnNewNumber)
        {
            PlayScaleAnim();
        }

        if (isMoveAnim)
        {
            PlayMoveAnim();
        }

        if (isInitScaleAnim)
        {
            PlayInitScale();
        }
    }

    private void PlayInitScale()
    {
        if (scaleInitAnimTimer < 1f)
        {
            scaleInitAnimTimer += Time.deltaTime * 10;
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, scaleInitAnimTimer);
        }
        else
        {
            scaleInitAnimTimer = 0f;
            isInitScaleAnim = false;
        }
    }

    private void PlayMoveAnim()
    {
        if (moveAnimTimer <= 1f)
        {
            moveAnimTimer += Time.deltaTime * 15f; // TODO: Add 1 / timeBetweenMoves variable from GameManager to calc time
            transform.position = Vector3.Lerp(oldPos, newPos, moveAnimTimer);
        }
        else
        {
            moveAnimTimer = 0f;
            isMoveAnim = false;
        }
    }

    internal void UpdateNewPosition(GameTile newTile)
    {
        newPos = newTile.transform.position;
        oldPos = transform.position;
        isMoveAnim = true;

    }

    void PlayScaleAnim()
    {
        if (scaleAnimTimer < 1f)
        {
            scaleAnimTimer += Time.deltaTime * animScaleSpeed;
            transform.localScale = Vector3.Lerp(transform.localScale, scaleAnim, scaleAnimTimer);


        }
        else
        {
            scaleAnimTimerBack += Time.deltaTime * animScaleSpeed;

            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, scaleAnimTimerBack);


            if (scaleAnimTimerBack > 1f)
            {
                scaleAnimTimerBack = 0f;
                scaleAnimTimer = 0f;

                isScaleAnimOnNewNumber = false;
            }


        }
    }

    public void Recycle()
    {
        originPool.Reclaim(this);
    }
}
