using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private float destroyDelay = 7f;

    private void Start()
    {
        // ��������� ������ ��� ����������� ����������� ����� �������� �����
        Invoke("DestroyObstacle", destroyDelay);
    }

    private void DestroyObstacle()
    {
        // ���������� �����������
        Destroy(gameObject);
    }
}
