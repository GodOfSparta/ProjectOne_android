using System.Collections;
using UnityEngine;

public class ObstacleSpawner2Scene : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // ������ � ��������� �����������
    public float spawnInterval; // �������� ����� ���������� �����������
    public float spawnDistance; // ���������� �� ������ ��� �������� �����������
    public float obstacleHeight; // ������, �� ������� ��������� �����������

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (playerTransform.position.x + spawnDistance > transform.position.x)
            {
                SpawnObstacle();
            }
        }
    }

    void SpawnObstacle()
    {
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        Quaternion obstacleRotation = obstaclePrefab.transform.rotation;

        Instantiate(obstaclePrefab, new Vector3(playerTransform.position.x + spawnDistance, obstacleHeight, transform.position.z), obstacleRotation);
    }
}
