using UnityEngine;

namespace Models
{
    public class Base
    {
        private int hp;
        private BaseBehaviour behaviour;
        public Vector3 pos;


        public Base(int hp , Vector3 pos, BaseBehaviour behaviour)
        {
            this.hp = hp;
            this.pos = pos;
            this.behaviour = behaviour;
        }

        //if base was destroyed return false
        public bool ResolveAttack(int attack)
        {
            hp -= attack;
            if ( hp <= 0 )
            {
                if (behaviour != null) behaviour.DestroyBase();
                App.levelManager.SetPlayerBase(null);
                return false;
            }

            return true;
        }
    }
}