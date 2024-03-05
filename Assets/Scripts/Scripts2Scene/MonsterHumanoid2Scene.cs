using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHumanoid2Scene : MonoBehaviour
{
    float moveSpeed = 6f;
    private float destroyDelay = 7f;

    private void Start()
    {
        // ��������� ������ ��� ����������� ����������� ����� �������� �����
        Invoke("DestroyObstacle", destroyDelay);
    }

    void Update()
    {
        Vector3 forwardMovement = new Vector3(0, 0, 1);
        transform.Translate(forwardMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void DestroyObstacle()
    {
        // ���������� �����������
        Destroy(gameObject);
    }
}