using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject deathMenuPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    void Awake()
    {
        deathMenuPanel.SetActive(false);
        restartButton.onClick.AddListener(OnRestart);
        exitButton.onClick.AddListener(OnExit);
    }

    public void ShowDeathMenu()
    {
        deathMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnExit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // đặt đúng tên scene MainMenu của bạn
    }
}