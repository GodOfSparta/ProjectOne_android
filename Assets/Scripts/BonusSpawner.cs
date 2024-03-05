using System;
using System.Collections;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public GameObject[] bonusPrefabs;
    public float spawnInterval; // �������� ����� ���������� �������
    public float spawnDistance; // ���������� �� ������ ��� �������� ������
    private Transform playerTransform;
    private float bonusHeight = 4f; // ������, �� ������� ��������� ������

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnBonuses());
    }

    IEnumerator SpawnBonuses()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // ���������, ����� �� ������� �����������
            if (playerTransform.position.x + spawnDistance > transform.position.x)
            {
                SpawnBonus();
            }
        }
    }

    void SpawnBonus()
    {
        GameObject bonusPrefab = bonusPrefabs[UnityEngine.Random.Range(0, bonusPrefabs.Length)];

        // �������� ������� �� �������
        Quaternion bonusRotation = bonusPrefab.transform.rotation;

        Instantiate(bonusPrefab, new Vector3(playerTransform.position.x + spawnDistance, bonusHeight, transform.position.z), bonusRotation);
    }
}
