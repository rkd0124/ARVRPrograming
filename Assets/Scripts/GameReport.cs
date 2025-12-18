using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameReport : MonoBehaviour
{
    public bool isGameEnded = false; //중복으로 호출되면.. 안되니깐
    public GameObject winUIPanel;  // 승리했을 때
    public GameObject loseUIPanel; // 패배
    // Start is called before the first frame update
    void Start()
    {
        //켜져있으면 안되니까
        if (winUIPanel != null) winUIPanel.SetActive(false);
        if (loseUIPanel != null) loseUIPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameWin()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        if (winUIPanel != null)
        {
            winUIPanel.SetActive(true);
        }
    }

    public void GameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("💀 GAME OVER 💀");
        if (loseUIPanel != null)
        {
            loseUIPanel.SetActive(true);
        }
    }
}
