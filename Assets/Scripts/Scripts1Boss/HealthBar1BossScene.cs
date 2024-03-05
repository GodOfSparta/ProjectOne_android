using UnityEngine;
using UnityEngine.UI;

public class HealthBar1BossScene : MonoBehaviour
{
    public Slider slider; // Ссылка на компонент Slider

    // Установка максимального здоровья
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // Установка текущего здоровья
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
