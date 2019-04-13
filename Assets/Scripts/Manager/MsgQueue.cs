using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgQueue : MonoBehaviour

{
    public delegate void Del();

    private struct Massage
    {
        public Del del;
        public float delay;
    }

    private Queue<Massage> dels = new Queue<Massage>();

    public float msgDelay;

    public static MsgQueue instance;

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
    }

    public void Push_Message(Del del, float d = 0.75f)
    {
        Massage msg;
        msg.del = del; msg.delay = d;
        dels.Enqueue(msg);
    }

    public IEnumerator MessageQueu()
    {
        while (true)
        {
            if (dels.Count > 0)
            {
                dels.Peek().del();
                yield return new WaitForSeconds(dels.Dequeue().delay);
            }
            else
            {
                yield return null;
            }
        }
    }
}