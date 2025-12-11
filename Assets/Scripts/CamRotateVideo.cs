using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���콺 �Է¿� ���� ī�޶� ȸ���ϴ� �ڵ�
// �ʿ��� �Ӽ�: ���� ���� �� ���콺 ����.
public class CamRotateVideo : MonoBehaviour
{
    Vector3 angle; //�� ����
    public float sensitivity = 200; //���콺 �ΰ���
    void Start()
    {
        //�����Ҷ� ���� ī�޶��� ���� ����
        angle = Camera.main.transform.eulerAngles;
        angle.x *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse Y"); //���콺�� �¿� �Է� �ޱ�
        float y = Input.GetAxis("Mouse X");

        angle.x = angle.x + x * sensitivity * Time.deltaTime; //�̵� ���Ŀ� ������ �� �Ӽ����� ȸ�� ���� ������Ŵ
        angle.y = angle.y + y * sensitivity * Time.deltaTime; //Time.deltaTime�� ������ ������ �ð��� �ǹ���
        angle.z = transform.eulerAngles.z; 

        angle.x = Mathf.Clamp(angle.x, -90, 90); //Clamp = �ּڰ� �� �ִ��� ������ �� ����. �� ��� ī�޶��� ���������� 90�� ������ ���ذ�.
        transform.eulerAngles = new Vector3(-angle.x, angle.y, angle.z); //���� ����� ��ġ ���� ��������(transform)
    }
}
