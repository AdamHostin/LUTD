using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerBehaviour : TowerBehaviour
{
    protected override void Awake()
    {
        model = new FlamethrowerTowerUnit(hp, attack, range, gunPos.position, scaler, xpToNxtLvl, this);
    }

    protected override IEnumerator Shooting()
    {

        model.ChangeState(UnitState.shooting);
        while (model.state == UnitState.shooting)
        {
            if (((FlamethrowerTowerUnit)model).Shoot())
                App.audioManager.Play(shootSound);
            //TODO: spawn particles
            yield return new WaitForSeconds(timeBetweenHits);
        }
    }
}
