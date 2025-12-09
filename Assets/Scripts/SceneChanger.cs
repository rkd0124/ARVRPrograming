using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string mainSceneName = "MainWaveScene";
    // 메인씬 이름이 위 변수랑 똑같아야함


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
        //씬 매니저 빌드설정
        //제가 해두겠슴다~
    }

    /* UI가 아직 없어서 여기에 주석 달아둘게
    1. 아무 오브젝트(씬 전환될때까지 유지되는 것)에 이 스크립트를 넣어둬
    2. 버튼 선택해서 인스팩터 창 쭉 내리다보면 On Cilck() 있어
    3. 거기서 플러스 버튼 눌러서 슬롯 추가
    4. 슬롯의 런타임 온리 매뉴 밑에 "1번 과정에서 스크립트 추가한 오브젝트"를 집어넣어
    5. 오른쪽에 no Function 거기서 SceneChanger누르고 LoadMainScene이거 누르면 연결됨다~~

    */
}
