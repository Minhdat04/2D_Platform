using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    AudioManager audioManager;

    [Header("UI References")]
    public TMP_Text coinText;  // drag in CoinText
    public Image coinIcon;  // drag in CoinIcon

    [Header("Gameplay")]
    public int coinCount = 0;

    void Awake()
    {
        audioManager = GameObject
            .FindGameObjectWithTag("Audio")
            .GetComponent<AudioManager>();
    }

    void Start()
    {
        // ensure icon is visible and text is initialized
        coinIcon.enabled = true;
        UpdateCoinUI();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            UpdateCoinUI();

            audioManager.PlaySFX(audioManager.coinPickUp);
            Destroy(other.gameObject);
        }
    }

    void UpdateCoinUI()
    {
        // Show “x10” or just “10” as you prefer
        coinText.text = "Coins: " + coinCount;
    }
}