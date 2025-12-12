using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class TutoSceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger) :
        // 인덱스 트리거 버튼이 '방금 눌렸는지' 확인합니다.
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            // "Game" 씬으로 전환합니다.
            ChangeToGameScene();
        }
    }
    
    // 씬 전환 기능을 수행하는 함수
    void ChangeToGameScene()
    {
        // 씬 매니저를 이용해 지정된 이름의 씬을 로드합니다.
        SceneManager.LoadScene("MainWaveScene");
        
        // 씬 전환이 완료되었음을 디버그 콘솔에 출력 (선택 사항)
        Debug.Log("인덱스 트리거가 눌려 'Game' 씬으로 전환합니다.");
    }
}