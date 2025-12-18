using UnityEngine;
using TMPro;

public class TimerCanvas : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 60f;   // 전체 시간 (초)
    public bool autoStart = true;   // 시작 시 자동 실행

    [Header("UI")]
    public TextMeshProUGUI timerText; // Canvas에 있는 텍스트

    [Header("Display")]
    public bool showMinutes = true; // mm:ss 형식 여부

    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        currentTime = totalTime;

        if (autoStart)
            StartTimer();

        UpdateText();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
        }

        UpdateText();
    }

    // ===============================
    // 외부 제어용 함수
    // ===============================
    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = totalTime;
        UpdateText();
    }

    // ===============================
    // 텍스트 업데이트
    // ===============================
    void UpdateText()
    {
        if (timerText == null) return;

        if (showMinutes)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }
}

