using System.Collections.Generic;
using UnityEngine;

public class GroundManager2Scene : MonoBehaviour
{
    public GameObject groundPrefab; // ������ �����
    public GameObject player; // ��������
    public int numberOfTiles = 5; // ���������� �������� �����, ������� ����� ������� ������������
    public float groundLength = 10f; // ����� ������� �����

    private Queue<GameObject> grounds; // ������� ��� �������� �������� �������� �����

    void Start()
    {
        grounds = new Queue<GameObject>();

        for (int i = 0; i < numberOfTiles; i++)
        {
            GameObject ground = Instantiate(groundPrefab, new Vector3(i * groundLength, 0, 0), Quaternion.identity);
            grounds.Enqueue(ground);
        }
    }

    void Update()
    {
        if (player.transform.position.x - grounds.Peek().transform.position.x > groundLength)
        {
            GameObject oldGround = grounds.Dequeue();
            Destroy(oldGround);

            GameObject newGround = Instantiate(groundPrefab, new Vector3(grounds.Peek().transform.position.x + groundLength, 0, 0), Quaternion.identity);
            grounds.Enqueue(newGround);
        }
    }
}
