using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestScoreText; // ����� ���������� ��� ����������� ������� �����
    private float timer;
    private int currentScore; // ����� ���������� ��� �������� �������� �����
    private int bestScore; // ����� ���������� ��� �������� ������� �����

    private void Start()
    {
        timer = 0f;
        currentScore = 0;
        bestScore = PlayerPrefs.GetInt("BestScore", 0); // �������� ������� ����� �� ����������� ������
        UpdateBestScoreText();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        currentScore = Mathf.RoundToInt(timer);
        UpdateTimerText();

        // ���������� ������� ����� ��� ���������� ������ �������
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore); // ���������� ������� �����
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