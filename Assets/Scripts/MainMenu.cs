using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText; // ����� ���������� ��� ����������� ������� �����
    public TextMeshProUGUI enemiesKilledText; // ��������� ��� ����������� ���������� ������ ������
    private float delayBeforeLoading = 1f; // �������� � ��������
    public GameObject levelsPanel;
    public Button[] levelButtons; // ������ ������ �������
    public Button level1Button; // ������ ��� 1 ������
    public Button level2Button; // ������ ��� 2 ������
    public Button level3Button; // ������ ��� 3 ������
    public Button level4Button; // ������ ��� 4 ������
    public TextMeshProUGUI resetConfirmationText; // ���������� ��� ������ ������������� ������
    public GameObject helpPanel;

    void Start()
    {
        // ��������� ������ ���� ��� ������ �������� ����
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        int bestEnemiesKilled = PlayerPrefs.GetInt("BestEnemiesKilled", 0);

        // ��������� UI ��� ����������� ������� ����� � ���������� ������ ������
        UpdateBestScoreText(bestScore);
        UpdateEnemiesKilledText(bestEnemiesKilled);

        // ��������� ������ ���������� ����������������� ������
        int lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);

        // ���������� ������ ������� � ����������� �� ���������
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = i <= lastUnlockedLevel;
        }

        // ���������, ������������� �� ������ �������
        int level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0); // �������� ������ ������������� ������� ������
        level2Button.interactable = level2Unlocked == 1; // ���������� ��� ������������ ������

        int boss1Defeated = PlayerPrefs.GetInt("Boss1Defeated", 0); // �������� ������ ������ ��� ������
        level3Button.interactable = boss1Defeated == 1; // ���������� ��� ������������ ������

        levelsPanel.SetActive(false);
        resetConfirmationText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!levelsPanel.activeSelf && !helpPanel.activeSelf) // ���������, ��������� �� ������
            {
                // ���� ������ ���������, ������� �� ����
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // ������������� ���� � ���������
#else
        Application.Quit(); // ������� �� ����
#endif 

            }
            else
            {
                helpPanel.SetActive(false);
                levelsPanel.SetActive(false); // ���� ������ �������, ������ �������� �
            }
        }
    }

    public void OnLevel2ButtonPressed()
    {
        if (PlayerPrefs.GetInt("Level2Unlocked", 0) == 1) // ���������, ������������� �� 2 �������
        {
            SceneManager.LoadScene(3); // ��������� ����� � �������� 3
        }
    }

    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadScene(2); // ��������� ����� � �������� 2
    }

    public void OnLevel3ButtonPressed()
    {
        if (PlayerPrefs.GetInt("Boss1Defeated", 0) == 1) // ���������, ���� �� ����
        {
            SceneManager.LoadScene(4); // ��������� ����� � �������� 4
        }
    }
    public void OnLevel4ButtonPressed()
    {
        if (PlayerPrefs.GetInt("Level3Unlocked", 0) == 1) // // ���������, ������������� 3 �������
        {
            SceneManager.LoadScene(5); // ��������� ����� � �������� 5
        }
    }

    public void ResetAllSaves()
    {
        PlayerPrefs.DeleteAll(); // ������� ��� ���������� ������
        PlayerPrefs.Save(); // ��������� ���������
        ShowResetConfirmation(); // ���������� ��������� � ������
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ������������� ������� �����
    }
    IEnumerator RestartGameAfterDelay()
    {
        resetConfirmationText.text = "��� ���������� ��������"; // ������������� ����� �������������
        resetConfirmationText.gameObject.SetActive(true); // ������ ����� �������
        yield return new WaitForSeconds(2f); // ��� 2 �������
        resetConfirmationText.gameObject.SetActive(false); // �������� ����� �������������
        StartCoroutine(RestartGameAfterDelay()); // ��������� �������� ��� ����������� ����
    }
    void ShowResetConfirmation()
    {
        resetConfirmationText.text = "��� ���������� ��������"; // ������������� ����� �������������
        resetConfirmationText.gameObject.SetActive(true); // ������ ����� �������
                                                          // ����� ����� �������� ������ ��� ��������������� ������� ��������� ����� ��������� ������
        StartCoroutine(HideResetConfirmation());
    }
    IEnumerator HideResetConfirmation()
    {
        yield return new WaitForSeconds(3); // ��� 3 �������
        resetConfirmationText.gameObject.SetActive(false); // �������� ����� �������������
    }

    public void HideLevelsPanel()
    {
        levelsPanel.SetActive(false);
    }
    public void ShowLevelsPanel()
    {
        levelsPanel.SetActive(true);
    }
    public void HideHelpsPanel()
    {
        helpPanel.SetActive(false);
    }
    public void ShowHelpsPanel()
    {
        helpPanel.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameWithDelay());
    }

    IEnumerator StartGameWithDelay()
    {
        // ���� ��������� ���������� ������
        yield return new WaitForSeconds(delayBeforeLoading);

        // �������� ������� �������� �����
        Scene currentScene = SceneManager.GetActiveScene();

        // ��������� ������ ��������� �����
        int nextSceneIndex = currentScene.buildIndex + 1;

        // ���� ��������� ����� ��������� ����� ���������� ����, ������������ � ������ ����� (������ 0)
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        // ��������� ��������� �����
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������������� ���� � ���������
#else
        Application.Quit(); // ������� �� ����
#endif 
    }

    // ����� ��� ���������� ������ ������� �����
    void UpdateBestScoreText(int bestScore)
    {
        bestScoreText.text = string.Format("Best Score: {0}", bestScore);
    }

    // ����� ��� ���������� ������ ���������� ������ ������
    void UpdateEnemiesKilledText(int bestEnemiesKilled)
    {
        enemiesKilledText.text = string.Format("Best Killed: {0}", bestEnemiesKilled);
    }
}
