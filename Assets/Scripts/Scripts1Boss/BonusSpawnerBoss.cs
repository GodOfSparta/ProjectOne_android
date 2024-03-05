using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawnerBoss : MonoBehaviour
{
    public GameObject[] bonusPrefabs; // ������ ����� �������� �������
    float spawnInterval = 9f; // �������� ������ �������

    void Start()
    {
        // ��������� ������������� ����� ������� SpawnBonus ������ spawnInterval ������
        InvokeRepeating("SpawnBonus", spawnInterval, spawnInterval);
    }

    void SpawnBonus()
    {
        // �������� ��������� ������ ������
        int index = Random.Range(0, bonusPrefabs.Length);
        GameObject bonusPrefab = bonusPrefabs[index];

        // �������� ��������� ������� ��� ������ ������
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 4.5f), transform.position.y, transform.position.z);

        // ������� ����� �����
        Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);
    }
}
