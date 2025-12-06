using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BombLauncher : MonoBehaviour
{
    public GameObject bombPrefab; // 폭탄 프리팹 연결
    public Transform throwPoint; // 폭탄이 생성될 위치 (손 끝)

    public int maxUses = 3; // 최대 횟수
    public int currentUses = 0; // 현재 사용 횟수
    public float cooldown = 1.5f; // 쿨타임
    private bool isCooldown = false; // 쿨타임 상태

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // E키 입력
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptThrow();
        }
    }

    void AttemptThrow()
    {
        // 횟수 초과 체크
        if (currentUses >= maxUses)
        {
            Debug.Log("폭탄소진");
            return;
        }

        // 쿨타임 체크
        if (isCooldown)
        {
            Debug.Log("폭탄 쿨타임 중...");
            return;
        }

        SpawnBomb();
    }

    void SpawnBomb()
    {
        // 1. 상태 업데이트
        isCooldown = true;
        currentUses++;
        StartCoroutine(CooldownRoutine());

        // 2. 폭탄 생성
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
        Debug.Log("폭탄 준비 완료!");
    }
    
    // 웨이브 클리어 -> 쿨타임 횟수 초기화
    public void ResetBombs()
    {
        currentUses = 0;
        isCooldown = false;
    }
}
