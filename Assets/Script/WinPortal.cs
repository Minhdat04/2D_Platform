using UnityEngine;

public class WinPortal : MonoBehaviour
{
    private CoinCollector coinCollector;
    private WinMenuController winMenu;

    void Awake()
    {
        // Tìm 2 component cần thiết
        coinCollector = FindObjectOfType<CoinCollector>();
        winMenu = FindObjectOfType<WinMenuController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Khi player chạm goal
        if (other.CompareTag("Player"))
        {
            int totalCoins = coinCollector != null ? coinCollector.coinCount : 0;
            if (winMenu != null)
                winMenu.ShowWinMenu(totalCoins);
            else
                Debug.LogWarning("WinMenuController not found!");
        }
    }
}