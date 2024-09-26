using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int coinsCount; // Compteur de pièces
    public Text coinsCountText; // Texte UI standard (UnityEngine.UI)
    public TMP_Text coinsText; // Texte UI pour TextMeshPro (TMPro)

    public static Inventory Instance; // Singleton pour Inventory

    // Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scène!");
            Destroy(gameObject); // Supprime cette instance pour éviter les doublons
            return;
        }

        Instance = this;
    }

    // Méthode pour ajouter des pièces
    public void AddCoins(int count)
    {
        coinsCount += count; // Incrémentation du compteur

        // Mise à jour du texte standard si assigné
        if (coinsCountText != null)
        {
            coinsCountText.text = coinsCount.ToString();
        }

        // Mise à jour du texte TextMeshPro si assigné
        if (coinsText != null)
        {
            coinsText.text = coinsCount.ToString();
        }
    }
}
