using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject victoryScreen;
    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject.GetComponent<CharController>();
        if (player != null)
        {
            victoryScreen.SetActive(true);
        }
    }
}
