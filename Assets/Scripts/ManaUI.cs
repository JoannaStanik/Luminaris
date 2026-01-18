using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    public Slider manaSlider;
    public PlayerMana playerMana;

    void Awake()
    {
        if (manaSlider == null) manaSlider = GetComponent<Slider>();
        if (playerMana == null) playerMana = GetComponent<PlayerMana>();
    }

    void OnEnable()
    {
        if (playerMana != null)
            playerMana.OnManaChanged += UpdateUI;
    }

    void OnDisable()
    {
        if (playerMana != null)
            playerMana.OnManaChanged -= UpdateUI;
    }

    void Start()
    {
        if (playerMana !=null) UpdateUI(playerMana.currentMana, playerMana.maxMana);
    }

    void UpdateUI(float current, float max)
    {
        manaSlider.maxValue = max;
        manaSlider.value = current;
    }
}