using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private struct ActionTable
    {
        public int turn;
        public string key;
        public int value;
    }

    private static bool isLoadAction;
    public float curHp;
    public float maxHP;
    public float curArmour;
    public Slider hpBar;
    public string name;
    public ParticleSystem particle;
    public GameObject armourBar;
    public bool isFaded;
    private List<ActionTable> actions = new List<ActionTable>();
    private Animator animator;

    // Use this for initialization
    private void Start()
    {
        SetActionTable();
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        HpBarUpdate();
        if (curHp <= 0)
            Dead();
    }

    public void GetDamage(int ad, ParticleSystem particle)
    {
        if (curArmour > 0)
        {
            curArmour -= ad;
        }
        else
        {
            StartCoroutine(Healthless(ad));
        }
        if (curArmour < 0)
        {
            StartCoroutine(Healthless(-curArmour));
        }
        ParticleSystem pt = Instantiate(particle);
        pt.transform.position = transform.position + new Vector3(0, 0, -1);
        Destroy(pt.gameObject, pt.duration + 2f);
        Debug.Log("데미지를 " + ad + "만큼 입었습니다");
    }

    private void HpBarUpdate()
    {
        hpBar.value = (float)(curHp / maxHP);
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

    private void SetActionTable()
    {
        TextAsset textAsset = CashManager.instance.GetData(name);
        ParseSongTextAsset(textAsset);
    }

    private void ParseSongTextAsset(TextAsset asset)
    {
        List<string> list = new List<string>(asset.text.Split('\n'));
        for (int i = 0; i < list.Count; i++)
        {
            ProcessLine(list[i]);
        }
    }

    private void ProcessLine(string list)
    {
        if (list.StartsWith("$"))
        {
            string[] temp = list.Split(',');

            AddTable(int.Parse(temp[1]), temp[2], int.Parse(temp[3]));
        }
    }

    private void AddTable(int t, string k, int v)
    {
        ActionTable ac;
        ac.turn = t; ac.key = k; ac.value = v;
        actions.Add(ac);
    }

    public void Action()
    {
        int curTurn = EnemyController.instance.curTurn % actions.Count;

        switch (actions[curTurn].key)
        {
            case "Attack":

                Player.instance.GetDamage(actions[curTurn].value, particle);
                animator.SetTrigger("Attack");
                break;

            case "GetArmour":
                curArmour = actions[curTurn].value;
                StartCoroutine(FadeUp());
                isFaded = false;
                break;
        }
    }

    private void Dead()
    {
        Debug.Log("애니메이션 실행");
        Destroy(gameObject, 0.25f);
    }

    private IEnumerator Healthless(float damage)
    {
        float purDamage = damage / 20;
        for (int i = 0; i < 20; i++)
        {
            curHp -= purDamage;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator FadeUp()
    {
        while (armourBar.transform.localScale.y <= 0.22f)
        {
            armourBar.transform.localScale = new Vector3(armourBar.transform.localScale.x, armourBar.transform.localScale.y + 0.01f, armourBar.transform.localScale.z);
            yield return new WaitForSeconds(0.005f);
        }

        armourBar.transform.localScale = new Vector3(armourBar.transform.localScale.x, 0.23f, armourBar.transform.localScale.z);
    }

    private IEnumerator FadeOut()
    {
        while (armourBar.transform.localScale.y >= 0)
        {
            armourBar.transform.localScale = new Vector3(armourBar.transform.localScale.x, armourBar.transform.localScale.y - 0.01f, armourBar.transform.localScale.z);
            yield return new WaitForSeconds(0.005f);
        }
    }
}