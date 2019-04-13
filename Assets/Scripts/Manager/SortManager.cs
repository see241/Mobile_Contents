using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortManager : MonoBehaviour
{
    private int totalCard;
    private int a;
    private GameObject card;
    public static SortManager instance;

    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Sort_Start()
    {
        StartCoroutine("Sort_Coroutine");
    }

    [ContextMenu("Sort")]
    private void CM_Sort()
    {
        totalCard = transform.GetChildCount();
        for (int i = 0; i < totalCard; i++)
        {
            transform.GetChild(i).GetComponent<CardControl>().Sort(i, totalCard);
        }
    }

    [ContextMenu("AllCardSetting")]
    private void AllCardSetting()
    {
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            transform.GetChild(i).GetComponent<CardBase>().CardSetting();
        }
    }

    private IEnumerator Sort_Coroutine()
    {
        yield return new WaitForFixedUpdate();
        totalCard = transform.GetChildCount();
        for (int i = 0; i < totalCard; i++)
        {
            transform.GetChild(i).GetComponent<CardControl>().Sort(i, totalCard);
            yield return null;
        }
    }
}