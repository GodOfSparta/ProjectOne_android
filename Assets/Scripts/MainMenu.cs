using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText; // Новая переменная для отображения лучшего счета
    public TextMeshProUGUI enemiesKilledText; // Добавлено для отображения количества убитых врагов
    private float delayBeforeLoading = 1f; // Задержка в секундах
    public GameObject levelsPanel;
    public Button[] levelButtons; // Массив кнопок уровней
    public Button level1Button; // Кнопка для 1 уровня
    public Button level2Button; // Кнопка для 2 уровня
    public Button level3Button; // Кнопка для 3 уровня
    public Button level4Button; // Кнопка для 4 уровня
    public TextMeshProUGUI resetConfirmationText; // Переменная для текста подтверждения сброса
    public GameObject helpPanel;

    void Start()
    {
        // Загружаем лучший счет при старте главного меню
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        int bestEnemiesKilled = PlayerPrefs.GetInt("BestEnemiesKilled", 0);

        // Обновляем UI для отображения лучшего счета и количества убитых врагов
        UpdateBestScoreText(bestScore);
        UpdateEnemiesKilledText(bestEnemiesKilled);

        // Загружаем индекс последнего разблокированного уровня
        int lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel", 1);

        // Активируем кнопки уровней в зависимости от прогресса
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = i <= lastUnlockedLevel;
        }

        // Проверяем, разблокирован ли второй уровень
        int level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0); // Получаем статус разблокировки второго уровня
        level2Button.interactable = level2Unlocked == 1; // Активируем или деактивируем кнопку

        int boss1Defeated = PlayerPrefs.GetInt("Boss1Defeated", 0); // Получаем статус победы над боссом
        level3Button.interactable = boss1Defeated == 1; // Активируем или деактивируем кнопку

        levelsPanel.SetActive(false);
        resetConfirmationText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!levelsPanel.activeSelf && !helpPanel.activeSelf) // Проверяем, неактивна ли панель
            {
                // Если панель неактивна, выходим из игры
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Останавливаем игру в редакторе
#else
        Application.Quit(); // Выходим из игры
#endif 

            }
            else
            {
                helpPanel.SetActive(false);
                levelsPanel.SetActive(false); // Если панель активна, просто скрываем её
            }
        }
    }

    public void OnLevel2ButtonPressed()
    {
        if (PlayerPrefs.GetInt("Level2Unlocked", 0) == 1) // Проверяем, разблокирован ли 2 уровень
        {
            SceneManager.LoadScene(3); // Загружаем сцену с индексом 3
        }
    }

    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadScene(2); // Загружаем сцену с индексом 2
    }

    public void OnLevel3ButtonPressed()
    {
        if (PlayerPrefs.GetInt("Boss1Defeated", 0) == 1) // Проверяем, убит ли босс
        {
            SceneManager.LoadScene(4); // Загружаем сцену с индексом 4
        }
    }
    public void OnLevel4ButtonPressed()
    {
        if (PlayerPrefs.GetInt("Level3Unlocked", 0) == 1) // // Проверяем, разблокирован 3 уровень
        {
            SceneManager.LoadScene(5); // Загружаем сцену с индексом 5
        }
    }

    public void ResetAllSaves()
    {
        PlayerPrefs.DeleteAll(); // Удаляем все сохранённые данные
        PlayerPrefs.Save(); // Сохраняем изменения
        ShowResetConfirmation(); // Показываем сообщение о сбросе
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезагружаем текущую сцену
    }
    IEnumerator RestartGameAfterDelay()
    {
        resetConfirmationText.text = "Все сохранения сброшены"; // Устанавливаем текст подтверждения
        resetConfirmationText.gameObject.SetActive(true); // Делаем текст видимым
        yield return new WaitForSeconds(2f); // Ждём 2 секунды
        resetConfirmationText.gameObject.SetActive(false); // Скрываем текст подтверждения
        StartCoroutine(RestartGameAfterDelay()); // Запускаем корутину для перезапуска игры
    }
    void ShowResetConfirmation()
    {
        resetConfirmationText.text = "Все сохранения сброшены"; // Устанавливаем текст подтверждения
        resetConfirmationText.gameObject.SetActive(true); // Делаем текст видимым
                                                          // Также можно добавить таймер для автоматического скрытия сообщения через несколько секунд
        StartCoroutine(HideResetConfirmation());
    }
    IEnumerator HideResetConfirmation()
    {
        yield return new WaitForSeconds(3); // Ждём 3 секунды
        resetConfirmationText.gameObject.SetActive(false); // Скрываем текст подтверждения
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
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(delayBeforeLoading);

        // Получаем текущую активную сцену
        Scene currentScene = SceneManager.GetActiveScene();

        // Вычисляем индекс следующей сцены
        int nextSceneIndex = currentScene.buildIndex + 1;

        // Если следующая сцена превышает общее количество сцен, возвращаемся к первой сцене (индекс 0)
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        // Загружаем следующую сцену
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Останавливаем игру в редакторе
#else
        Application.Quit(); // Выходим из игры
#endif 
    }

    // Метод для обновления текста лучшего счета
    void UpdateBestScoreText(int bestScore)
    {
        bestScoreText.text = string.Format("Best Score: {0}", bestScore);
    }

    // Метод для обновления текста количества убитых врагов
    void UpdateEnemiesKilledText(int bestEnemiesKilled)
    {
        enemiesKilledText.text = string.Format("Best Killed: {0}", bestEnemiesKilled);
    }
}
