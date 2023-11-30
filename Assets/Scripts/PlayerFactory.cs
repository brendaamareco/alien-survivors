using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    private static PlayerFactory instance;

    private void Awake()
    {

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DestroyAllPlayers();
        }   
    }

    private Player LoadCharacter(string characterID)
    {
        GameObject[] charactersPrefab = Resources.LoadAll<GameObject>("Characters/");

        foreach (GameObject characterPrefab in charactersPrefab)
        {
            Player player = characterPrefab.GetComponent<Player>();

            if ( characterID == player.GetName())
            {
                GameObject[] weaponSlots = GameObject.FindGameObjectsWithTag("WeaponSlot");
                foreach(GameObject weaponSlot in weaponSlots)
                    weaponSlot.SetActive(false);

                GameObject character = Instantiate(characterPrefab, new Vector3(63, 0, 66), Quaternion.identity);
                Animator animator = character.GetComponent<Animator>();
                character.transform.Rotate(0, 180, 0);

                return player;
            }
        }

        return null;
    }

    public GameObject Create(string playerId)
    {
        DestroyAllPlayers();
        Player player = LoadCharacter(playerId);

        return player.gameObject;
    }

    private void DestroyAllPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject p in players)
            Destroy(p);
    }
}

