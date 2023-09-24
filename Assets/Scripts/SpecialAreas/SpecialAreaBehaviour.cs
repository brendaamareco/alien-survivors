using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAreaBehaviour : MonoBehaviour
{
    public abstract void HandlePlayerExitArea(Player player);
    public abstract void HandlePlayerInArea(Player player);
}
