using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private float destroyDelay = 7f;

    private void Start()
    {
        // Запускаем таймер для уничтожения препятствия через заданное время
        Invoke("DestroyObstacle", destroyDelay);
    }

    private void DestroyObstacle()
    {
        // Уничтожаем препятствие
        Destroy(gameObject);
    }
}
