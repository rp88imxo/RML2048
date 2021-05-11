using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonMainMenuClicked()
    {
        // TODO: Add load main menu
        GameManager.Instance.OnMainMenu();
    }
    public void OnButtonRestartClicked()
    {
        GameManager.Instance.OnGameRestarted();
    }


}
