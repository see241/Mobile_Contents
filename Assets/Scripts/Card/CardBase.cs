using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum CardState
{
    Armour, Effect, Attack
};

public class CardBase : MonoBehaviour
{
    [Header("에디터")]
    public string cardName;

    [TextArea]
    public string cardText;

    public Sprite cardImage;
    public int damage;
    public int armour;
    public int cardCost;
    public bool isAllAttack;
    public CardState state;
    public ParticleSystem particle;
    public Sprite[] sprites = new Sprite[3];

    [Header("마법카드만 작성")]
    public string effectName;

    public int value;

    public GameObject effectTarget;

    [Header("오브젝트")]
    [SerializeField]
    public TextMesh go_name;

    public TextMesh go_text;

    public TextMesh go_cost;

    public GameObject go_Image;

    private GameObject enemyController;

    private GameObject attackTarget;

    private void Awake()
    {
    }

    // Use this for initialization
    private void Start()
    {
        CardSetting();
        enemyController = GameObject.Find("EnemyController");

        go_name.color = Color.black;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void GetArmour()
    {
        Player.instance.GetArmour(armour);
    }

    private void AttackEnemy()
    {
        if (isAllAttack)
        {
            MsgQueue.instance.Push_Message(AllAttack);
        }
        if (!isAllAttack)
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        attackTarget.GetComponent<Enemy>().GetDamage(damage, particle);
    }

    private void AllAttack()
    {
        StartCoroutine("AttackAllEnemy");
    }

    public void CastCard(GameObject target = null)
    {
        Player.instance.curMana -= cardCost;
        attackTarget = target;
        StartCoroutine(CRT_CastCard());
    }

    [ContextMenu("CardSetting")]
    public void CardSetting()
    {
        go_Image.GetComponent<SpriteRenderer>().sprite = cardImage;
        go_name.text = cardName;
        go_text.text = cardText;
        go_cost.text = cardCost.ToString();
        GetComponent<SpriteRenderer>().sprite = sprites[(int)state];
    }

    private IEnumerator AttackAllEnemy()
    {
        for (int i = 0; i < enemyController.transform.GetChildCount(); i++)
        {
            attackTarget = enemyController.transform.GetChild(i).gameObject;
            attackTarget.GetComponent<Enemy>().GetDamage(damage, particle);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator CRT_CastCard()
    {
        switch (state)
        {
            case CardState.Armour:
                GetArmour();
                break;

            case CardState.Effect:
                GameObject.Find("SkillManager").GetComponent<Do_SomeThing>().CallEffect(effectName, value, effectTarget, particle);
                break;

            case CardState.Attack:
                AttackEnemy();
                break;
        }
        transform.parent = GameObject.Find("UsedCardPool").gameObject.transform;
        GetComponent<CardControl>().SetPhase("isUsed", true);
        yield return new WaitForSeconds(0.25f);
        transform.position = transform.parent.transform.position;
    }
}