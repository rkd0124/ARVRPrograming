using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_add : MonoBehaviour
{
    public int totalScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Score_plus(int plusScore){
        totalScore += plusScore;
        Debug.LogWarning("현재 점수: "+totalScore);
    }
}
