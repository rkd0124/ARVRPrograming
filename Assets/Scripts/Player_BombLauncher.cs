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

    public float interactionDistance = 10f; // 상호작용 가능한 거리
    public float requiredLookTime = 3.0f; // 바라봐야 하는 시간 (3초)
    private float lookTimer = 0f; // 현재 바라본 시간
    private Transform mainCameraTransform; // 메인 카메라 위치

    // Start is called before the first frame update
    void Start()
    {
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }

        else
        {
            Debug.LogError("Main Camera가 씬에 없습니다!");
        }
    }

    // Update is called once per frame
    void Update()
    {

        CheckBombMaker(); // 폭탄 바라보기

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

    void CheckBombMaker()
    {
        Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // 부딪힌 물체의 이름이 "BombMaker" 인지 확인
            if (hit.collider.CompareTag("BombMaker"))
            {
                lookTimer += Time.deltaTime; //시간을 카운트
                if (lookTimer >= requiredLookTime)
                {
                    AttemptThrow(); // 폭탄 생성(쿨타임 검증단계에ㅔ)
                    lookTimer = 0f; // 타이머 초기화 (연속 리필 방지)
                }
                return; // 함수 종료 (아래 초기화 코드 실행 안 함)
            }
        }

        // BombMaker를 안 보고 있거나, 허공을 보면 타이머 초기화
        lookTimer = 0f;
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
