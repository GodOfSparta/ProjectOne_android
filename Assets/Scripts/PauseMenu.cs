using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                TogglePause();
            }
        }
        // �������� ��������� �� ���� � �����, � ���� ��, ��������� ���������� Update
        if (GameIsPaused)
        {
            return;
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        // ������������� �� ������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // ������������ �����
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // ������������� �����
        GameIsPaused = true;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Resume();
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������������� ���� � ���������
#else
        Application.Quit(); // ������� �� ����
#endif 
    }
}
