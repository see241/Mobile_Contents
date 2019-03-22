using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public int curTurn;

    public Image turnImage;
    public static EnemyController instance;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        StartBattle();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void StartBattle()
    {
        curTurn = 0;
    }

    public void StartEnemyTurn()
    {
        MsgQueue.instance.Push_Message(SetUiTrriger, 1f);
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            MsgQueue.instance.Push_Message(transform.GetChild(i).GetComponent<Enemy>().Action);
            transform.GetChild(i).GetComponent<Enemy>().curArmour = 0;
        }
        MsgQueue.instance.Push_Message(EndEnemyTurn);
    }

    private void SetUiTrriger()
    {
        turnImage.GetComponent<Animator>().SetTrigger("UI_PopUp");
    }

    private void EndEnemyTurn()
    {
        curTurn++;

        MsgQueue.instance.Push_Message(Player.instance.StartPlayerTurn);
    }

    private void PlayerBattleWin()
    {
        Debug.Log("이겼다!");
    }
}