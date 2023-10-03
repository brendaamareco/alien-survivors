using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : StatsDecorator, IEntity
{
    [SerializeField] string itemName;
    [SerializeField] float attackPercentage = 0f;
    [SerializeField] float defensePercentage = 0f;
    [SerializeField] float healthPercentage = 0f;
    [SerializeField] float speedPercentage = 0f;

    [SerializeField] int maxLevel = 1;
    [SerializeField] float upgradePercentage = 0.25f;

    private int level = 1;

    public override int GetAttack()
    { return GetStats().GetAttack() + (int)(GetStats().GetAttack() * attackPercentage); }

    public override int GetDefense()
    { return GetStats().GetDefense() + (int)(GetStats().GetDefense() * defensePercentage); }

    public override int GetHealth()
    { return GetStats().GetHealth() + (int)(GetStats().GetHealth() * healthPercentage); }

    public override int GetSpeed()
    { return GetStats().GetSpeed() + (int)(GetStats().GetSpeed() * speedPercentage); }

    public string GetName()
    { return itemName; }

    public void Upgrade() 
    {
        level += 1;
        if (attackPercentage != 0) { attackPercentage += upgradePercentage; }
        if (defensePercentage != 0) { defensePercentage += upgradePercentage; }
        if (healthPercentage != 0) { healthPercentage += upgradePercentage; }
        if (speedPercentage != 0) { speedPercentage += upgradePercentage; }
    }
}
