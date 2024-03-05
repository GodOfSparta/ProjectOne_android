using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    public int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        // Если пуля столкнулась с объектом, имеющим тег "Player" или "Ground"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Получаем ссылку на скрипт игрока
            ManualPlayerController player = collision.gameObject.GetComponent<ManualPlayerController>();
            // Наносим урон игроку
            player.TakeDamage(damage);
            // Уничтожаем пулю
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        { // Уничтожаем пулю
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("BossKiller"))
        {
            Destroy(collision.gameObject);
        }
    }
}
