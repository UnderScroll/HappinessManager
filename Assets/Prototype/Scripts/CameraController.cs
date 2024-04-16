using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float rotationSpeed = 1.0f;

    private Camera cam;
    private float targetAngle;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cam.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            cam.transform.RotateAround(Vector3.zero, Vector3.up, -Time.deltaTime * rotationSpeed);
        }
    }
}
