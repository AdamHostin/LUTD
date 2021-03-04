using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Models
{
    public class Player
    {
        private int coins;
        private int vaccines;
        private int tempCost = 0;
        private bool canPlace = false;

        GameObject pickedUnitPrefab;
        Transform transparentUnitTransform;
        

        public class UpdatePlayerUIEvent : UnityEvent<int> { }
        public UpdatePlayerUIEvent updateCoinsUIEvent = new UpdatePlayerUIEvent();
        //from where payer gets theeese???
        public UpdatePlayerUIEvent updateVaccinesUIEvent = new UpdatePlayerUIEvent();

        public Player(int coins, int vaccines)
        {
            this.coins = coins;
            this.vaccines = vaccines;
        }

        public void ReInitPlayer(int coins, int vaccines)
        {
            this.coins = coins;
            this.vaccines = vaccines;
        }
        public void SetPlayerInfoUI(PlayerInfoPanelController playerInfoPanelController)
        {
            updateCoinsUIEvent.AddListener(playerInfoPanelController.UpdateCoinText);
            updateVaccinesUIEvent.AddListener(playerInfoPanelController.UpdateVaccineText);
            updateVaccinesUIEvent.Invoke(this.vaccines);
            updateCoinsUIEvent.Invoke(this.coins);
        }

        public void EarnCoins(int coins)
        {
            this.coins += coins;
            updateCoinsUIEvent.Invoke(this.coins);
        }

        public void SpendCoins(int coins)
        {
            this.coins -= coins;
            updateCoinsUIEvent.Invoke(this.coins);
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

