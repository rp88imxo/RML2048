using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;


public class GameEndManager : MonoBehaviour
{
    [SerializeField]
    ModalWindowManager modalWindow;


    private void Awake()
    {
       
    }

    void Start()
    {
        GameManager.Instance.onGameEnd += OnGameEnd;
    }

    private void OnGameEnd()
    {
        modalWindow.OpenWindow();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRestartBtnClicked()
    {
        GameManager.Instance.OnGameRestarted();
    }

    public void OnOkBtnClicked()
    {
        modalWindow.CloseWindow();
    }
#if UNITY_EDITOR

    [ContextMenu("Show Modal")]
    void DebugShow()
    {
        modalWindow.OpenWindow();
    }
#endif
}
