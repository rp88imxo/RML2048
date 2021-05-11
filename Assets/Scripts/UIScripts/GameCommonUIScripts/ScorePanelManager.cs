using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class ScorePanelManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreNumberText;


    private void Start()
    {
        scoreNumberText.text = "0";
        GameManager.Instance.onScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        scoreNumberText.text = score.ToString();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
