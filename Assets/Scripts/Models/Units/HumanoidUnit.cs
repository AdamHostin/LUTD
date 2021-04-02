using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class HumanoidUnit : Unit
{
    int toxicityResistance;
    int maxToxicityResistance;
    // behaviour;

    public HumanoidUnit(int hp, int toxicityResistance, int attack, float range, Vector3 gunPos, float scaler, List<int> xpToNxtLvl, HumanoidBehaviour behaviour)
        : base(hp, attack, range, gunPos, scaler, xpToNxtLvl, behaviour)
    {
        this.toxicityResistance = maxToxicityResistance = toxicityResistance;
        this.behaviour = behaviour;
        behaviour.toxicityBar.OnUIUpdate(0f, toxicityResistance, toxicityResistance);
    }

    public bool Vaccinating()
    {
        if (maxToxicityResistance == toxicityResistance) return false;

        toxicityResistance += App.player.vaccineEffectivnes;
        toxicityResistance = Mathf.Clamp(toxicityResistance, 0, maxToxicityResistance);
        UpdateToxicityBar();
        App.player.UseVaccine();

        return true;
    }

    void UpdateToxicityBar()
    {
        ((HumanoidBehaviour) behaviour).toxicityBar.OnUIUpdate(((float)(maxToxicityResistance - toxicityResistance) / maxToxicityResistance), toxicityResistance, maxToxicityResistance);
    }

    public override bool GetDamage(int damage, int infection = 0)
    {
        bool result = base.GetDamage(damage, infection);

        toxicityResistance -= infection;

        if (toxicityResistance <= 0)
        {
            if (behaviour != null)
            {
                onDamagableDeath.Invoke(this);
                ((HumanoidBehaviour)behaviour).GetInfected();
            }
            return false;
        }
        else
        {
            UpdateToxicityBar();
        }

        return result;
    }

    protected override void LevelUp()
    {
        base.LevelUp();

        //Improve toxicity resistance
        int newMaxToxicityResistance = (int)Mathf.Ceil((scaler * maxToxicityResistance));
        if (newMaxToxicityResistance > maxToxicityResistance) hp += (newMaxToxicityResistance - maxToxicityResistance);
        if (toxicityResistance > newMaxToxicityResistance) toxicityResistance = newMaxToxicityResistance;
        maxToxicityResistance = newMaxToxicityResistance;
        UpdateToxicityBar();
    }

}
