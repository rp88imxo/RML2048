using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class TotalMovesPanelManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI totalMovesText;

    // Start is called before the first frame update
    void Start()
    {
        totalMovesText.text = "0";
        GameManager.Instance.onTotalMovesChanged += OnTotalMovesChanged;
    }

    private void OnTotalMovesChanged(int totalMoves)
    {
        totalMovesText.text = totalMoves.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
