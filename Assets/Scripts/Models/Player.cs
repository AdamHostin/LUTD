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
        GameObject transparentUnit;

        private PlayerState playerState = PlayerState.idle;

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
            DeleteTransparentUnit(false);
            pickedUnitPrefab = prefab;
            if (cost <= coins)
            {
                playerState = PlayerState.placing;
                tempCost = cost;
                this.transparentUnit = transparentUnit;
            }
            else
            {
                playerState = PlayerState.idle;
            }
        }

        public void PlaceUnit(Vector3 position)
        {
            App.levelManager.InstatiateUnit(pickedUnitPrefab, position, transparentUnit);
            SpendCoins(tempCost);
            App.unitCardManager.SwitchToCard(null);
            DeleteTransparentUnit(true);
        }

        public void SetUnitToRelocate(GameObject unit, GameObject transparentUnit)
        {
            DeleteTransparentUnit(false);
            pickedUnitPrefab = unit;
            this.transparentUnit = transparentUnit;
            playerState = PlayerState.relocating;
        }

        public GameObject GetPickedUnit()
        {
            return pickedUnitPrefab;
        }

        public void SetTransparentUnitPosition(Vector3 position)
        {
            transparentUnit.transform.position = position;
        }

        public void DeleteTransparentUnit(bool changeState)
        {
            if (transparentUnit)
                transparentUnit.transform.position = new Vector3(1000, 1000, 1000);
            if (changeState)
                playerState = PlayerState.idle;
        }

        public bool ComparePlayerState(PlayerState state)
        {
            return playerState == state ? true : false;
        }

        public void SetPlayerState(PlayerState state)
        {
            playerState = state;
        }
    }
}

