using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Ce script g�re l'inventaire, plus pr�cis�ment le compteur de pi�ces du joueur, 
/// et met � jour l'interface utilisateur en cons�quence.
/// </summary>
public class Inventory : MonoBehaviour
{
    // Nombre actuel de pi�ces
    public int coinsCount;

    // Texte standard pour afficher le nombre de pi�ces dans l'interface utilisateur (UnityEngine.UI)
    public Text coinsCountText;

    public List<Item> contentItems = new List<Item>();
    public int contentCurrentIndex = 0;

    // Texte pour TextMeshPro pour afficher le nombre de pi�ces dans l'interface utilisateur (TMPro)
    public TMP_Text coinsText;

    // Instance Singleton de l'inventaire
    public static Inventory instance;

    /// <summary>
    /// La m�thode Awake est appel�e lorsque le script est charg� pour la premi�re fois. 
    /// Elle initialise le Singleton de l'inventaire.
    /// </summary>
    private void Awake()
    {
        // V�rifie si une autre instance d'Inventory existe d�j�
        if (instance != null)
        {
            // Avertissement dans la console Unity si plus d'une instance est pr�sente
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la sc�ne!");

            // Supprime cette instance suppl�mentaire pour maintenir le mod�le Singleton
            Destroy(gameObject);
            return;
        }

        // Assigne cette instance comme �tant l'instance unique (Singleton)
        instance = this;
    }

   

    public void ConsumeItem()
    {
        Item currentItem = contentItems[contentCurrentIndex];
        PlayerHealth.instance.HealPlayer(currentItem.hpGiven);
        PlayerMovement.instance.moveSpeed += currentItem.speedGiven;
        contentItems.Remove(currentItem);
        GetNextItem();
    }

    public void GetNextItem()
    {
        contentCurrentIndex++;
        if (contentCurrentIndex > contentItems.Count - 1)
        {
            contentCurrentIndex = 0;
        }
    }

    public void GetPreviousItem()
    {
        contentCurrentIndex--;
        if (contentCurrentIndex < 0)
        {
            contentCurrentIndex = contentItems.Count - 1;
        }
    }


    /// <summary>
    /// M�thode pour ajouter un certain nombre de pi�ces � l'inventaire et mettre � jour l'interface utilisateur.
    /// </summary>
    /// <param name="count">Le nombre de pi�ces � ajouter</param>
    public void AddCoins(int count)
    {
        // Augmente le compteur de pi�ces
        coinsCount += count;

        // Si un texte standard (UnityEngine.UI) est assign�, met � jour son affichage avec le nouveau nombre de pi�ces
        if (coinsCountText != null)
        {
               UpdateTextUI();
        }

        // Si un texte TextMeshPro est assign�, met � jour son affichage avec le nouveau nombre de pi�ces
        if (coinsText != null)
        {
               UpdateTextUI();
        }
    }

    /// <summary>
    /// M�thode pour retirer un certain nombre de pi�ces de l'inventaire et mettre � jour l'interface utilisateur.
    /// </summary>
    /// <param name="count">Le nombre de pi�ces � retirer</param>
    public void RemoveCoins(int count)
    {
        // Diminue le compteur de pi�ces
        coinsCount -= count;

        // Si un texte standard (UnityEngine.UI) est assign�, met � jour son affichage avec le nouveau nombre de pi�ces
        if (coinsCountText != null)
        {
               UpdateTextUI();
          }

        // Si un texte TextMeshPro est assign�, met � jour son affichage avec le nouveau nombre de pi�ces
        if (coinsText != null)
        {
               UpdateTextUI();
        }
    }

     public void UpdateTextUI()
     {
          coinsText.text = coinsCount.ToString();
     }
}
