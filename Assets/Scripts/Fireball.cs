using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 12f;
    public int damage = 25;
    public float lifeTime = 4f;
    void Start() => Destroy(gameObject, lifeTime);

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var hp = other.GetComponent<EnemyHealth>();
            if (hp != null) hp.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
