using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// G�re les effets temporaires appliqu�s au joueur, tels que l'augmentation de vitesse.
/// </summary>
public class PlayerEffects : MonoBehaviour
{
    /// <summary>
    /// Augmente temporairement la vitesse du joueur.
    /// </summary>
    /// <param name="speedGiven">Valeur de l'augmentation de vitesse.</param>
    /// <param name="speedDuration">Dur�e de l'effet en secondes.</param>
    public void AddSpeed(int speedGiven, float speedDuration)
    {
        PlayerMovement.instance.moveSpeed += speedGiven;
        StartCoroutine(RemoveSpeed(speedGiven, speedDuration));
    }

    /// <summary>
    /// R�duit la vitesse du joueur apr�s la fin de l'effet temporaire.
    /// </summary>
    /// <param name="speedGiven">Valeur de l'augmentation de vitesse � retirer.</param>
    /// <param name="speedDuration">Dur�e de l'effet avant suppression.</param>
    private IEnumerator RemoveSpeed(int speedGiven, float speedDuration)
    {
        yield return new WaitForSeconds(speedDuration);
        PlayerMovement.instance.moveSpeed -= speedGiven;
    }
}
