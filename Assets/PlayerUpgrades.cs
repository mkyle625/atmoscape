using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Upgrade[] shieldPowerLevels = new Upgrade[5];
    [SerializeField] private Upgrade[] fuelPowerLevels = new Upgrade[5];

    void Start()
    {
        ApplyUpgrades();
    }
    
    private void ApplyUpgrades()
    {
        //Shield charge time
        switch (SaveManager.Instance.state.shieldPowerPurchased)
        {
            case 0: //No upgrade
                player.playerShield.shieldRechargeTime = shieldPowerLevels[0].UpgradeData;
                break;
            case 1: //Level 1
                player.playerShield.shieldRechargeTime = shieldPowerLevels[1].UpgradeData;
                break;
            case 2: //Level 2
                player.playerShield.shieldRechargeTime = shieldPowerLevels[2].UpgradeData;
                break;
            case 3: //Level 3
                player.playerShield.shieldRechargeTime = shieldPowerLevels[3].UpgradeData;
                break;
            case 4: //Level 4
                player.playerShield.shieldRechargeTime = shieldPowerLevels[4].UpgradeData;
                break;
        }
        
        //Fuel power upgrades
        switch (SaveManager.Instance.state.fuelPowerPurchased)
        {
            case 0: //No upgrade
                player.fuelCapacity = fuelPowerLevels[0].UpgradeData;
                break;
            case 1: //Level 1
                player.fuelCapacity = fuelPowerLevels[1].UpgradeData;
                break;
            case 2: //Level 2
                player.fuelCapacity = fuelPowerLevels[2].UpgradeData;
                break;
            case 3: //Level 3
                player.fuelCapacity = fuelPowerLevels[3].UpgradeData;
                break;
            case 4: //Level 4
                player.fuelCapacity = fuelPowerLevels[4].UpgradeData;
                break;
        }
    }

    [System.Serializable]
    private class Upgrade
    {
        [SerializeField] private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [SerializeField] private int _upgradeData;

        public int UpgradeData
        {
            get => _upgradeData;
            set => _upgradeData = value;
        }
    }
}
