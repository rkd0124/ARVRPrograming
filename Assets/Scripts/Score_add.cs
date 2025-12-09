using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_add : MonoBehaviour
{
    public int totalScore;

    [Header("Time Bonus Settings")]
    public float maxWaveBonus = 1000f; // 웨이브당 최대 보너스 점수
    public float scoreDecayRate = 100f; // 1초당 깎이는 점수 -> 웨이브 시작되자마자 클리어되면 1000점
    
    private float waveStartTime; // 웨이브 시작 시간
    private bool isTimerRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Score_plus(int plusScore){
        totalScore += plusScore;
        Debug.LogWarning("현재 점수: "+totalScore);
    }

    public void StartWaveTimer() //타이머 시작
    {
        waveStartTime = Time.time;
        isTimerRunning = true;
        Debug.Log("시간 보너스 카운트 시작!");
    }

    public void CalculateTimeBonus()
    {
        if (!isTimerRunning) return;

        // 걸린 시간 계산
        float duration = Time.time - waveStartTime;

        // 보너스 계산 공식: 최대 점수 - (걸린 시간 * 1초당 감점)
        float bonus = maxWaveBonus - (duration * scoreDecayRate);

        // 점수가 음수가 되지 않도록 최소 0점으로 제한 (Mathf.Max)
        int finalBonus = Mathf.RoundToInt(Mathf.Max(0, bonus));

        totalScore += finalBonus; // 총점에 합산
        
        Debug.LogWarning($"[웨이브 클리어] 소요 시간: {duration:F2}초 / 시간 보너스: +{finalBonus}점 / 총점: {totalScore}");

        isTimerRunning = false; // 타이머 종료
    }
}
