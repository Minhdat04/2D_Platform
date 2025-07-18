using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private TMP_Text controlsText;

    void Awake()
    {
        // Pause game (nếu muốn) và show panel
        Time.timeScale = 0f;
        introPanel.SetActive(true);

        continueButton.onClick.AddListener(OnContinue);

        // 1) Cốt chuyện hải tặc
        storyText.text =
            "Thuyền trưởng Hải Tặc Cánh Đại Bàng, sau một cơn bão kinh hoàng, " +
            "đã đánh mất toàn bộ kho báu vàng trên một hòn đảo hoang đầy quái vật. " +
            "Để chuộc lại danh tiếng và số vàng bị rơi rụng, Thuyền trưởng " +
            "phải chiến đấu, vượt qua những bẫy rập hiểm hóc và thu nhặt từng đồng vàng." +
            "\n\nChuyến hành trình gian nan nhưng vinh quang đang chờ ở cuối con đường. " +
            "Hãy lên đường và đòi lại kho báu của bạn!";

        // 2) Controls
        controlsText.text =
            "Điều khiển:\n" +
            "• A / D: Di chuyển trái / phải\n" +
            "• Space: Nhảy\n" +
            "• Chuột trái: Đánh kiếm\n" +
            "• X: Lướt (Dash)";
    }

    private void OnContinue()
    {
        // Unpause và ẩn panel
        Time.timeScale = 1f;
        introPanel.SetActive(false);
    }
}