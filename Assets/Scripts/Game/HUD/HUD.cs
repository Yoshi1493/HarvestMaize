using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] IntObject harvestCounter;
    TextMeshProUGUI counterText;

    void Awake()
    {
        counterText = GetComponent<TextMeshProUGUI>();
        FindObjectOfType<PlayerHarvester>().HarvestAction += OnHarvest;
    }

    void OnHarvest()
    {
        counterText.text = harvestCounter.value.ToString();
    }
}