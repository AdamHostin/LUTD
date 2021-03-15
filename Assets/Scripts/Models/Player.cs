using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


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

        public int vaccineEffectivnes;
        public PlayerState playerState = PlayerState.idle;

        public class UpdatePlayerUIEvent : UnityEvent<int> { }
        public UpdatePlayerUIEvent updateCoinsUIEvent = new UpdatePlayerUIEvent();
        //from where payer gets theeese???
        public UpdatePlayerUIEvent updateVaccinesUIEvent = new UpdatePlayerUIEvent();

        public Player(int coins, int vaccines , int vaccineEffectivnes)
        {
            this.coins = coins;
            this.vaccines = vaccines;
            this.vaccineEffectivnes = vaccineEffectivnes;
            
            
        }

        public void ReInitPlayer(int coins, int vaccines)
        {
            this.coins = coins;
            this.vaccines = vaccines;
        }

        public void StartVaccinating()
        {
            if (vaccines < 1) return;
            Debug.Log("Covid");
            DeleteTransparentUnit();
            ChangeState(PlayerState.vaccinating);
        }

        public void useVaccine()
        {
            vaccines--;
            updateVaccinesUIEvent.Invoke(vaccines);
            ChangeState(PlayerState.idle);
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
            DeleteTransparentUnit();
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
            DeleteTransparentUnit();
            ChangeState(PlayerState.idle);
        }

        public void SetUnitToRelocate(GameObject unit, GameObject transparentUnit)
        {
            DeleteTransparentUnit();
            pickedUnitPrefab = unit;
            this.transparentUnit = transparentUnit;
            playerState = PlayerState.relocating;
        }

        public void StopRelocating()
        {
            if (playerState == PlayerState.relocating)
                DeleteTransparentUnit();
            ChangeState(PlayerState.idle);
        }

        public GameObject GetPickedUnit()
        {
            return pickedUnitPrefab;
        }

        public void SetTransparentUnitPosition(Vector3 position)
        {
            transparentUnit.transform.position = position;
        }

        public void DeleteTransparentUnit()
        {
            if (transparentUnit)
                transparentUnit.transform.position = new Vector3(1000, 1000, 1000);
        }

        public void ChangeState(PlayerState targetState)
        {
            playerState = targetState;
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

