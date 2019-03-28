using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        DrawCard(3);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    [ContextMenu("Draw")]
    public void DrawCard()
    {
        if (transform.GetChildCount() > 0)
        {
            if (SortManager.instance.transform.GetChildCount() <= 9)
            {
                int rand = Random.Range(0, transform.GetChildCount());
                GameObject randTarget = transform.GetChild(rand).gameObject;
                randTarget.transform.parent = GameObject.Find("SortManager").transform;
                StartCoroutine(MoveCenter(randTarget));
            }
            else
            {
                Player.instance.Talk("카드가 가득 찼어");
            }
        }
        else
        {
            Player.instance.Talk("덱에 카드가 부족해");
        }
    }

    public void DrawCard(int v)
    {
        for (int i = 0; i < v - 1; i++)
        {
            if (transform.GetChildCount() <= 1 || SortManager.instance.transform.GetChildCount() >= 8)
            {
                Player.instance.Talk("카드가 가득 찼어");
                break;
            }
            MsgQueue.instance.Push_Message(NoSortDraw, 0.25f);
        }
        MsgQueue.instance.Push_Message(DrawCard, 0.25f);
    }

    private void NoSortDraw()
    {
        if (transform.GetChildCount() > 0)
        {
            int rand = Random.Range(0, transform.GetChildCount());
            GameObject randTarget = transform.GetChild(rand).gameObject;
            randTarget.transform.parent = GameObject.Find("SortManager").transform;
            StartCoroutine(MoveCenter(randTarget, false));
        }
        else
        {
            Player.instance.Talk("덱에 카드가 부족해");
        }
    }

    private IEnumerator MoveCenter(GameObject target, bool trriger = true)
    {
        target.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        for (int i = 0; i < 30; i++)
        {
            target.transform.position = Vector3.Lerp(target.transform.position, new Vector3(0, 0, transform.GetChildCount() % 10), 0.1f);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.05f);
        if (trriger)
            SortManager.instance.Sort_Start();
    }
}