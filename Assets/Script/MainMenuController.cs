using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Tooltip("Tên scene gameplay chính")]
    public string gameSceneName = "SampleScene";

    [Tooltip("Panel chứa các thiết lập Options")]
    public GameObject optionsPanel;

    // Nút Start
    public void OnStartGame()
    {
        // Tải scene gameplay
        SceneManager.LoadScene(gameSceneName);
    }

    // Nút Options: hiện panel Options
    public void OnOpenOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    // Nút Back trong Options: ẩn panel
    public void OnCloseOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    // Nút Exit
    public void OnExitGame()
    {
        // Nếu chạy dưới Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Nếu build executable
        Application.Quit();
#endif
    }
}