using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceItemManager : MonoBehaviour
{
    public float requiredGauge = 100f;     // 필요한 게이지
    public float currentGauge = 100f;        // 현재 게이지: 스테이지 초반에는 만땅으로
    public float slowDuration = 5f;        // 감속 지속시간
    public float slowPercent = 0.5f;       // 50% 감속

    bool isReady = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() // Q키 입력 감지
    {
        if (Input.GetKeyDown(KeyCode.Q))   // Q 버튼
        {
            Debug.Log(currentGauge);
            ActivateIceItem();             // 얼음 아이템 발동
        }
    }

    public void AddGauge(float amount)     // 적 처치 시 게이지 증가
    {
        currentGauge += amount; //게이지에 적처치 게이지 추가
        if (currentGauge >= requiredGauge) // 게이지가 발동을 위한 게이지 값 이상일때
        {
            currentGauge = requiredGauge; // 그 값 그대로 고정
            isReady = true;                // 사용 가능
        }
    }

    public void ActivateIceItem() //얼음 발동         
    {
        if (!isReady) return; //얼음 발동이 아니면 종료

        StartCoroutine(SlowAllEnemies()); //감속 시작
        isReady = false; //아이템 사용 불가
        currentGauge = 0f; //게이지 초기화
    }

    // 필드 전체 적 슬로우
    IEnumerator SlowAllEnemies()
    {
        // Enemy 태그가 붙은 모든 오브젝트 가져오기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        List<IEnemy> targets = new List<IEnemy>(); //대상의 적들 리스트

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

        // 원래 속도로 복구
        foreach (IEnemy enemy in targets)
        {
            if (enemy != null)
                enemy.RemoveSlow();
        }
    }
}
