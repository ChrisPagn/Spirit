using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ce script gère la barre de vie d'un personnage, en ajustant la valeur de la santé 
/// et en modifiant la couleur en fonction de la santé restante.
/// </summary>
public class HealthBar : MonoBehaviour
{
    // Slider qui représente la barre de vie
    public Slider slider;

    // Gradient utilisé pour changer la couleur de la barre de vie en fonction de la santé
    public Gradient gradient;

    // Image qui remplit la barre de vie et dont la couleur va changer
    public Image fill;

    /// <summary>
    /// Définit la santé maximale et initialise la barre de vie à cette valeur.
    /// </summary>
    /// <param name="maxHealth">La santé maximale du personnage</param>
    public void SetMaxHealth(int maxHealth)
    {
        // Définit la valeur maximale du slider sur la santé maximale
        slider.maxValue = maxHealth;

        // Initialise la barre de vie à la santé maximale
        slider.value = maxHealth;

        // Modifie la couleur de la barre de vie en fonction de la santé maximale (couleur du gradient pour 100% de vie)
        fill.color = gradient.Evaluate(1f); // 1 correspond à 100% de la barre de vie
    }

    /// <summary>
    /// Met à jour la barre de vie avec la santé actuelle et ajuste la couleur en fonction de la santé restante.
    /// </summary>
    /// <param name="health">La santé actuelle du personnage</param>
    public void SetHealth(int health)
    {
        // Met à jour la valeur du slider avec la santé actuelle
        slider.value = health;

        // Change la couleur du remplissage de la barre en fonction de la santé actuelle, normalisée entre 0 et 1
        // normalizedValue renvoie un pourcentage de remplissage (entre 0 et 1) basé sur la valeur actuelle et la valeur maximale
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
