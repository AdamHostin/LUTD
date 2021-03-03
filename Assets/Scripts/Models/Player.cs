using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Models
{
    public class Player
    {
        private int coins;
        private int vaccines;
        private int tempCost = 0;
        

        GameObject pickedUnitPrefab;
        Transform transparentUnitTransform;
        private bool canPlace = false;

        public Player(int coins, int vaccines)
        {
            this.coins = coins;
            this.vaccines = vaccines;
        }

        public void ReInitPlayer(DefaultPlayerValues defaultVals)
        {
            this.coins = defaultVals.coins;
            this.vaccines = defaultVals.vaccines;
        }
        

        public void EarnCoins(int coins)
        {
            this.coins += coins;
            Debug.Log("coins: " + this.coins);
        }

        public void SpendCoins(int coins)
        {
            this.coins -= coins;
            Debug.Log("coins: " + this.coins);
        }

        public void SetUnitPrefab(GameObject prefab, GameObject transparentUnit, int cost)
        {
            pickedUnitPrefab = prefab;
            if (cost <= coins)
            {
                canPlace = true;
                tempCost = cost;
                transparentUnitTransform = transparentUnit.GetComponent<Transform>();
            }
            else
            {
                canPlace = false;
            }
        }

        public void PlaceUnit(Vector3 position)
        {
            App.levelManager.InstatiateUnit(pickedUnitPrefab, position);
            SpendCoins(tempCost);
            canPlace = false;
            App.unitCardManager.SwitchToCard(null);
            DeleteTransparentUnit();
        }

        public void SetTransparentUnitPosition(Vector3 position)
        {
            transparentUnitTransform.position = position;
        }

        public void DeleteTransparentUnit()
        {
            transparentUnitTransform.position = new Vector3(1000, 1000, 1000);
            canPlace = false;
        }

        public bool CanPlace()
        {
            return canPlace;
        }
    }
}

