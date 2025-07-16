using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    AudioManager audioManager;
    public int coinCount = 0;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            audioManager.PlaySFX(audioManager.coinPickUp);
            Destroy(other.gameObject);

            // Log ra để kiểm tra
            Debug.Log("Đã ăn Coin! Tổng coin: " + coinCount);
        }
    }
}