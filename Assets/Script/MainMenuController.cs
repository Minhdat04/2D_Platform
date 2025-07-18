using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Tooltip("Tên scene gameplay chính")]
    public string gameSceneName = "SampleScene";

    [Tooltip("Panel chứa menu chính (Start, Options, Exit)")]
    public GameObject menuPanel;

    [Tooltip("Panel chứa các thiết lập Options")]
    public GameObject optionsPanel;

    void Start()
    {
        // lúc bắt đầu chỉ show menu chính, ẩn Options
        if (menuPanel != null) menuPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    // Nút Start
    public void OnStartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Nút Options: hiện panel Options, ẩn menu chính
    public void OnOpenOptions()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    // Nút Back trong Options: ẩn Options, hiện lại menu chính
    public void OnCloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);
    }

    // Nút Exit
    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}