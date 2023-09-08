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
    { return GetAttack() + (int)(GetAttack() * attackPercentage); }

    public override int GetDefense()
    { return GetDefense() + (int)(GetDefense() * defensePercentage); }

    public override int GetHealth()
    { return GetHealth() + (int)(GetHealth() * healthPercentage); }

    public override int GetSpeed()
    { return GetSpeed() + (int)(GetSpeed() * speedPercentage); }

    public string GetName()
    { return itemName; }

    public void Upgrade() { }
}
