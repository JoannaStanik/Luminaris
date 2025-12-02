using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float zoomSpeed = 2f;
    public float minDistance = 3f;
    public float maxDistance = 10f;
    public float mouseSensitivity = 3f;
    public float cameraHeight = 2f;

    public bool isLocked = false;

    float rotationX = 0f;

    void Update()
    {
        if (isLocked || target == null) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
    }

    void LateUpdate()
    {
        if (isLocked || target == null) return;

        Quaternion rotation = Quaternion.Euler(20f, rotationX, 0f);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset + Vector3.up * cameraHeight;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    public Quaternion GetCameraRotation()
    {
        return Quaternion.Euler(0, rotationX, 0);
    }
}