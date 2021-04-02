using UnityEngine;
using UnityEngine.Events;

namespace Models
{
    public class Baricade : IDamagable , IPlacebla
    {
        private int hp;
        private int maxHp;
        private BaricadeBehaviour behaviour;
        public Vector3 pos;


        GameObject transparentSelf;
        TileBehaviour currentTile;

        public class DamagableDeathEvent : UnityEvent<IDamagable> { }
        public DamagableDeathEvent onDamagableDeath = new DamagableDeathEvent();

        public Baricade(int hp, Vector3 pos, BaricadeBehaviour behaviour)
        {
            this.hp = hp;
            maxHp = hp;
            this.pos = pos;
            this.behaviour = behaviour;

            behaviour.hpBar.OnUIUpdate(1f, hp, maxHp);

        }

        //if base was destroyed return false
        public bool GetDamage(int attack, int infection = 0)
        {
            hp -= attack;
            if (hp <= 0)
            {
                //App.audioManager.Stop("BaseNearFailState");
                if (behaviour != null) behaviour.DestroyBase();
                onDamagableDeath.Invoke(this);
                return false;
            }
            else
            {
                behaviour.hpBar.OnUIUpdate(((float)hp / maxHp), hp, maxHp);
            }

            return true;
        }

        public Vector3 GetPosition()
        {
            return pos;
        }



        public void UnsubscribeToDeathEvent(Enemy enemy)
        {
            onDamagableDeath.RemoveListener(enemy.OnDamagableInAwarenessRangeDeath);
        }

        public void SubscribeToDeathEvent(Enemy enemy)
        {
            onDamagableDeath.AddListener(enemy.OnDamagableInAwarenessRangeDeath);
        }

        public void SetTransparentSelf(GameObject transparentSelf)
        {
            this.transparentSelf = transparentSelf;
        }

        public GameObject GetTransparentSelf()
        {
            return transparentSelf;
        }

        public void SetCurrentTile(TileBehaviour tile)
        {
            this.currentTile = tile;
        }

        public TileBehaviour GetCurrentTile()
        {
            return currentTile;
        }

        public void SwitchToTile(TileBehaviour newTile)
        {
            currentTile.SetOccupied(false);
            currentTile = newTile;
            currentTile.SetOccupied(true);
        }
    }
}