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
        private int medkits;
        private int tempCost = 0;
        private bool canPlace = false;

        GameObject pickedUnitPrefab;
        GameObject transparentUnit;

        public int vaccineEffectivnes;
        public int medkitEffectivness;
        public PlayerState playerState = PlayerState.idle;

        public class UpdatePlayerUIEvent : UnityEvent<int> { }
        public UpdatePlayerUIEvent updateCoinsUIEvent = new UpdatePlayerUIEvent();
        //from where payer gets theeese???
        public UpdatePlayerUIEvent updateVaccinesUIEvent = new UpdatePlayerUIEvent();
        public UpdatePlayerUIEvent updateMedkitsUIEvent = new UpdatePlayerUIEvent();

        public UnityEvent vaccinationEndedEvent = new UnityEvent();
        public UnityEvent healingEndedEvent = new UnityEvent();

        [HideInInspector] public PlayerBehaviour behaviour;

        public Player(int coins, int vaccines , int vaccineEffectivnes, int medkits, int medkitEffectivness)
        {
            this.coins = coins;
            this.vaccines = vaccines;
            this.vaccineEffectivnes = vaccineEffectivnes;
            this.medkits = medkits;
            this.medkitEffectivness = medkitEffectivness;
        }

        public void ReInitPlayer(int coins, int vaccines)
        {
            this.coins = coins;
            this.vaccines = vaccines;
        }

        public void StartVaccinating()
        {
            if ((vaccines < 1)||(playerState == PlayerState.vaccinating)) return;
            Debug.Log("Covid");
            DeleteTransparentUnit();
            ChangeState(PlayerState.vaccinating);
        }

        public void UseVaccine()
        {
            vaccines--;
            updateVaccinesUIEvent.Invoke(vaccines);
            ChangeState(PlayerState.idle);
            vaccinationEndedEvent.Invoke();
        }

        public void StartHealing()
        {
            if ((medkits < 1) || (playerState == PlayerState.healing)) return;
            Debug.Log("Heal");
            DeleteTransparentUnit();
            ChangeState(PlayerState.healing);
            Debug.Log("Healing started");
        }

        public void UseMedkit()
        {
            medkits--;
            ChangeState(PlayerState.idle);
            updateMedkitsUIEvent.Invoke(medkits);
            healingEndedEvent.Invoke();
        }

        public void SetPlayerInfoUI(PlayerInfoPanelController playerInfoPanelController)
        {
            updateCoinsUIEvent.AddListener(playerInfoPanelController.UpdateCoinText);
            updateVaccinesUIEvent.AddListener(playerInfoPanelController.UpdateVaccineText);
            updateMedkitsUIEvent.AddListener(playerInfoPanelController.UpdateMedkitsText);
            updateVaccinesUIEvent.Invoke(this.vaccines);
            updateCoinsUIEvent.Invoke(this.coins);
            updateMedkitsUIEvent.Invoke(this.medkits);
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
            vaccinationEndedEvent.Invoke();
            healingEndedEvent.Invoke();
            DeleteTransparentUnit();
            pickedUnitPrefab = prefab;
            if (cost <= coins)
            {
                ChangeState(PlayerState.placing);
                tempCost = cost;
                this.transparentUnit = transparentUnit;
            }
            else
            {
                ChangeState(PlayerState.idle);
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
            ChangeState(PlayerState.relocating);
        }

        public void StopRelocating()
        {
            if (playerState == PlayerState.relocating)
            {
                DeleteTransparentUnit();
                ChangeState(PlayerState.idle);
            }
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

            if (playerState == PlayerState.placing || playerState == PlayerState.relocating)
                behaviour.SetCanRotate(true);
            else
                behaviour.SetCanRotate(false);
        }

        public bool ComparePlayerState(PlayerState state)
        {
            return playerState == state ? true : false;
        }

        public void SetPlayerState(PlayerState state)
        {
            playerState = state;
        }

        public void SetCoins(int value)
        {
            coins = value;
        }

        public void SetVaccines(int value)
        {
            vaccines = value;
        }

        public void SetMedkits(int value)
        {
            medkits = value;
        }

        public int GetCoins() 
        {
            return coins;
        }

        public int GetVaccines()
        {
            return vaccines;
        }

        public int GetMedkits()
        {
            return medkits;
        }

        public void Rotate()
        {
            transparentUnit.transform.rotation *= Quaternion.Euler(new Vector3(0, 90, 0));
        }

        public GameObject GetTransparentUnit()
        {
            return transparentUnit;
        }
    }
}

