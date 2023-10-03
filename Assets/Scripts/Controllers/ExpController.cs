using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LevelUpController levelUpController;
    private int expNeeded = 20;

    public void CheckExp(Player player)
    {
        if (player.GetExperience() >= expNeeded)
        {
            gameManager.SwitchLevelUp();
            levelUpController.Show();
            expNeeded += expNeeded;
        }
    }

    public int GetExpNeeded()
    {
        return expNeeded;
    }

}
