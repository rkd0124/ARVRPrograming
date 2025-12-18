using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceItemManager : MonoBehaviour
{
    [Header("Gauge")]
    public float requiredGauge = 100f;   // 최대 게이지
    public float currentGauge = 100f;    // 현재 게이지

    [Header("Ice Effect")]
    public float slowDuration = 5f;
    public float slowPercent = 0.5f;

    bool isReady = true;

    [Header("Gauge UI")]
    public RectTransform gaugeBar;       // ⭐ 게이지 이미지
    public float maxWidth = 340f;         // 게이지 가득 찼을 때
    public float minWidth = 0f;           // 게이지 0일 때
    public float smoothSpeed = 10f;       // ⭐ 애니메이션 속도

    Coroutine gaugeAnimCoroutine;

    void Start()
    {
        currentGauge = Mathf.Clamp(currentGauge, 0, requiredGauge);
        UpdateGaugeImmediate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) ||
            ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            ActivateIceItem();
        }
    }

    // ===============================
    // 게이지 증가 (적 처치 시)
    // ===============================
    public void AddGauge(float amount)
    {
        currentGauge += amount;
        currentGauge = Mathf.Clamp(currentGauge, 0f, requiredGauge);

        if (currentGauge >= requiredGauge)
            isReady = true;

        AnimateGauge();
    }

    // ===============================
    // 아이템 발동
    // ===============================
    public void ActivateIceItem()
    {
        if (!isReady) return;

        StartCoroutine(SlowAllEnemies());

        isReady = false;
        currentGauge = 0f;

        AnimateGauge();
    }

    // ===============================
    // 필드 전체 적 슬로우
    // ===============================
    IEnumerator SlowAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<IEnemy> targets = new List<IEnemy>();

        foreach (GameObject obj in enemies)
        {
            IEnemy enemy = obj.GetComponent<IEnemy>();
            if (enemy != null)
            {
                enemy.ApplySlow(slowPercent);
                targets.Add(enemy);
            }
        }

        yield return new WaitForSeconds(slowDuration);

        foreach (IEnemy enemy in targets)
        {
            if (enemy != null)
                enemy.RemoveSlow();
        }
    }

    // ===============================
    // 웨이브 종료 시 완충
    // ===============================
    public void FullCharge()
    {
        currentGauge = requiredGauge;
        isReady = true;
        AnimateGauge();
    }

    // ===============================
    // UI 처리
    // ===============================
    void AnimateGauge()
    {
        if (gaugeBar == null) return;

        if (gaugeAnimCoroutine != null)
            StopCoroutine(gaugeAnimCoroutine);

        gaugeAnimCoroutine = StartCoroutine(SmoothGauge());
    }

    IEnumerator SmoothGauge()
    {
        float ratio = currentGauge / requiredGauge;
        float targetWidth = Mathf.Lerp(minWidth, maxWidth, ratio);
        float currentWidth = gaugeBar.sizeDelta.x;

        while (Mathf.Abs(currentWidth - targetWidth) > 0.1f)
        {
            currentWidth = Mathf.Lerp(
                currentWidth,
                targetWidth,
                Time.deltaTime * smoothSpeed
            );

            Vector2 size = gaugeBar.sizeDelta;
            size.x = currentWidth;
            gaugeBar.sizeDelta = size;

            yield return null;
        }

        Vector2 finalSize = gaugeBar.sizeDelta;
        finalSize.x = targetWidth;
        gaugeBar.sizeDelta = finalSize;
    }

    void UpdateGaugeImmediate()
    {
        if (gaugeBar == null) return;

        float ratio = currentGauge / requiredGauge;
        float width = Mathf.Lerp(minWidth, maxWidth, ratio);

        Vector2 size = gaugeBar.sizeDelta;
        size.x = width;
        gaugeBar.sizeDelta = size;
    }
}
