using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int coinsCount; // Compteur de pi�ces
    public Text coinsCountText; // Texte UI standard (UnityEngine.UI)
    public TMP_Text coinsText; // Texte UI pour TextMeshPro (TMPro)

    public static Inventory Instance; // Singleton pour Inventory

    // Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la sc�ne!");
            Destroy(gameObject); // Supprime cette instance pour �viter les doublons
            return;
        }

        Instance = this;
    }

    // M�thode pour ajouter des pi�ces
    public void AddCoins(int count)
    {
        coinsCount += count; // Incr�mentation du compteur

        // Mise � jour du texte standard si assign�
        if (coinsCountText != null)
        {
            coinsCountText.text = coinsCount.ToString();
        }

        // Mise � jour du texte TextMeshPro si assign�
        if (coinsText != null)
        {
            coinsText.text = coinsCount.ToString();
        }
    }
}
