using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameReport : MonoBehaviour
{
    public bool isGameEnded = false; //중복으로 호출되면.. 안되니깐
    public GameObject winUIPanel;  // 승리했을 때
    public GameObject loseUIPanel; // 패배

    [Header("Result Texts")]
    public TextMeshProUGUI hpText;      // 남은 타워 체력
    public TextMeshProUGUI killText;    // 물리친 적의 수
    public TextMeshProUGUI timeText;    // 남은 시간
    public TextMeshProUGUI scoreText;   // 점수
    public GameObject hpUi; //체력 UI

    private Tower towerScript;
    private WaveManager waveScript;
    private TimerCanvas timerScript;
    private Score_add scoreScript;

    // Start is called before the first frame update
    void Start()
    {
        //켜져있으면 안되니까
        if (winUIPanel != null) winUIPanel.SetActive(false);
        if (loseUIPanel != null) loseUIPanel.SetActive(false);
        if (hpUi != null) hpUi.SetActive(true);

        towerScript = FindObjectOfType<Tower>();
        waveScript = FindObjectOfType<WaveManager>();
        timerScript = FindObjectOfType<TimerCanvas>();
        scoreScript = FindObjectOfType<Score_add>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateResultUI()
    {
        // 타워 체력 표시
        if (hpText != null && towerScript != null)
        {
            // 타워가 파괴되어 null일 경우 0으로 표시
            int currentHP = (towerScript != null) ? towerScript.hp : 0;
            // 음수가 나오지 않게 처리
            if (currentHP < 0) currentHP = 0; 
            
            hpText.text = $"{currentHP}";
        }

        // 물리친 적 수 표시
        if (killText != null && waveScript != null)
        {
            killText.text = $"{waveScript.totalKilled}마리";
        }

        //남은 시간 표시
        if (timeText != null && timerScript != null)
        {
            float t = timerScript.currentTime;
            int min = Mathf.FloorToInt(t / 60f);
            int sec = Mathf.FloorToInt(t % 60f);
            timeText.text = $"{min:00}:{sec:00}";
        }

        //점수 표시
        if (scoreText != null && scoreScript != null)
        {
            scoreText.text = $"{scoreScript.totalScore}점";
        }
    }

    public void GameWin()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        UpdateResultUI(); //텍스트 갱신

        if (winUIPanel != null)
        {
            winUIPanel.SetActive(true);
        }

        hpUi.SetActive(false);
    }

    public void GameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        UpdateResultUI(); //텍스트 갱신

        if (loseUIPanel != null)
        {
            loseUIPanel.SetActive(true);
        }

        hpUi.SetActive(false);
    }
}
