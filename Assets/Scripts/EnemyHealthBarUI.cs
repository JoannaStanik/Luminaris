using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;

    [Header("Follow")]
    public Vector3 offset = new Vector3(0, 50f, 0);

    private EnemyHealth target;
    private Camera cam;

    public void Init(EnemyHealth enemy)
    {
        target = enemy;
        cam = Camera.main;

        slider.minValue = 0;
        slider.maxValue = target.MaxHealth;
        slider.value = target.CurrentHealth;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // nad g³ow¹
        transform.position = target.transform.position + offset;

        // zawsze do kamery
        if (cam != null)
            transform.forward = cam.transform.forward;

        // aktualizacja HP
        slider.value = target.CurrentHealth;
    }
}