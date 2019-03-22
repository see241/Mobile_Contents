using UnityEngine;

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
            originPos = new Vector3(((totalCard / 2) - cardIndex) * 1.6f, -5, cardIndex);
            transform.position = originPos;
        }
        if ((totalCard % 2) == 0)
        {
            originPos = new Vector3((((totalCard / 2) - cardIndex) - 0.5f) * 1.6f, -5, cardIndex);
            transform.position = originPos;
        }
    }

    public void SetPhase(string key, bool value)
    {
        animator.SetBool(key, value);
    }

    public void ReturnOrigin()
    {
        transform.position = originPos;
    }
}