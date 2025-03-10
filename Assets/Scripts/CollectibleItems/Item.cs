using UnityEngine;

/// <summary>
/// Repr�sente un objet dans l'inventaire, avec des attributs tels que l'ID, le nom, la description, le prix, l'image, et les effets sur le joueur.
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    /// <summary>
    /// Identifiant unique de l'objet.
    /// </summary>
    public int id;

    /// <summary>
    /// Nom de l'objet.
    /// </summary>
    public string name;

    /// <summary>
    /// Description de l'objet.
    /// </summary>
    public string description;

    /// <summary>
    /// Prix de l'objet.
    /// </summary>
    public int price;

    /// <summary>
    /// Image associ�e � l'objet.
    /// </summary>
    public Sprite image;

    /// <summary>
    /// Points de vie rendus par l'objet lorsqu'il est utilis�.
    /// </summary>
    public int hpGiven;

    /// <summary>
    /// Bonus de vitesse accord� par l'objet lorsqu'il est utilis�.
    /// </summary>
    public int speedGiven;

    /// <summary>
    /// Dur�e pendant laquelle le bonus de vitesse est actif.
    /// </summary>
    public float speedDuration;
}
