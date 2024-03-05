using UnityEngine;
using System.Collections;

public class EnemySpawner2Scene : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Массив с префабами препятствий
    public float spawnInterval; // Интервал между появлением препятствий
    public float spawnDistance; // Расстояние от игрока для создания препятствия
    public float obstacleHeight; // Высота, на которой создаются препятствия

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Запускаем корутину для создания препятствий с интервалом
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Проверяем, нужно ли создать препятствие
            if (playerTransform.position.x + spawnDistance > transform.position.x)
            {
                SpawnObstacle();
            }
        }
    }

    void SpawnObstacle()
    {
        // Выбираем случайный префаб препятствия из массива
        GameObject obstaclePrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Получаем ротацию из префаба
        Quaternion obstacleRotation = obstaclePrefab.transform.rotation;

        // Создаем препятствие в позиции впереди игрока с ротацией префаба
        Instantiate(obstaclePrefab, new Vector3(playerTransform.position.x + spawnDistance, obstacleHeight, transform.position.z), obstacleRotation);
    }
}
