using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFactory : MonoBehaviour
{
    public GameObject PoisonPrefab;      // 풀링할 독 프리팹
    public int poolSize = 20;            // 풀 크기

    private List<GameObject> posions;

    void Awake()
    {
        posions = new List<GameObject>();

        // 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(PoisonPrefab); 
            obj.SetActive(false); // 비활성화 상태로 시작
            posions.Add(obj);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject posion in posions)
        {
            if (!posion.activeInHierarchy)
            {
                posion.SetActive(true); // 사용 직전에 활성화
                return posion;
            }
        }

        // 모두 사용중이면 새로 생성
        GameObject obj = Instantiate(PoisonPrefab);
        obj.SetActive(true);  // 활성화 후 반환
        posions.Add(obj);
        return obj;

        //임시커밋용 주석
    }
}
