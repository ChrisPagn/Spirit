using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère les effets temporaires appliqués au joueur, tels que l'augmentation de vitesse.
/// </summary>
public class PlayerEffects : MonoBehaviour
{
    /// <summary>
    /// Augmente temporairement la vitesse du joueur.
    /// </summary>
    /// <param name="speedGiven">Valeur de l'augmentation de vitesse.</param>
    /// <param name="speedDuration">Durée de l'effet en secondes.</param>
    public void AddSpeed(int speedGiven, float speedDuration)
    {
        PlayerMovement.instance.moveSpeed += speedGiven;
        StartCoroutine(RemoveSpeed(speedGiven, speedDuration));
    }

    /// <summary>
    /// Réduit la vitesse du joueur après la fin de l'effet temporaire.
    /// </summary>
    /// <param name="speedGiven">Valeur de l'augmentation de vitesse à retirer.</param>
    /// <param name="speedDuration">Durée de l'effet avant suppression.</param>
    private IEnumerator RemoveSpeed(int speedGiven, float speedDuration)
    {
        yield return new WaitForSeconds(speedDuration);
        PlayerMovement.instance.moveSpeed -= speedGiven;
    }
}
