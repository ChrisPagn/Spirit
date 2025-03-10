using System.Runtime.ConstrainedExecution;
using UnityEngine;

/// <summary>
/// Représente un dialogue dans le jeu, comprenant le nom du personnage et les phrases qu'il prononce.
/// </summary>
[System.Serializable]
public class Dialogue
{
    /// <summary>
    /// Nom du personnage qui parle dans le dialogue.
    /// </summary>
    public string name;

    /// <summary>
    /// Tableau de phrases prononcées par le personnage dans le dialogue.
    /// Chaque phrase est un élément du tableau.
    /// </summary>
    [TextArea(3, 10)]
    public string[] sentences;
}


//[System.Serializable] :
//Cet attribut permet à la classe Dialogue d'être sérialisée,
//ce qui signifie que ses instances peuvent être sauvegardées et chargées facilement.
//C'est utile pour stocker des données dans des fichiers ou les transmettre sur un réseau.

// [TextArea(3, 10)] :
// Cet attribut est utilisé pour indiquer à l'éditeur Unity que le champ sentences doit être
// affiché comme une zone de texte multiligne dans l'inspecteur, avec une hauteur minimale de 3 lignes
// et une hauteur maximale de 10 lignes. Cela facilite l'édition de longues chaînes de texte
// directement dans l'éditeur.