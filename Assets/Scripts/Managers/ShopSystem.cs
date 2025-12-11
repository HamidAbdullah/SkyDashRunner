using UnityEngine;
using System.Collections.Generic;

namespace SkyDashRunner.Managers
{
    [System.Serializable]
    public class ShopItem
    {
        public string id;
        public string name;
        public string description;
        public int price;
        public bool isUnlocked;
        public bool isPurchased;
        public Sprite icon;
    }
    
    public class ShopSystem : MonoBehaviour
    {
        public static ShopSystem Instance { get; private set; }
        
        [Header("Shop Items")]
        [SerializeField] private List<ShopItem> skins = new List<ShopItem>();
        [SerializeField] private List<ShopItem> particleTrails = new List<ShopItem>();
        [SerializeField] private List<ShopItem> powerUpUpgrades = new List<ShopItem>();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            LoadShopData();
        }
        
        public bool PurchaseItem(ShopItem item)
        {
            if (GameManager.Instance == null) return false;
            
            if (item.isPurchased)
            {
                // Equip item
                EquipItem(item);
                return true;
            }
            
            if (GameManager.Instance.Coins >= item.price)
            {
                GameManager.Instance.SpendCoins(item.price);
                item.isPurchased = true;
                EquipItem(item);
                SaveShopData();
                return true;
            }
            
            return false;
        }
        
        private void EquipItem(ShopItem item)
        {
            // Apply item effect (skin, trail, upgrade)
            Debug.Log($"Equipped: {item.name}");
            SaveShopData();
        }
        
        public List<ShopItem> GetSkins()
        {
            return skins;
        }
        
        public List<ShopItem> GetParticleTrails()
        {
            return particleTrails;
        }
        
        public List<ShopItem> GetPowerUpUpgrades()
        {
            return powerUpUpgrades;
        }
        
        private void SaveShopData()
        {
            // Save shop data to PlayerPrefs
            int index = 0;
            foreach (ShopItem item in skins)
            {
                PlayerPrefs.SetInt($"Skin_{index}_Purchased", item.isPurchased ? 1 : 0);
                index++;
            }
            
            index = 0;
            foreach (ShopItem item in particleTrails)
            {
                PlayerPrefs.SetInt($"Trail_{index}_Purchased", item.isPurchased ? 1 : 0);
                index++;
            }
            
            index = 0;
            foreach (ShopItem item in powerUpUpgrades)
            {
                PlayerPrefs.SetInt($"Upgrade_{index}_Purchased", item.isPurchased ? 1 : 0);
                index++;
            }
            
            PlayerPrefs.Save();
        }
        
        private void LoadShopData()
        {
            int index = 0;
            foreach (ShopItem item in skins)
            {
                item.isPurchased = PlayerPrefs.GetInt($"Skin_{index}_Purchased", 0) == 1;
                index++;
            }
            
            index = 0;
            foreach (ShopItem item in particleTrails)
            {
                item.isPurchased = PlayerPrefs.GetInt($"Trail_{index}_Purchased", 0) == 1;
                index++;
            }
            
            index = 0;
            foreach (ShopItem item in powerUpUpgrades)
            {
                item.isPurchased = PlayerPrefs.GetInt($"Upgrade_{index}_Purchased", 0) == 1;
                index++;
            }
        }
    }
}

