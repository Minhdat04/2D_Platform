using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public int coinCount = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);

            // Log ra để kiểm tra
            Debug.Log("Đã ăn Coin! Tổng coin: " + coinCount);
        }
    }
}