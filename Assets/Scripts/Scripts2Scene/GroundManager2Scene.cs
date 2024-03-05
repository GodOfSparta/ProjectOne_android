using System.Collections.Generic;
using UnityEngine;

public class GroundManager2Scene : MonoBehaviour
{
    public GameObject groundPrefab; // Префаб земли
    public GameObject player; // Персонаж
    public int numberOfTiles = 5; // Количество участков земли, которые будут активны одновременно
    public float groundLength = 10f; // Длина участка земли

    private Queue<GameObject> grounds; // Очередь для хранения активных участков земли

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
