using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem2Scene : MonoBehaviour
{
    public GameObject stonePrefab; // Префаб камня
    public Transform player; // Ссылка на игрока
    public Transform throwPoint; // Точка вылета камня
    public float throwForce; // Сила броска
    private Animator golemAnimator;
    private float destroyDelay = 7f;

    void Start()
    {
        golemAnimator = GetComponent<Animator>();
        AssignPlayer();
        // Запускаем таймер для уничтожения препятствия через заданное время
        Invoke("DestroyObstacle", destroyDelay);
    }

    void AssignPlayer()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void DestroyObstacle()
    {
        // Уничтожаем препятствие
        Destroy(gameObject);
    }

    public void ThrowStone()
    {
        // Создаем камень в точке вылета
        GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        // Вычисляем направление к игроку
        Vector3 direction = (player.position - throwPoint.position).normalized;
        // Добавляем силу к камню, чтобы он полетел в сторону игрока
        stone.GetComponent<Rigidbody>().AddRelativeForce(direction * throwForce, ForceMode.VelocityChange);
    }
}
