using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI; //텔레포트 표시 UI
    LineRenderer lr; //라인렌더러 표시 변수
    Vector3 originScale = Vector3.one * 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.Get(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch)) //왼쪽 컨트롤러의 원 버튼 눌렀을떄 텔레포트에 대한 정보 활성화
        {
            // 왼쪽 컨트롤러 기준으로 Ray를 만들기
            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Terrain"); //지면 레이어 확인
            if (Physics.Raycast(ray, out hitInfo, 200, layer)) //지면에 닿았을 경우 terrain만 ray충돌 검출
            {
                lr.SetPosition(0, ray.origin); //레이가 부딫힌 지점에 라인 그려내기
                lr.SetPosition(1, hitInfo.point);

                teleportCircleUI.gameObject.SetActive(true); //레이 부딫힌 지점에 UI를 표시
                teleportCircleUI.position = hitInfo.point;
                teleportCircleUI.forward = hitInfo.normal; // 표시될 UI의 방향 정의
                teleportCircleUI.localScale = originScale * Mathf.Max(1, hitInfo.distance); //텔레포드 ui의 크기가 거리에 따라 보정됌
            }
            else
            {
                lr.SetPosition(0, ray.origin); //레이 충돌 발생하지 않는다면 선이 ray방향으로 그려지도록 처리
                lr.SetPosition(1, ray.origin + ARAVRInput.RHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);
            }
        }
        
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            lr.enabled = true;
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            //lr.enabled = false;
            if (teleportCircleUI.gameObject.activeSelf)
            {
                GetComponent<CharacterController>().enabled = false;

                transform.position = teleportCircleUI.position + Vector3.up; //텔레포트 ui의 위치로 순간이동
                GetComponent<CharacterController>().enabled = true;
            }

            teleportCircleUI.gameObject.SetActive(false);
        }
    }
}
