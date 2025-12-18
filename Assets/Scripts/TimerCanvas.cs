using UnityEngine;
using TMPro;

public class TimerCanvas : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 60f;   // ��ü �ð� (��)
    public bool autoStart = true;   // ���� �� �ڵ� ����

    [Header("UI")]
    public TextMeshProUGUI timerText; // Canvas�� �ִ� �ؽ�Ʈ

    [Header("Display")]
    public bool showMinutes = true; // mm:ss ���� ����

    public float currentTime;
    private bool isRunning = false;

    public GameReport gameReport; // 결과 매니저

    void Start()
    {
        if (gameReport == null)
        {
            gameReport = FindObjectOfType<GameReport>();
        }

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
            if (gameReport != null)
            {
                gameReport.GameOver();
            }
        }

        UpdateText();
    }

    // ===============================
    // �ܺ� ����� �Լ�
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
    // �ؽ�Ʈ ������Ʈ
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

