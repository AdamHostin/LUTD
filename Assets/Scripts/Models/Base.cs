using UnityEngine;

namespace Models
{
    public class Base
    {
        private int hp;
        public Vector3 pos;

        public Base(int hp , Vector3 pos )
        {
            this.hp = hp;
            this.pos = pos;
        }

        public void ResolveAttack(int attack)
        {
            hp -= attack;
            Debug.Log("Current hp: " + this.hp);
        }
    }
}