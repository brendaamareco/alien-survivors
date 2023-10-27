using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class Healer : SpecialAreaBehaviour
{
    [SerializeField][Range(0, 1)] float healPercentage = 0f;
    public GameObject damageTextPrefab;
    
    public override void HandlePlayerExitArea(Player player)
    { }

    public override void HandlePlayerInArea(Player player)
    {
        float healingPoints = player.GetCurrentHealthPoints() + player.GetMaxHealthPoints() * healPercentage;
        player.Heal(healingPoints);
        Quaternion rotacion = Quaternion.identity;
        //Quaternion rotacion = Quaternion.Euler(30.304f, 0f, 0f);
        GameObject DamageTextInstance = Instantiate(damageTextPrefab, player.transform.position, rotacion);
        DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("+" + healingPoints.ToString());

        Debug.Log("Posición del jugador: " + player.transform.position);
        Debug.Log("Posición de DamageTextInstance: " + DamageTextInstance.transform.position);

        Destroy(gameObject);
    }
}
