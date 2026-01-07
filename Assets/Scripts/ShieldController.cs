using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldVisual;
    public float duration = 3f;

    public bool IsActive {  get; private set; }

    public void Activate()
    {
        if (IsActive) return;
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        IsActive = true;
        if (shieldVisual != null) shieldVisual.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (shieldVisual != null) shieldVisual.SetActive(false);
        IsActive = false;
    }
}