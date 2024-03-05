using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject groundPrefab;
    public float segmentLength = 60f;
    public float destroyTime = 5f; // Время до уничтожения после прохождения
    public float playerOffset = 10f;

    private Transform playerTransform;
    private Camera mainCamera;

    private class GroundSegment
    {
        public GameObject segment;
        public float creationTime;
    }

    private readonly LinkedList<GroundSegment> groundSegments = new LinkedList<GroundSegment>();

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
    }

    void Update()
    {
        float playerX = playerTransform.position.x;

        // Проверяем, нужно ли создать новый сегмент перед игроком
        if (playerX > transform.position.x - playerOffset)
        {
            SpawnGroundSegment();
        }

        // Проверяем, нужно ли уничтожить сегменты, которые далеко за пределами камеры
        CheckAndDestroyGround();
    }

    void SpawnGroundSegment()
    {
        // Создаем новый сегмент земли
        GameObject newGround = Instantiate(groundPrefab, new Vector3(transform.position.x + segmentLength, transform.position.y, transform.position.z), Quaternion.identity);

        // Добавляем информацию о сегменте в список
        GroundSegment segmentInfo = new GroundSegment { segment = newGround, creationTime = Time.time };
        groundSegments.AddLast(segmentInfo);

        // Перемещаем объект управления землей в конец нового сегмента
        transform.position = new Vector3(transform.position.x + segmentLength, transform.position.y, transform.position.z);
    }

    void CheckAndDestroyGround()
    {
        // Получаем координаты левой границы камеры в мировых координатах
        float leftBorderX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;

        // Проверяем каждый сегмент земли в списке
        LinkedListNode<GroundSegment> node = groundSegments.First;
        while (node != null)
        {
            GroundSegment segmentInfo = node.Value;

            // Если центр сегмента земли за пределами левой границы камеры
            if (segmentInfo.segment.transform.position.x + segmentLength / 2 < leftBorderX)
            {
                // Если прошло достаточно времени, уничтожаем сегмент земли
                if (Time.time - segmentInfo.creationTime > destroyTime)
                {
                    Destroy(segmentInfo.segment);
                    LinkedListNode<GroundSegment> nextNode = node.Next;
                    groundSegments.Remove(node);
                    node = nextNode;
                }
            }
            else
            {
                // Переходим к следующему сегменту в списке
                node = node.Next;
            }
        }
    }
}
