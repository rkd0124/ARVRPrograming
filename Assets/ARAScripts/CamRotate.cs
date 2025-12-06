using UnityEngine;

public class CamRotate : MonoBehaviour
{
    Vector3 angle;
    
    public float sensitivity = 200;

    public Transform playerRoot;

    void Start()
    {
        
        angle = Camera.main.transform.eulerAngles;
        angle.x *= -1;
    }

    void Update()
    {
       
        float yRotation = ARAVRInput.GetAxis("Horizontal", ARAVRInput.Controller.RTouch);

        if (Mathf.Abs(yRotation) > 0.01f)
        {
            playerRoot.Rotate(Vector3.up * yRotation * sensitivity * Time.deltaTime);
        }
    }
}
