using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �������� ��� ������� �� ������ ��������

    public int enemiesKilled;
    public int bestEnemiesKilled;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ��� �������� ����� �����
            LoadBestScore(); // ��������� ������ ���� ��� �������
        }
        else
        {
            Destroy(gameObject); // ��� ���� ������ ���������, ���������� ����
        }
    }

    public void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestEnemiesKilled", bestEnemiesKilled);
        PlayerPrefs.Save();
    }

    public void LoadBestScore()
    {
        bestEnemiesKilled = PlayerPrefs.GetInt("BestEnemiesKilled", 0);
    }

    // ������� ���� �����, ����� ����� ������� �����
    public void EnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled > bestEnemiesKilled)
        {
            bestEnemiesKilled = enemiesKilled;
            SaveBestScore();
        }
    }
}
