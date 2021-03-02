using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public interface IDamagable
{
    bool GetDamage(int damage, int infection);
    Vector3 GetPosition();
    void SubscribeToDeathEvent(Enemy enemy);
    void UnsubscribeToDeathEvent(Enemy enemy);
}
