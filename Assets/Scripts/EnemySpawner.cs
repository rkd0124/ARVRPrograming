using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool enemyPool; // EnemyPool 참조
    public Transform[] spawnPoints; // 소환 위치들

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Spawn(string enemyType)
    {
        GameObject enemy = enemyPool.Get(enemyType);

        // 소환위치에서 랜덤 배치
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = sp.position;
        enemy.transform.rotation = sp.rotation;

        return enemy;
    }
}
