using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    bool isGrabbing = false; //��ü�� ����ִ����� Ȯ���ϴ� ����
    GameObject grabbedObject; //��ü�� ��� �ִ����� ����
    public LayerMask grabbedLayer; // ���� ��ü�� ����
    public float grabRange; //���� �� �ִ� �Ÿ�
    Vector3 prevPos;
    public float throwPower = 10;
    Quaternion prevRot;
    public float rotPower = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing == false)
        {
            TryGrab(); //��� �õ� Ȯ�� �Լ� <- �ǹ������� �ٷ� Ȯ�� ����
        }
        else
        {
            TryUngrab();
        }
    }

    private void TryGrab()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch)) //Grab��ư ������,
        {
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.LHandPosition,grabRange,grabbedLayer); //���� �������� ��� ��ź ������Ʈ ����(����Ʈ�� ����)

            int closest = -1; //���� ����� ��ź �ε���
            float closestDistance = float.MaxValue;

            for (int i = 0; i < hitObjects.Length; i++)
            {
                var rigid = hitObjects[i].GetComponent<Rigidbody>(); //�հ� ���� ����� ��ü�� �Ÿ� ����
                if (rigid == null) continue;

                Vector3 nextPos = hitObjects[i].transform.position; //����Ʈ �� ���� ��ü�� ���� �Ÿ�
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.LHandPosition);

                if(nextDistance < closestDistance) //���� ��ü���� �Ÿ��� �� �����ٸ� �װ� ����
                {
                    closest = i;
                    closestDistance = nextDistance;
                }
            }

            if (closest > -1) //��ü�� ����� ��
            {
                isGrabbing = true; //����� ��ü �����Ƿ� Ʈ��
                grabbedObject = hitObjects[closest].gameObject; //���� ��ü�� ���� ���
                grabbedObject.transform.parent = ARAVRInput.LHand; //���� ��ü�� ���� �ڽ����� ���

                grabbedObject.GetComponent<Rigidbody>().isKinematic = true; //���� ��ü�� ���� ���� ��� ����

                prevPos = ARAVRInput.LHandPosition; //�ʱ� ��ġ�� ����

                prevPos = ARAVRInput.LHandPosition; //�ʱ� ��ġ�� ����
                prevRot = ARAVRInput.LHand.rotation; //�ʱ� ȸ���� ����
            }
        }
    }

    private void TryUngrab() //���� ��ü ���Ҵٸ�
    {
        Vector3 throwDirection = (ARAVRInput.LHandPosition - prevPos); //���� ����
        prevPos = ARAVRInput.LHandPosition; //��ġ ���

        Quaternion deltaRotation = ARAVRInput.LHand.rotation * Quaternion.Inverse(prevRot);
        prevRot = ARAVRInput.LHand.rotation; //���� ȸ�� ����

        if (ARAVRInput.GetUp(ARAVRInput.Button.Thumbstick,ARAVRInput.Controller.LTouch))
        {
            isGrabbing = false;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false; //���� ��� Ȱ��ȭ
            grabbedObject.transform.parent = null; //�θ� �ڽİ��� ����
            grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower; //������ �ӵ� �߰� ���
            grabbedObject = null;
        }
    }
}
