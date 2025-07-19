using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalPortal : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("Đối tượng sẽ bị dịch chuyển (thường là Player)")]
    public Transform targetToTeleport;
    [Tooltip("Tag collider để kích hoạt portal")]
    public string triggerTag = "Player";

    [Header("UI Completion")]
    [Tooltip("Panel chứa UI khi hoàn thành (có 2 nút và text)")]
    public GameObject completionPanel;
    [Tooltip("Button reload lại màn")]
    public Button restartButton;
    [Tooltip("Button dịch chuyển qua ải tiếp theo")]
    public Button nextLevelButton;

    [Header("Next Level Settings")]
    [Tooltip("Transform vị trí end để teleport sang ải sau")]
    public Transform endPoint;

    [Tooltip("Delay (giây) trước khi reload nếu cần dùng lại RestartDelay")]
    public float restartDelay = 2f;

    void Start()
    {
        // 1. Ẩn panel ngay từ đầu
        if (completionPanel != null)
            completionPanel.SetActive(false);

        // 2. Gắn listener cho nút
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Khi player chạm portal
        if (col.CompareTag(triggerTag))
        {
            // Hiện bảng thông báo
            if (completionPanel != null)
                completionPanel.SetActive(true);

            // Khóa control của player (giả sử controller có script PlayerController)
            if (targetToTeleport != null)
            {
                var ctrl = targetToTeleport.GetComponent<PlayerController>();
                if (ctrl != null)
                    ctrl.enabled = false;
            }
        }
    }

    // Xử lý khi nhấn Restart
    void OnRestartClicked()
    {
        // Nếu muốn delay: Invoke(nameof(ReloadScene), restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Xử lý khi nhấn Next Level
    void OnNextLevelClicked()
    {
        // 1. Ẩn panel, mở control
        if (completionPanel != null)
            completionPanel.SetActive(false);

        if (targetToTeleport != null)
        {
            var ctrl = targetToTeleport.GetComponent<PlayerController>();
            if (ctrl != null)
                ctrl.enabled = true;

            // 2. Teleport tới endPoint
            if (endPoint != null)
                targetToTeleport.position = endPoint.position;
        }
    }

    // Nếu muốn delay reload, dùng hàm sau và Invoke ở OnRestartClicked
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}