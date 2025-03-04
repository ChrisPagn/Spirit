using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ce script g�re la barre de vie d'un personnage, en ajustant la valeur de la sant� 
/// et en modifiant la couleur en fonction de la sant� restante.
/// </summary>
public class HealthBar : MonoBehaviour
{
    // Slider qui repr�sente la barre de vie
    public Slider slider;

    // Gradient utilis� pour changer la couleur de la barre de vie en fonction de la sant�
    public Gradient gradient;

    // Image qui remplit la barre de vie et dont la couleur va changer
    public Image fill;

    /// <summary>
    /// D�finit la sant� maximale et initialise la barre de vie � cette valeur.
    /// </summary>
    /// <param name="maxHealth">La sant� maximale du personnage</param>
    public void SetMaxHealth(int maxHealth)
    {
        // D�finit la valeur maximale du slider sur la sant� maximale
        slider.maxValue = maxHealth;

        // Initialise la barre de vie � la sant� maximale
        slider.value = maxHealth;

        // Modifie la couleur de la barre de vie en fonction de la sant� maximale (couleur du gradient pour 100% de vie)
        fill.color = gradient.Evaluate(1f); // 1 correspond � 100% de la barre de vie
    }

    /// <summary>
    /// Met � jour la barre de vie avec la sant� actuelle et ajuste la couleur en fonction de la sant� restante.
    /// </summary>
    /// <param name="health">La sant� actuelle du personnage</param>
    public void SetHealth(int health)
    {
        // Met � jour la valeur du slider avec la sant� actuelle
        slider.value = health;

        // Change la couleur du remplissage de la barre en fonction de la sant� actuelle, normalis�e entre 0 et 1
        // normalizedValue renvoie un pourcentage de remplissage (entre 0 et 1) bas� sur la valeur actuelle et la valeur maximale
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
