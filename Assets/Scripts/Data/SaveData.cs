using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public string UserId;
    public string DisplayName;
    public int CoinsCount;
    public int LevelReached;
    public List<int> InventoryItems;
    public List<string> InventoryItemsName;
    public DateTime LastModified; 
}
