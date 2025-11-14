using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    [Header("Orbit")]
    public float mouseSensitivity = 3f;
    public bool invertY = false;
    [Range(-80, 0)] public float minPitch = -30f;
    [Range(0, 85)] public float maxPitch = 60f;

    [Header("Offsets")]
    public float cameraHeight = 2f;
    public float lookHeight = 1.5f;

    [Header("Zoom")]
    public float distance = 6f;
    public float zoomSpeed = 2f;
    public float minDistance = 3f;
    public float maxDistance = 10f;

    [Header("Prosta kolizja kamery")]
    public LayerMask collisionMask = ~0;
    public float collisionPadding = 0.15f;

    float yaw = 0f;
    float pitch = 15f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

        float my = Input.GetAxis("Mouse Y") * mouseSensitivity * (invertY ? 1f : -1f);
        pitch = Mathf.Clamp(pitch + my, minPitch, maxPitch);
    }

    float GetRefHeight()
    {
        if (target.TryGetComponent<CharacterController>(out var cc))
            return Mathf.Max(1f, cc.height * 0.75f);

        return Mathf.Max(1f, cameraHeight);
    }

    void LateUpdate()
    {
        if (target == null) return;

        float refH = GetRefHeight();
        Vector3 pivot = target.position + Vector3.up * refH;
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desired = pivot + rot * new Vector3(0, 0, -distance);
        Vector3 dir = (desired - pivot).normalized;
        float dist = Vector3.Distance(pivot, desired);
        if (Physics.Raycast(pivot, dir, out RaycastHit hit, dist, collisionMask, QueryTriggerInteraction.Ignore))
            desired = hit.point - dir * collisionPadding;

        transform.position = desired;

        float lookH = refH * 0.85f;
        transform.LookAt(target.position + Vector3.up * lookH);
    }


    public Quaternion GetCameraRotation()
    {
        return Quaternion.Euler(0f, yaw, 0f);
    }
}
