using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveData
    {
        public int TkillerCount; //소환 목표치
        public int NKCount; //소환 목표치
        public int SuziSangCount; //소환 목표치
        public float spawnInterval; // 소환 쿨타임
    }

    public EnemySpawner spawner; //적 스포너
    public List<WaveData> waves; //웨이브 정보
    public int currentWave = 0; //진행중인 웨이브 정보

    private int enemiesToSpawn; // 이번 웨이브에 나올 총 적의 수
    private int enemiesAlive; //살아있는 적의 수

    public Tower tower;               // 타워 체력 회복
    public ResourceManager resources; // 폭탄/얼음 충전

    public Material[] backgroundMaterials;   // 웨이브별 맵 텍스처

    public Renderer mapRenderer;// 텍스쳐를 적용할 배경

    private Score_add scoreManager; //시간당 점수

    // Start is called before the first frame update
    void Start()
    {
        // [중요] 매니저를 먼저 찾고 웨이브 시작 (순서 수정됨)
        scoreManager = FindObjectOfType<Score_add>();
        StartWave(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(int waveIndex) //웨이브 시작!
    {
        currentWave = waveIndex; // 현재 웨이브 번호 갱신
        WaveData data = waves[waveIndex]; // 리스트에서 웨이브의 정보 가져오기

        if (scoreManager != null)
        {
            scoreManager.StartWaveTimer(); //타이머 발동
        }
         
        // 배경 텍스처 변경
        if (waveIndex < backgroundMaterials.Length)
        {
            mapRenderer.material = backgroundMaterials[waveIndex];
        }

        // 적 총합 계산
        enemiesToSpawn = data.TkillerCount + data.NKCount + data.SuziSangCount;
        enemiesAlive = enemiesToSpawn; // 살아있는 적을 적 총합으로 변경

        StartCoroutine(RunWave(data)); //적 소환
        
        Debug.Log($"웨이브 {waveIndex + 1} 시작!");
    }

    IEnumerator RunWave(WaveData data)
    {
        int t = data.TkillerCount; 
        int n = data.NKCount; 
        int s = data.SuziSangCount; 

        while (t > 0 || n > 0 || s > 0)
        {
            if (t > 0) { spawner.Spawn("Tkiller"); t--; }
            if (n > 0) { spawner.Spawn("NK"); n--; }
            if (s > 0) { spawner.Spawn("SuziSang"); s--; }

            yield return new WaitForSeconds(data.spawnInterval);
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--; //적이 죽음

        if (enemiesAlive <= 0) //적이 전멸
            OnWaveCleared(); //웨이브 클리어
    }

    void OnWaveCleared() //웨이브 클리어하면
    {
        // 1. 점수 및 리소스 정산 (즉시 실행)
        if (scoreManager != null)
        {
            scoreManager.CalculateTimeBonus();
        } 

        tower.RestoreToFull(); 
        resources.RechargeAllResources(); 

        Debug.Log("웨이브 클리어");

        // 2. [수정됨] 바로 시작하지 않고, 5초 대기 코루틴 실행
        StartCoroutine(WaitAndStartNextWave());
    }

    // [추가됨] 5초 대기 후 다음 웨이브를 시작하는 코루틴
    IEnumerator WaitAndStartNextWave()
    {
        // 5초 동안 대기
        yield return new WaitForSeconds(5.0f);

        // 다음 웨이브가 있는지 확인 후 실행
        if (currentWave + 1 < waves.Count)
        {
            StartWave(currentWave + 1); //다음 웨이브로
        }
        else //남은 웨이브 없다면
        {
            Debug.Log("모든 웨이브 클리어 (게임 승리)");
            // 승리 UI넣으면 됨
        }
    }

    public void OnTowerDestroyed()
    {
        // 패배 처리
    }
}