using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void StartTurnCheat()
    {
        MsgQueue.instance.Push_Message(Player.instance.StartPlayerTurn);
    }
}