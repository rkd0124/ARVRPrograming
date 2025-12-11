using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class GazePointerCtrl : MonoBehaviour
{
    public Transform uiCanvas; //캔버스
    public UnityEngine.UI.Image gazeImg; //캔버스에 들갈 이미지
    public Video360Play vp360; //360스피어에 추가된 영상 플레이 기능
    public Transform vrCamera; //카메라 지정

    Vector3 defalutScale;
    public float uiScaleVal = 1f;

    bool isHitObj; //인터렉션이 일어나는 오브젝트에 시선이 닿으면 트루, 닿지 않을시 팔스
    GameObject preHitObj; // 이전 프레임의 시선이 머물렀던 오브젝트 정보 담는 변수
    GameObject curHitObj; //현재 프레임의 시선이 머무르는 오브젝트 정보를 담는 변수
    //float curGazeTime;
    public float gazeChargeTime = 3.0f; //시선이 머무는 시간 체크
    float curGazeTime = 0f; //현재의 게이즈 시간 (초기화)

    // Start is called before the first frame update
    void Start()
    {
        defalutScale = uiCanvas.localScale;
        curGazeTime = 0f; //시선체크 관련 변수 시작했을때 초기화
    }

    // Update is called once per frame
    void Update()
    {
        // 캔버스 오브젝트의 스케일을 거리에 따라서 조종
        // 1. 카메라를 기준으로 전방 방향의 좌표 정보 담기 (각도)
        Vector3 dir = vrCamera.forward;
        Ray ray = new Ray(vrCamera.position, dir);
        RaycastHit hitInfo;
        // 3. 레이에 부딫힌 경우 거리값이용해 uiCanvas의 크기를 조절
        if (Physics.Raycast(ray, out hitInfo))
        {
            uiCanvas.localScale = defalutScale * uiScaleVal * hitInfo.distance;
            uiCanvas.position = vrCamera.position + vrCamera.forward * hitInfo.distance; // UI 위치를 충돌 지점 근처로 이동 (카메라 앞쪽에 위치)
            if (hitInfo.transform.tag == "GazeObj")
            {
                isHitObj = true;
            }
            curHitObj = hitInfo.transform.gameObject; // 현재 시선이 닿은 오브젝트 저장
        }
        else // 4. 충돌 발생 안하는 경우 -> 기본 스케일 값으로 uiCanvas크기 조절
        {
            uiCanvas.localScale = defalutScale * uiScaleVal;
            uiCanvas.position = vrCamera.position + vrCamera.forward * 2.0f; 
        }
        // 5. uiCanvas가 사용자를 바라볼수 있도록 반전 (전면 방향을 반대로 바꾸기)
        uiCanvas.rotation = vrCamera.rotation;


        //데이터 처리
        if (isHitObj) //오브젝트에 레이가 닿았을때
        {
            if (curHitObj == preHitObj) //충돌과 바라보는게 같을때 -> 바라보고있음을 설명가능
            {
                curGazeTime = curGazeTime + Time.deltaTime; //바라볼때 시간증가, 게이지 증가
            }
            else
            {
                preHitObj = curHitObj; //이전 프레임의 영상 정보 업데이트
            }
            HitObjChecker(curHitObj, true); // 현재 바라보는 오브젝트에 "시선이 닿았다" 신호 전달
        }
        else //오브젝트를 바라보고 있지 않을때
        {
            curGazeTime = 0;
            if(preHitObj != null)
            {
                HitObjChecker(preHitObj, false);
                preHitObj = null;
            }
        }

        curGazeTime = Mathf.Clamp(curGazeTime, 0, gazeChargeTime); //시선이 머무는 시간을 최솟 최댓값 사이 계산 / 시선 유지 시간 제한 (0 ~ gazeChargeTime 사이로 고정)
        gazeImg.fillAmount = curGazeTime / gazeChargeTime; //0 ~ 100% 값표현. 게이지 차오르는 기능

        //다음을 위한 후속 조치
        isHitObj = false; //위에 트루가 계속 남아있을수 있으므로
        curHitObj = null; //현재보는 오브젝트 비게 만들기
    }

    void HitObjChecker(GameObject hitObj, bool isActive) //히트된 오브젝트 타입별로 작동 방식 구분 / 충돌한 오브젝트 타입에 따라 반응 제어
    {
        if (hitObj.GetComponent<VideoFrame>())//hit가 비디오 플레이어 컴포넌트를 갖고 있는지 확인 / 오브젝트가 VideoFrame 컴포넌트를 가지고 있다면,
        {
            if (isActive)
            {
                hitObj.GetComponent<VideoFrame>().CheckVideoFrame(true); // 시선이 닿았을 때 동작 (예: 비디오 재생)
            }
            else
            {
                hitObj.GetComponent<VideoFrame>().CheckVideoFrame(false); // 시선이 떠났을 때 동작 (예: 비디오 정지)
            }
        }

        if (gazeImg.fillAmount >= 1) //영상 프레임을 충분히 보고 있을때 (게이지가 다 찼을때,)
        {
            vp360.SetVideoPlay(hitObj.transform.GetSiblingIndex()); //영상 인덱스 값 받아오기
        }
    }
}
