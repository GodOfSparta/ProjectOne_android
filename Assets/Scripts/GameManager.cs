using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Синглтон для доступа из других скриптов

    public int enemiesKilled;
    public int bestEnemiesKilled;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новой сцены
            LoadBestScore(); // Загружаем лучший счет при запуске
        }
        else
        {
            Destroy(gameObject); // Уже есть другой экземпляр, уничтожаем этот
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

    // Вызывай этот метод, когда игрок убивает врага
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
