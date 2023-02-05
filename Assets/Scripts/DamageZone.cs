using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<CharController>();
        if (player != null)
        {
            player.DecreaseHealth(1);
            player.Respawn();
        }
    }
}
