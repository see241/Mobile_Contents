using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public int curMana;

    [HideInInspector]
    public float curArmour;

    [HideInInspector]
    public float curHp;

    public int maxMana;
    public float maxHp;
    public float armour;

    public float damage;
    public Slider hpBar;

    [Header("UI")]
    public Text manaText;

    public Image manaFill;
    public Image turnImage;
    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        StartPlayerTurn();
    }

    // Update is called once per frame
    private void Update()
    {
        UI_Update();
    }

    private void UI_Update()
    {
        hpBar.value = curHp / maxHp;
        manaFill.fillAmount = (float)curMana / (float)maxMana;
        manaText.text = curMana + "/" + maxMana;
    }

    public void GetDamage(float ad, ParticleSystem particle)
    {
        StartCoroutine(HealthLess(ad));
        ParticleSystem pt = Instantiate(particle);
        pt.transform.position = transform.position + new Vector3(0, 0, -1);
        Destroy(pt, pt.duration + 2f);
    }

    public void StartPlayerTurn()
    {
        Debug.Log("플레이어 턴 시작");
        curMana = maxMana;
        TouchManager.instance.isMyTurn = true;
        turnImage.GetComponent<Animator>().SetTrigger("UI_PopUp");
    }

    private void EndPlayerTurn()
    {
        Debug.Log("플레이어 턴 종료");
        //MsgQueue.instance.Push_Message(EnemyController.instance.StartEnemyTurn);
        EnemyController.instance.StartEnemyTurn();
    }

    public void EndPlayerTurnButton()
    {
        if (TouchManager.instance.isMyTurn)
        {
            TouchManager.instance.isMyTurn = false;
            MsgQueue.instance.Push_Message(EndPlayerTurn);
        }
    }

    private IEnumerator HealthLess(float damage)
    {
        float purDamage = damage / 25;
        for (int i = 0; i < 25; i++)
        {
            if (curArmour > 0)
            {
                curArmour -= purDamage;
                if (curArmour < 0)
                {
                    curHp += curArmour;
                }
            }
            else
            {
                curHp -= purDamage;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}