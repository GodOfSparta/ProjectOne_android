using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawnerBoss : MonoBehaviour
{
    public GameObject[] bonusPrefabs; // Массив ваших префабов бонусов
    float spawnInterval = 9f; // Интервал спавна бонусов

    void Start()
    {
        // Запускаем повторяющийся вызов функции SpawnBonus каждые spawnInterval секунд
        InvokeRepeating("SpawnBonus", spawnInterval, spawnInterval);
    }

    void SpawnBonus()
    {
        // Выбираем случайный префаб бонуса
        int index = Random.Range(0, bonusPrefabs.Length);
        GameObject bonusPrefab = bonusPrefabs[index];

        // Выбираем случайную позицию для спавна бонуса
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 4.5f), transform.position.y, transform.position.z);

        // Создаем новый бонус
        Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);
    }
}
