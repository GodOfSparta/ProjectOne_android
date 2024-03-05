using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem2Scene : MonoBehaviour
{
    public GameObject stonePrefab; // ������ �����
    public Transform player; // ������ �� ������
    public Transform throwPoint; // ����� ������ �����
    public float throwForce; // ���� ������
    private Animator golemAnimator;
    private float destroyDelay = 7f;

    void Start()
    {
        golemAnimator = GetComponent<Animator>();
        AssignPlayer();
        // ��������� ������ ��� ����������� ����������� ����� �������� �����
        Invoke("DestroyObstacle", destroyDelay);
    }

    void AssignPlayer()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void DestroyObstacle()
    {
        // ���������� �����������
        Destroy(gameObject);
    }

    public void ThrowStone()
    {
        // ������� ������ � ����� ������
        GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        // ��������� ����������� � ������
        Vector3 direction = (player.position - throwPoint.position).normalized;
        // ��������� ���� � �����, ����� �� ������� � ������� ������
        stone.GetComponent<Rigidbody>().AddRelativeForce(direction * throwForce, ForceMode.VelocityChange);
    }
}
