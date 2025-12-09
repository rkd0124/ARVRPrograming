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
        mapRenderer.material = backgroundMaterials[waveIndex];

        // 적 총합 계산
        enemiesToSpawn = data.TkillerCount + data.NKCount + data.SuziSangCount;
        enemiesAlive = enemiesToSpawn; // 살아있는 적을 적 총합으로 변경

        StartCoroutine(RunWave(data)); //적 소환
    }

    IEnumerator RunWave(WaveData data)
    {
        int t = data.TkillerCount; // 소환 목표치 : tkiller세포
        int n = data.NKCount; // 소환 목표치 : tkiller세포
        int s = data.SuziSangCount; // 소환 목표치 : tkiller세포

        while (t > 0 || n > 0 || s > 0)// 셋중 하나라도 목표치 미달이면
        {
            if (t > 0)
            {
                spawner.Spawn("Tkiller"); //소환
                t--;
            }

            if (n > 0)
            {
                spawner.Spawn("NK"); //소환
                n--;
            }

            if (s > 0)
            {
                spawner.Spawn("SuziSang"); //소환
                s--;
            }

            yield return new WaitForSeconds(data.spawnInterval);
            //소환 쿨타임
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
        if (scoreManager != null)
        {
            scoreManager.CalculateTimeBonus();
        } //시간 보너스 계산

        tower.RestoreToFull(); //타워의 체력 회복
        resources.RechargeAllResources(); //특수 아이템들(얼음 폭탄)

        if (currentWave + 1 < waves.Count)
            StartWave(currentWave + 1); //다음 웨이브로
    }

    public void OnTowerDestroyed()
    {
        // 패배 처리 — 게임오버 화면 등 연결
    }
}
