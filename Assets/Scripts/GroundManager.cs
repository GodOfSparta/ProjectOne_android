using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject groundPrefab;
    public float segmentLength = 60f;
    public float destroyTime = 5f; // ����� �� ����������� ����� �����������
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

        // ���������, ����� �� ������� ����� ������� ����� �������
        if (playerX > transform.position.x - playerOffset)
        {
            SpawnGroundSegment();
        }

        // ���������, ����� �� ���������� ��������, ������� ������ �� ��������� ������
        CheckAndDestroyGround();
    }

    void SpawnGroundSegment()
    {
        // ������� ����� ������� �����
        GameObject newGround = Instantiate(groundPrefab, new Vector3(transform.position.x + segmentLength, transform.position.y, transform.position.z), Quaternion.identity);

        // ��������� ���������� � �������� � ������
        GroundSegment segmentInfo = new GroundSegment { segment = newGround, creationTime = Time.time };
        groundSegments.AddLast(segmentInfo);

        // ���������� ������ ���������� ������ � ����� ������ ��������
        transform.position = new Vector3(transform.position.x + segmentLength, transform.position.y, transform.position.z);
    }

    void CheckAndDestroyGround()
    {
        // �������� ���������� ����� ������� ������ � ������� �����������
        float leftBorderX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;

        // ��������� ������ ������� ����� � ������
        LinkedListNode<GroundSegment> node = groundSegments.First;
        while (node != null)
        {
            GroundSegment segmentInfo = node.Value;

            // ���� ����� �������� ����� �� ��������� ����� ������� ������
            if (segmentInfo.segment.transform.position.x + segmentLength / 2 < leftBorderX)
            {
                // ���� ������ ���������� �������, ���������� ������� �����
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
                // ��������� � ���������� �������� � ������
                node = node.Next;
            }
        }
    }
}
