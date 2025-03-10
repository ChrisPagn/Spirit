using System.Runtime.ConstrainedExecution;
using UnityEngine;

/// <summary>
/// Repr�sente un dialogue dans le jeu, comprenant le nom du personnage et les phrases qu'il prononce.
/// </summary>
[System.Serializable]
public class Dialogue
{
    /// <summary>
    /// Nom du personnage qui parle dans le dialogue.
    /// </summary>
    public string name;

    /// <summary>
    /// Tableau de phrases prononc�es par le personnage dans le dialogue.
    /// Chaque phrase est un �l�ment du tableau.
    /// </summary>
    [TextArea(3, 10)]
    public string[] sentences;
}


//[System.Serializable] :
//Cet attribut permet � la classe Dialogue d'�tre s�rialis�e,
//ce qui signifie que ses instances peuvent �tre sauvegard�es et charg�es facilement.
//C'est utile pour stocker des donn�es dans des fichiers ou les transmettre sur un r�seau.

// [TextArea(3, 10)] :
// Cet attribut est utilis� pour indiquer � l'�diteur Unity que le champ sentences doit �tre
// affich� comme une zone de texte multiligne dans l'inspecteur, avec une hauteur minimale de 3 lignes
// et une hauteur maximale de 10 lignes. Cela facilite l'�dition de longues cha�nes de texte
// directement dans l'�diteur.