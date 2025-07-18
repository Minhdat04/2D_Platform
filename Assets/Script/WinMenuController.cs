using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject winMenuPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    void Awake()
    {
        winMenuPanel.SetActive(false);
        restartButton.onClick.AddListener(OnRestart);
        exitButton.onClick.AddListener(OnExit);
    }

    /// <summary>
    /// Gọi để show menu thắng, truyền vào tổng số coin
    /// </summary>
    public void ShowWinMenu(int totalScore)
    {
        // Cập nhật text
        scoreText.text = $"Tổng số điểm của bạn là: {totalScore}";
        // Show panel và pause game
        winMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnExit()
    {
        // reset lại timeScale
        Time.timeScale = 1f;

#if UNITY_EDITOR
    // namespace UnityEditor, chỉ hoạt động trong Editor
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}