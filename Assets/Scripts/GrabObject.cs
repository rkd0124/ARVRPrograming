using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    bool isGrabbing = false;

    GameObject grabbedObject;

    public LayerMask grabbedLayer;

    public float grabRange = 0.2f;

    Vector3 prevPos;

    float throwPower = 10;

    Quaternion prevRot;
    public float rotPower = 5;

    public bool isRemoteGrab = true;

    public float remoteGrabDistance = 20;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing == false)
        {
            TryGrab();
        }

        else
        {
            TryUngrab();
        }
    }

    private void TryGrab()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange, grabbedLayer);
            int closest = -1;
            float closestDistance = float.MaxValue; //가장 가까운 폭탄

            for (int i = 0; i < hitObjects.Length; i++){
                var rigid = hitObjects[i].GetComponent<Rigidbody>();
                if (rigid == null) continue;

                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.RHandPosition);

                if (nextDistance < closestDistance)
                {
                    closest = i;
                    closestDistance = nextDistance;
                }
            }

            if (closest >-1)
            {
                isGrabbing = true;

                grabbedObject = hitObjects[closest].gameObject;

                grabbedObject.transform.parent = ARAVRInput.RHand;

                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                prevPos = ARAVRInput.RHandPosition;
                prevRot = ARAVRInput.RHand.rotation;
            }
        }
    }

    private void TryUngrab()
    {
        Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos);
        prevPos = ARAVRInput.RHandPosition;

        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);

        prevRot = ARAVRInput.RHand.rotation;

        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            isGrabbing = false;

            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;

            grabbedObject.transform.parent = null;

            grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

            float angle;
            Vector3 axis;

            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime)*angle*axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;


            grabbedObject = null;
        }
    }
}
