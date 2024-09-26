using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        // affichage de la couleur qui a la valeur dans le gradient (1)
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        // transformation de la valeur de value slider de healthBar en la normalisant pour etre entre 0 et 1
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
