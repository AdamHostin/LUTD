using UnityEngine;

namespace Models
{
    public class Base : IDamagable
    {
        private int hp;
        private BaseBehaviour behaviour;
        public Vector3 pos;


        public Base(int hp , Vector3 pos, BaseBehaviour behaviour)
        {
            this.hp = hp;
            this.pos = pos;
            this.behaviour = behaviour;
            App.levelManager.SetPlayerBase(this);
        }     

        //if base was destroyed return false
        public bool GetDamage(int attack, int infection = 0)
        {
            hp -= attack;
            if ( hp <= 0 )
            {
                if (behaviour != null) behaviour.DestroyBase();
                App.levelManager.EndLevel(false);
                return false;
            }

            return true;
        }

        public Vector3 GetPosition()
        {
            return pos;
        }



        public void UnsubscribeToDeathEvent(Enemy enemy)
        {

        }

        public void SubscribeToDeathEvent(Enemy enemy)
        {
            
        }
    }
}