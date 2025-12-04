using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveData
    {
        public int TkillerCount;
        public int NKCount;
        public int SuziSangCount;
        public float spawnInterval;          // 소환 쿨타임
    }

    public EnemySpawner spawner; //적 스포너
    public List<WaveData> waves; //웨이브 정보
    public int currentWave = 0;

    private int enemiesToSpawn;
    private int enemiesAlive;

    public Tower tower;               // 타워 체력 회복
    public ResourceManager resources; // 폭탄/얼음/무기 충전

    public Material[] backgroundMaterials;   // 웨이브별 맵 텍스처

    public Renderer mapRenderer;

    // Start is called before the first frame update
    void Start()
    {
        StartWave(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(int waveIndex)
    {
        currentWave = waveIndex;
        WaveData data = waves[waveIndex];

        // 배경 텍스처 변경
        mapRenderer.material = backgroundMaterials[waveIndex];

        // 적 총합 계산
        enemiesToSpawn = data.TkillerCount + data.NKCount + data.SuziSangCount;
        enemiesAlive = enemiesToSpawn;

        StartCoroutine(RunWave(data));
    }

    IEnumerator RunWave(WaveData data)
    {
        int t = data.TkillerCount;
        int n = data.NKCount;
        int s = data.SuziSangCount;

        while (t > 0 || n > 0 || s > 0)
        {
            if (t > 0)
            {
                spawner.Spawn("Tkiller");
                t--;
            }

            if (n > 0)
            {
                spawner.Spawn("NK");
                n--;
            }

            if (s > 0)
            {
                spawner.Spawn("SuziSang");
                s--;
            }

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
