using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private float delayBeforeLoading = 0.5f; // �������� � ��������

    public void RestartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(ReStartGameWithDelay());
    }

    IEnumerator ReStartGameWithDelay()
    {
        // ���� ��������� ���������� ������
        yield return new WaitForSeconds(delayBeforeLoading);

        // ������������� �� ������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}