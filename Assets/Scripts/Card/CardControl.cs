using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CardControl : MonoBehaviour
{
    [HideInInspector]
    public Vector3 originPos;

    [HideInInspector]
    public int cardIndex;

    [HideInInspector]
    public int totalCard;

    private Animator animator;

    // Use this for initialization
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Sort(int index, int total)
    {
        cardIndex = index;
        totalCard = total;
        if ((totalCard % 2) == 1)
        {
            originPos = new Vector3(((totalCard / 2) - cardIndex) * 1.6f, -4.5f, -cardIndex);
        }
        if ((totalCard % 2) == 0)
        {
            originPos = new Vector3((((totalCard / 2) - cardIndex) - 0.5f) * 1.6f, -4.5f, -cardIndex);
        }
        StartCoroutine(MoveToOrigin());
    }

    public void SetPhase(string key, bool value)
    {
        animator.SetBool(key, value);
    }

    public void ReturnOrigin()
    {
        transform.position = originPos;
    }

    private IEnumerator MoveToOrigin()
    {
        for (int i = 0; i < 20; i++)
        {
            transform.position = Vector3.Lerp(transform.position, originPos, 0.5f);
            yield return new WaitForSeconds(0.03f);
        }
        transform.position = originPos;
    }
}