using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
   
    public float speed = 5.0f;

    CharacterController cc;

    public float gravity = -20.0f;

    float yVelocity = 0;

    public float jumpPower = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

   /* // Update is called once per frame
    void Update()
    {
        float player_X = ARAVRInput.GetAxis("Horizontal");
        float player_Z = ARAVRInput.GetAxis("Vertical");

        Vector3 dir = new Vector3(player_X,0,player_Z);

        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;
        if(cc.isGrounded){
            
           yVelocity = 0;

           // Q. 왜 여기로 점프를 옮겼냐?
           // A. 바닥에 있어야만 점프가능하게 만든거임, 안 그러면 무한점프&허공답보함
           // 공중점프를 원하면 조건문에서 빼면 됨
           if(ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch)){

            yVelocity = jumpPower; 

            }

        }

        
        dir.y = yVelocity;
        cc.Move(dir*speed*Time.deltaTime);
        
    }*/

    void Update()
    {
        float player_X = ARAVRInput.GetAxis("Horizontal");
        float player_Z = ARAVRInput.GetAxis("Vertical");

        // 1. 이동 입력 벡터
        Vector3 moveDir = new Vector3(player_X, 0, player_Z);
        moveDir = Camera.main.transform.TransformDirection(moveDir);
        moveDir.y = 0; // 카메라 기울기 영향 제거

        // 2. 중력 및 점프 처리
        if (cc.isGrounded)
        {
            yVelocity = -1f;  // 살짝 붙여두기 (0이면 지면 체크 끊기는 문제 있음)

            if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
            {
                yVelocity = jumpPower;
            }
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        // 3. 최종 이동 벡터
        Vector3 finalMove = moveDir * speed;
        finalMove.y = yVelocity;

        cc.Move(finalMove * Time.deltaTime);
    }
}
