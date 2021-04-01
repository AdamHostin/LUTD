using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : UnitBehaviour
{
    protected override IEnumerator Shooting()
    {
        
        model.ChangeState(UnitState.shooting);
        while (model.state == UnitState.shooting)
        {

            if (model.Shoot())
            {
                App.audioManager.Play(shootSound);
                Debug.Log(model.target);
                if (model.target != null)
                {
                    transform.LookAt(model.VectorToLookAt(), gunPos.up);
                }
            }
            yield return new WaitForSeconds(timeBetweenHits);
        }       
    }

    public override void Die()
    {
        base.Die();
        //TODO: particles
        Destroy(gameObject);
    }
}
