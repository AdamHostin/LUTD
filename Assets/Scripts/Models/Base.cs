using UnityEngine;

namespace Models
{
    public class Base : IDamagable
    {
        private int hp;
        private int maxHp;
        private BaseBehaviour behaviour;
        public Vector3 pos;

        bool wasHit = false;
        bool isNearFail = false;
        float nearFailPoint;

        public Base(int hp , Vector3 pos, BaseBehaviour behaviour)
        {
            this.hp = hp;
            maxHp = hp;
            this.pos = pos;
            this.behaviour = behaviour;
            App.levelManager.SetPlayerBase(this);
            behaviour.hpBar.OnUIUpdate(1f, hp, maxHp);
            nearFailPoint = App.audioManager.GetNearFailPerCent() * (((float)maxHp) / 100);
        }     

        //if base was destroyed return false
        public bool GetDamage(int attack, int infection = 0)
        {
            hp -= attack;
            if ( hp <= 0 )
            {
                App.audioManager.Stop("BaseNearFailState");
                if (behaviour != null) behaviour.DestroyBase();
                App.levelManager.EndLevel(false);
                return false;
            }
            else
            {
                behaviour.hpBar.OnUIUpdate(((float)hp / maxHp), hp, maxHp);
            }

            if (!isNearFail && hp < nearFailPoint)
            {
                App.audioManager.PlayLoop("BaseNearFailState");
                isNearFail = true;
            }

            if (!wasHit)
            {
                App.audioManager.Play("BaseUnderAttack");
                wasHit = true;
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