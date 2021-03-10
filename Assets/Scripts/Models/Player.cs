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
        GameObject transparentUnit;

        private PlayerState playerState = PlayerState.idle;

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

        public void PlaceUnit(Vector3 position, TileBehaviour tile)
        {
            App.levelManager.InstatiateUnit(pickedUnitPrefab, position, transparentUnit, tile);
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

        public void StopRelocating()
        {
            if (playerState == PlayerState.relocating)
                DeleteTransparentUnit(true);
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

