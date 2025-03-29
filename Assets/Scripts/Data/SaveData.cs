using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Classe sérialisable représentant les données de sauvegarde du jeu.
/// Contient toutes les informations nécessaires pour sauvegarder et restaurer l'état du jeu.
/// </summary>
[System.Serializable]
public class SaveData
{
    /// <summary>
    /// Identifiant unique de l'utilisateur.
    /// </summary>
    public string UserId;

    /// <summary>
    /// Nom d'affichage de l'utilisateur.
    /// </summary>
    public string DisplayName;

    /// <summary>
    /// Nombre de pièces que possède le joueur.
    /// </summary>
    public int CoinsCount;

    /// <summary>
    /// Niveau le plus élevé atteint par le joueur.
    /// </summary>
    public int LevelReached;

    /// <summary>
    /// Liste des identifiants des objets dans l'inventaire du joueur.
    /// </summary>
    public List<int> InventoryItems;

    /// <summary>
    /// Liste des noms des objets dans l'inventaire du joueur.
    /// </summary>
    public List<string> InventoryItemsName;

    /// <summary>
    /// Date et heure de la dernière modification des données de sauvegarde.
    /// </summary>
    public DateTime LastModified;
}
