using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int hp = 10; //타워 체력

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("타워 체력: "+hp); //디버그용
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log("타워 체력: "+hp); //디버그용

        if(hp<=0){
            Destroy(gameObject);
        }
    }
}
