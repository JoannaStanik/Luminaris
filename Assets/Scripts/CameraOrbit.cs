using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;     // Obiekt, wokó³ którego chcemy kr¹¿yæ
    public float distance = 6f;  // Dystans kamery od obiektu
    public float speed = 20f;    // Prêdkoœæ obrotu (stopnie na sekundê)
    public float height = 1.5f;  // Wysokoœæ kamery od poziomu celu

    private float angle = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        angle += speed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * distance;
        float z = Mathf.Sin(rad) * distance;

        Vector3 position = new Vector3(x, height, z) + target.position;
        transform.position = position;

        transform.LookAt(target);
    }
}
