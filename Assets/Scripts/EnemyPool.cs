using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string enemyType;        // “TKiller”, “NKiller”, “Dendritic”
        public GameObject prefab;       // 해당 타입의 프리팹
        public int initialCount = 20;   // 초기 풀 크기

        [HideInInspector] public Queue<GameObject> objects = new Queue<GameObject>(); 
        //인스펙터 창에서 수정 불가능한 내부변수 , 다른 스크립트에 접근해야하지만 인스펙터에선 수정 금지
    }

    public List<Pool> pools = new List<Pool>();
    private Dictionary<string, Pool> poolMap = new Dictionary<string, Pool>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        foreach (var p in pools)
        {
            poolMap[p.enemyType] = p; // 적 타입 문자열로 해당 풀을 빠르게 검색할 수 있게 저장

             // 초기 풀 크기만큼 미리 생성해서 비활성화 후 큐에 넣기
            for (int i = 0; i < p.initialCount; i++)
            {
                GameObject obj = Instantiate(p.prefab);
                obj.SetActive(false);
                p.objects.Enqueue(obj);
            }
        }
    }

    public GameObject Get(string enemyType)
    {
        // 요청된 타입의 풀이 존재하는지 확인
        if (!poolMap.ContainsKey(enemyType))
        {
            Debug.LogError($"Pool for enemy type '{enemyType}' does not exist.");
            return null;
        }

        Pool p = poolMap[enemyType];

        if (p.objects.Count > 0)
        {
            GameObject obj = p.objects.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // 풀 부족 시 자동 확장
        GameObject newObj = Instantiate(p.prefab);
        return newObj;
    }

    public void Return(string enemyType, GameObject obj)
    {
        obj.SetActive(false);
        poolMap[enemyType].objects.Enqueue(obj);
    }
}

