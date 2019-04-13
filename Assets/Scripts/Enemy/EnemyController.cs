using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public GameObject result;
    public Text resultText;

    [HideInInspector]
    public int curTurn;

    private bool isSpawned;

    private int ec;
    public int enemyCount { get { return ec; } set { ec = value; if (ec <= 0) WinCheck(); } }

    public Image turnImage;
    public static EnemyController instance;

    public GameObject[] enemys = new GameObject[2];

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
    }

    private void OnEnable()
    {
        StartBattle();
    }

    private void OnDisable()
    {
        resultText.text = null;
        result.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    { }

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

    public void InitEnemy()
    {
        resultText.text = null;
        result.SetActive(false);

        for (int i = 0; i < 2; i++)
        {
            GameObject enemy = Instantiate(enemys[Random.Range(0, enemys.Length)], transform);
            enemy.transform.position = new Vector3((i + 1) * 3, 1 + i * 0.5f, -2);
            enemyCount++;
        }
        Debug.Log("Init");
        isSpawned = true;
    }

    private void WinCheck()
    {
        Debug.Log("Win");
        result.SetActive(true);
        int getGold = Character.instance.getGold;
        GameManager.instance.gGold = getGold;
        resultText.text = "You Get " + getGold + " Gold";
    }
}