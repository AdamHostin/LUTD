using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBehaviour : UnitBehaviour
{
    private void Awake()
    {
        base.Awake();
    }

    protected override IEnumerator Shooting()
    { 
        model.ChangeState(UnitState.shooting);
        while (model.state == UnitState.shooting)
        {

            if (model.Shoot())
            {
                Debug.Log(model.target);
                if (model.target != null)
                {
                    transform.LookAt(model.VectorToLookAt(), gunPos.up);
                    animator.SetBool("Shoot", true);
                }

            }
            else
            {
                animator.SetBool("Shoot", false);
            }
            yield return new WaitForSeconds(timeBetweenHits);
        }
       
    }
}
