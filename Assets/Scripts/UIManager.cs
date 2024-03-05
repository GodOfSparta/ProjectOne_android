using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestScoreText; // Новая переменная для отображения лучшего счета
    private float timer;
    private int currentScore; // Новая переменная для хранения текущего счета
    private int bestScore; // Новая переменная для хранения лучшего счета

    private void Start()
    {
        timer = 0f;
        currentScore = 0;
        bestScore = PlayerPrefs.GetInt("BestScore", 0); // Загрузка лучшего счета из сохраненных данных
        UpdateBestScoreText();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        currentScore = Mathf.RoundToInt(timer);
        UpdateTimerText();

        // Обновление лучшего счета при достижении нового рекорда
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore); // Сохранение лучшего счета
            UpdateBestScoreText();
        }
    }

    void UpdateTimerText()
    {
        timerText.text = string.Format("{0}", currentScore);
    }

    void UpdateBestScoreText()
    {
        bestScoreText.text = string.Format("Best: {0}", bestScore);
    }
}