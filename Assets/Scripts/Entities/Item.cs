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

    [SerializeField] int maxLevel = 10;
    [SerializeField] float upgradePercentage = 1f;

    private int level = 1;

    public override int GetAttack()
    { return (int)(attackPercentage); }

    public override int GetDefense()
    { return (int)(defensePercentage); }

    public override int GetHealth()
    { return (int)(healthPercentage); }

    public override int GetSpeed()
    { return (int)(speedPercentage); }

    public string GetName()
    { return itemName; }

    public void Upgrade() 
    {
        if (level < maxLevel)
        {
            level += 1;
            if (attackPercentage != 0) { attackPercentage += attackPercentage * upgradePercentage; }
            if (defensePercentage != 0) { defensePercentage += defensePercentage * upgradePercentage; }
            if (healthPercentage != 0) { healthPercentage += healthPercentage * upgradePercentage; }
            if (speedPercentage != 0) { speedPercentage += speedPercentage * upgradePercentage; }
        }
        else { Debug.Log("item al nivel maximo"); }
    }
}
