using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    public int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        // ���� ���� ����������� � ��������, ������� ��� "Player" ��� "Ground"
        if (collision.gameObject.CompareTag("Player"))
        {
            // �������� ������ �� ������ ������
            ManualPlayerController player = collision.gameObject.GetComponent<ManualPlayerController>();
            // ������� ���� ������
            player.TakeDamage(damage);
            // ���������� ����
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        { // ���������� ����
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("BossKiller"))
        {
            Destroy(collision.gameObject);
        }
    }
}
