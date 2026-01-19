using UnityEngine;

public class EnemyHealthBarSpawner : MonoBehaviour
{
    public EnemyHealthBarUI healthBarPrefab;

    private void Start()
    {
        var hp = GetComponent<EnemyHealth>();
        if (hp == null)
        {
            Debug.LogWarning("Brak EnemyHealth na przeciwniku");
            return;
        }

        var bar = Instantiate(healthBarPrefab);
        bar.Init(hp);
    }
}