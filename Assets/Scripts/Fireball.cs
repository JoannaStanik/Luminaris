using UnityEngine;

public class Fireball : MonoBehaviour
{
    // podstawowe wartoœci
    public float speed = 60f;
    public int damage = 25;
    public float lifeTime = 4f;
    void Start() => Destroy(gameObject, lifeTime);

    void Update()
    {
        // akcja po rozpoznaniu zaklêcia Ignis
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Fireball hit: " +  other.name);

        if (other.CompareTag("Enemy"))
        {
            var hp = other.GetComponent<EnemyHealth>();
            if (hp != null)
            {
                Debug.Log("Enemy HP before hit");
                hp.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
