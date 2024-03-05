using UnityEngine;
using UnityEngine.UI;

public class HealthBar1BossScene : MonoBehaviour
{
    public Slider slider; // ������ �� ��������� Slider

    // ��������� ������������� ��������
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // ��������� �������� ��������
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
