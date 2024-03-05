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
        // Проверка находится ли игра в паузе, и если да, прерываем выполнение Update
        if (GameIsPaused)
        {
            return;
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        // Переключаемся на текущую сцену
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
        Time.timeScale = 1f; // Возобновляем время
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Останавливаем время
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
        UnityEditor.EditorApplication.isPlaying = false; // Останавливаем игру в редакторе
#else
        Application.Quit(); // Выходим из игры
#endif 
    }
}
