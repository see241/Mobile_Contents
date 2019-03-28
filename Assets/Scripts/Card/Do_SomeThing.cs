using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Do_SomeThing : MonoBehaviour
{
    private Player player;
    private int value;

    private void Start()
    {
        player = Player.instance;
    }

    public void GetMana()
    {
        player.curMana += value;
        if (player.curMana > player.maxMana)
            player.curMana = player.maxMana;
    }

    private void DrawCard()
    {
        if (value > 1)
            DeckManager.instance.DrawCard(value);
        else
            DeckManager.instance.DrawCard();
    }

    public void CallEffect(string mathodName, int v, GameObject particleTarget = null, ParticleSystem particle = null)
    {
        value = v;
        Invoke(mathodName, 0f);
    }
}