using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private float delayBeforeLoading = 0.5f; // Задержка в секундах

    public void RestartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(ReStartGameWithDelay());
    }

    IEnumerator ReStartGameWithDelay()
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(delayBeforeLoading);

        // Переключаемся на текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}