using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] IntObject harvestCounter;
    [SerializeField] GameObject counterImage;
    [SerializeField] TextMeshProUGUI counterText;

    void Awake()
    {
        FindObjectOfType<PlayerHarvester>().HarvestAction += OnHarvest;
    }

    void OnHarvest()
    {
        if (!counterImage.activeSelf)
        {
            counterImage.SetActive(true);
        }

        counterText.text = harvestCounter.value.ToString();
    }
}