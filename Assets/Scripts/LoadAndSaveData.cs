using UnityEngine;

public class LoadAndSaveData : MonoBehaviour
{
     public static LoadAndSaveData instance;

     /// <summary>
     /// </summary>
     private void Awake()
     {
          if (instance != null)
          {
               Debug.LogWarning("Il y a plus d'une instance LoadAndSaveData dans la scène");
               return;
          }

          instance = this;
     }

 
     void Start()
    {
          Inventory.instance.coinsCount = PlayerPrefs.GetInt("coinsCount", 0);
          Inventory.instance.UpdateTextUI();

          
          //int currentHealth = PlayerPrefs.GetInt("playerHealth", PlayerHealth.instance.maxHealth);
          //PlayerHealth.instance.currentHealth = currentHealth;
          //PlayerHealth.instance.healthBar.SetHealth(currentHealth);
    }

    
     public void SaveData()
     {
          PlayerPrefs.SetInt("coinsCount",Inventory.instance.coinsCount);
          //PlayerPrefs.SetInt("playerHealth",PlayerHealth.instance.currentHealth);
     }
}
