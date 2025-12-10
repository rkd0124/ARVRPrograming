using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCurve : MonoBehaviour
{
    public Transform teleportCircleUI; //텔레포트 표시 UI
    LineRenderer lr; //라인렌더러 표시 변수
    Vector3 originScale = Vector3.one * 0.02f; //최초의 텔레포트 ui크기
    public int lineSmooth = 40;
    public float curveLength = 50;
    public float gravity = -60;
    public float simulateTime = 0.02f; //곡선 시뮬레이션 간격 및 시간
    List<Vector3> lines = new List<Vector3>(); //곡선을 이루는 점들을 기억할 리스트

    private bool CheckHitRay(Vector3 lastPos, ref Vector3 pos)
    {
        Vector3 rayDir = pos - lastPos; //앞점 라스트 포스에서 포스로 향하는 벡터 값 계산(곡선)
        Ray ray = new Ray(lastPos, rayDir);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, rayDir.magnitude)) //레이캐스트 할때 레이를 앞 점과 다음 점 사이의 거리로 한정하기
        {
            pos = hitInfo.point; //다음 점의 위치를 충돌한 지점으로 설정
            int layer = LayerMask.NameToLayer("Terrain");
            if (hitInfo.transform.gameObject.layer == layer)
            {
                teleportCircleUI.gameObject.SetActive(true); //텔레포트 UI활성화
                teleportCircleUI.position = pos; //텔레포트 유아이 위치 지정
                teleportCircleUI.forward = hitInfo.normal; //텔레포트 유아이 방향 설정
                float distance = (pos - ARAVRInput.LHandPosition).magnitude;
                teleportCircleUI.localScale = originScale * Mathf.Max(1, distance); //텔레포트 유아이가 보일 크기 설정
            }
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.02f; //라인 렌더러의 선 너비 지정
        lr.endWidth = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = true; //라인 렌더러 컴포넌트 활성화
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
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
        else if (ARAVRInput.Get(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
        {
            MakeLines();
        }
    }

    void MakeLines()
    {
        lines.RemoveRange(0, lines.Count); //리스트에 담긴 위치정보 비우기
        Vector3 dir = ARAVRInput.LHandDirection * curveLength; //선 그려진 위치 초깃값 설정
        Vector3 pos = ARAVRInput.LHandPosition; // 최초위치 리스트에 담기
        lines.Add(pos);

        for (int i = 0; i < lineSmooth; i++)
        {
            Vector3 lastPos = pos; //위치를 기억
            dir.y += gravity * simulateTime; //등속운동으로 위치 계산
            pos += dir * simulateTime;
            //레이 충돌 체크가 일어났다면
            if (CheckHitRay(lastPos, ref pos))
            {
                lines.Add(pos);
                break; //포스를 받아왔다면 바로 브레이크
            }
            else
            {
                teleportCircleUI.gameObject.SetActive(false); //텔레포트 유아이 비활성화 (충돌 안했으므로)
            }
            lines.Add(pos); //계산한 결과로 위치를 등록
        }
        lr.positionCount = lines.Count; //라인렌더러가 표현할 점의 개수를 등록된 개수의 크기로 할당
        lr.SetPositions(lines.ToArray()); //라인 렌더러에 구해진 점의 정보를 지정
    }
}
