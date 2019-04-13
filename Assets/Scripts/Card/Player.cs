using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public int curMana;

    private int curTurn;

    [HideInInspector]
    public float curArmour;

    public float curHp;

    public int maxMana;
    public float maxHp;
    public float armour;

    public float damage;
    public Slider hpBar;
    public GameObject armourBar;
    private bool isFaded;

    [Header("UI")]
    public Image manaFill;

    public Image manaBackGround;
    public Image turnImage;
    public GameObject dialogBox;
    private bool isTalking;
    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
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

        if (curArmour > 0)
        {
            armourBar.transform.GetChild(0).GetComponent<TextMesh>().text = curArmour.ToString();
        }
        else
        {
            if (!isFaded)
            {
                StartCoroutine(FadeOut());
                isFaded = true;
            }
        }
    }

    public void GetDamage(float ad, ParticleSystem particle)
    {
        if (curArmour > 0)
        {
            curArmour -= ad;
        }
        else
        {
            StartCoroutine(HealthLess(ad));
        }
        if (curArmour < 0)
        {
            StartCoroutine(HealthLess(-curArmour));
        }
        ParticleSystem pt = Instantiate(particle);
        pt.transform.position = transform.position + new Vector3(0, 0, -1);
        Destroy(pt.gameObject, pt.duration + 2f);
    }

    public void GetArmour(int v)
    {
        curArmour += v;
        StartCoroutine(FadeUp());
        isFaded = false;
    }

    public void StartPlayerTurn()
    {
        Debug.Log("플레이어 턴 시작");
        StartCoroutine(FadeOut());
        curArmour = 0;
        curMana = maxMana;
        TouchManager.instance.isMyturn = true;
        if (curTurn > 0)
            MsgQueue.instance.Push_Message(DeckManager.instance.DrawCard);
        turnImage.GetComponent<Animator>().SetTrigger("UI_PopUp");
        curTurn++;
    }

    private void EndPlayerTurn()
    {
        Debug.Log("플레이어 턴 종료");
        //MsgQueue.instance.Push_Message(EnemyController.instance.StartEnemyTurn);
        EnemyController.instance.StartEnemyTurn();
    }

    public void EndPlayerTurnButton()
    {
        if (TouchManager.instance.isMyturn && !TouchManager.instance.isCasting)
        {
            TouchManager.instance.isMyturn = false;
            MsgQueue.instance.Push_Message(EndPlayerTurn);
        }
    }

    public void WarningBattery()
    {
        StartCoroutine(_WarningBattery());
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

    public void Talk(string str)
    {
        if (!isTalking)
        {
            StartCoroutine(_Talk(str));
        }
    }

    public void InitPlayer()
    {
        TouchManager.instance.isMyturn = true;
        curTurn = 0;
    }

    public IEnumerator _WarningBattery()
    {
        for (int i = 0; i < 2; i++)
        {
            manaBackGround.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            manaBackGround.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator _Talk(string str)
    {
        isTalking = true;
        dialogBox.transform.GetChild(0).GetComponent<TextMesh>().text = str;
        for (int i = 0; i < 30; i++)
        {
            dialogBox.transform.localScale = Vector3.Lerp(dialogBox.transform.localScale, Vector3.one * 0.6f, 0.25f);
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < 30; i++)
        {
            dialogBox.transform.localScale = Vector3.Lerp(dialogBox.transform.localScale, Vector3.zero, 0.25f);
            yield return new WaitForSeconds(0.02f);
        }
        dialogBox.transform.localScale = Vector3.zero;
        dialogBox.transform.GetChild(0).GetComponent<TextMesh>().text = null;
        isTalking = false;
    }

    private IEnumerator FadeUp()
    {
        while (armourBar.transform.localScale.y < 0.6f)
        {
            armourBar.transform.localScale = new Vector3(armourBar.transform.localScale.x, armourBar.transform.localScale.y + 0.03f, armourBar.transform.localScale.z);
            yield return new WaitForSeconds(0.005f);
        }

        armourBar.transform.localScale = new Vector3(armourBar.transform.localScale.x, 0.6f, armourBar.transform.localScale.z);
    }

    private IEnumerator FadeOut()
    {
        while (armourBar.transform.localScale.y > 0)
        {
            armourBar.transform.localScale = new Vector3(armourBar.transform.localScale.x, armourBar.transform.localScale.y - 0.03f, armourBar.transform.localScale.z);
            yield return new WaitForSeconds(0.005f);
        }
        armourBar.transform.localScale = new Vector3(armourBar.transform.localScale.x, 0f, armourBar.transform.localScale.z);
    }
}