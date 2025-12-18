using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 100;
    public int hp = 100;

    [Header("HP Bar UI")]
    public RectTransform hpBar;   // 초록색 게이지 이미지
    public float maxWidth = 340f; // 체력 100%
    public float minWidth = 0f;   // 체력 0%

    [Header("HP Animation")]
    public float smoothSpeed = 10f; // ⭐ 클수록 빠름 (Inspector 조절)

    private Coroutine hpAnimCoroutine;

    public GameReport gameReport;

    void Start()
    {

        if (gameReport == null)
        {
            gameReport = FindObjectOfType<GameReport>();
        }
        
        hp = maxHP;
        SetHPBarImmediate();
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        hp = Mathf.Clamp(hp, 0, maxHP);

        Debug.Log("타워 체력: " + hp);

        AnimateHPBar();
        

        if (hp <= 0)
        {
            if (gameReport != null)
            {
                gameReport.GameOver();
            }
            
            Destroy(gameObject);
        }
    }

    public void RestoreToFull()
    {
        hp = maxHP;
        Debug.Log("타워 체력: " + hp);

        AnimateHPBar();
    }

    // ===============================
    // HP Bar 애니메이션 처리
    // ===============================
    void AnimateHPBar()
    {
        if (hpBar == null) return;

        // 기존 애니메이션 중지
        if (hpAnimCoroutine != null)
            StopCoroutine(hpAnimCoroutine);

        hpAnimCoroutine = StartCoroutine(SmoothHPBar());
    }

    IEnumerator SmoothHPBar()
    {
        float hpRatio = (float)hp / maxHP;
        float targetWidth = Mathf.Lerp(minWidth, maxWidth, hpRatio);

        float currentWidth = hpBar.sizeDelta.x;

        while (Mathf.Abs(currentWidth - targetWidth) > 0.1f)
        {
            currentWidth = Mathf.Lerp(
                currentWidth,
                targetWidth,
                Time.deltaTime * smoothSpeed
            );

            Vector2 size = hpBar.sizeDelta;
            size.x = currentWidth;
            hpBar.sizeDelta = size;

            yield return null;
        }

        // 오차 보정
        Vector2 finalSize = hpBar.sizeDelta;
        finalSize.x = targetWidth;
        hpBar.sizeDelta = finalSize;
    }

    // 시작 시 즉시 반영
    void SetHPBarImmediate()
    {
        if (hpBar == null) return;

        float hpRatio = (float)hp / maxHP;
        float width = Mathf.Lerp(minWidth, maxWidth, hpRatio);

        Vector2 size = hpBar.sizeDelta;
        size.x = width;
        hpBar.sizeDelta = size;
    }
}
