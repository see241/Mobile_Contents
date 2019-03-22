using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBase : MonoBehaviour
{
    [Header("에디터")]
    public string cardName;

    public string cardText;
    public Sprite cardImage;
    public int damage;
    public int armour;
    public int cost;
    public bool isAllAttack;
    public ParticleSystem particle;

    [Header("오브젝트")]
    [SerializeField]
    public TextMesh go_name;

    public TextMesh go_text;
    public GameObject go_Image;

    private GameObject enemyController;

    // Use this for initialization
    private void Start()
    {
        CardSetting();
        enemyController = GameObject.Find("EnemyController");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void GetArmour()
    {
    }

    private void AttackEnemy()
    {
        if (isAllAttack)
        {
            MsgQueue.instance.Push_Message(AllAttack);
        }
    }

    private void AllAttack()
    {
        StartCoroutine("AttackAllEnemy");
    }

    public void CastCard()
    {
        StartCoroutine(CRT_CastCard());
        GameObject.Find("ManaText").GetComponent<Animator>().SetTrigger("HighLight");
        Player.instance.curMana -= cost;
    }

    [ContextMenu("CardSetting")]
    public void CardSetting()
    {
        go_Image.GetComponent<SpriteRenderer>().sprite = cardImage;
        go_name.text = cardName;
        go_text.text = cardText;
    }

    private IEnumerator AttackAllEnemy()
    {
        for (int i = 0; i < enemyController.transform.GetChildCount(); i++)
        {
            GameObject go_enemy = enemyController.transform.GetChild(i).gameObject;
            go_enemy.GetComponent<Enemy>().GetDamage(damage, particle);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator CRT_CastCard()
    {
        GetArmour();
        AttackEnemy();
        transform.parent = GameObject.Find("UsedCardPool").gameObject.transform;
        GetComponent<CardControl>().SetPhase("isUsed", true);
        yield return new WaitForSeconds(0.25f);
        transform.position = transform.parent.transform.position;
    }
}