using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    SpawnTile spawnTile;

    [SerializeField, Range(0f, 1f)]
    float timeBetweenMoves = 0.1f;

    [SerializeField]
    bool loadPreviousState = true;

    bool hasAnyMove;
    bool isMoveOnCooldown;

    [SerializeField]
    SwipeControl swipeControl;

    float currentMoveTimer;
    bool hasPreviousState;

    #region USEFULL_VARIABLES
    int totalScore;
    int totalMoves;
    #endregion

    #region EVENTS
    public event Action<int> onScoreChanged;
    public event Action<int> onTotalMovesChanged;
    public event Action onGameEnd;
    public event Action onGameRestarted;
    public event Action onMainMenu;
    #endregion


    protected override void Awake()
    {
        base.Awake();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        hasPreviousState = GameDataPersistentManager.HasPreviosState;
        StartCoroutine(InitGame());
    }

  

    void RestartGame()
    {

        hasAnyMove = true;
        totalMoves = totalScore = 0;

        spawnTile.RestartGame();

        for (int i = 0; i < 2; i++)
        {
            spawnTile.SpawnNumber();
        }
    }

    IEnumerator InitGame()
    {
        hasAnyMove = true;
        spawnTile.BuildTiles();

        yield return new WaitForEndOfFrame();

        if (loadPreviousState && hasPreviousState)
        {
            spawnTile.LoadPreviousState();
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                spawnTile.SpawnNumber();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveOnCooldown)
        {
            currentMoveTimer += Time.deltaTime;
            if (currentMoveTimer > timeBetweenMoves)
            {
                currentMoveTimer = 0f;
                isMoveOnCooldown = false;
            }
        }

        if (hasAnyMove && !isMoveOnCooldown)
        {
            // TODO: Заменить контрактом вместо директив условной компиляции чтобы не нарушать SOLID
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.W))
            {
                hasAnyMove = spawnTile.MoveTiles(KeyCode.W);
                isMoveOnCooldown = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                hasAnyMove = spawnTile.MoveTiles(KeyCode.S);
                isMoveOnCooldown = true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                hasAnyMove = spawnTile.MoveTiles(KeyCode.A);
                isMoveOnCooldown = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                hasAnyMove = spawnTile.MoveTiles(KeyCode.D);
                isMoveOnCooldown = true;
            }
#endif


#if PLATFORM_ANDROID
            if (swipeControl.SwipeLeft)
            {
                hasAnyMove = spawnTile.MoveTiles(KeyCode.A);
                isMoveOnCooldown = true;
            }
            else if (swipeControl.SwipeRight)
            {
                hasAnyMove = spawnTile.MoveTiles(KeyCode.D);
                isMoveOnCooldown = true;
            }
            else if (swipeControl.SwipeUp)
            {
                hasAnyMove = spawnTile.MoveTiles(KeyCode.W);
                isMoveOnCooldown = true;
            }
            else if (swipeControl.SwipeDown)
            {
                hasAnyMove = spawnTile.MoveTiles(KeyCode.S);
                isMoveOnCooldown = true;
            }
#endif

            if (!hasAnyMove)
            {
                Debug.Log("Game is End");
                onGameEnd?.Invoke();

            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

    }

    #region GLOBAL_EVENTS
    public void OnScoreChanged(int score)
    {
        totalScore += score;
        onScoreChanged?.Invoke(totalScore);
    }

    public void OnMoveCountChanged()
    {
        totalMoves++;
        onTotalMovesChanged?.Invoke(totalMoves);
    }

    public void OnGameRestarted()
    {
        onGameRestarted?.Invoke();
        RestartGame();
    }

    public void OnMainMenu()
    {
        onMainMenu?.Invoke();
        Loader.LoadSceneAsync(SceneName.MainMenuScene);
    }

    #endregion

}
