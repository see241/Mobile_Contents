﻿using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    private GameObject touchTarget;
    private bool isTouching;

    [HideInInspector]
    public bool isCasting;

    public bool isMyturn;
    public float modifyY;
    public static TouchManager instance;

    private Vector2 touchPosition;

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
        if (GameManager.instance.eState == GameState.Dungeon)
        {
            if (isMyturn)
                CardTouch();
        }
    }

    private void CardTouch()
    {
        if (Input.touchCount == 1)
        {
            if (!isCasting)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 rayPos = new Vector3(touch.position.x, touch.position.y, 0);
                Ray ray = Camera.main.ScreenPointToRay(rayPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Card")
                    {
                        isTouching = true;
                        if (touch.phase == TouchPhase.Began)
                        {
                            touchTarget = hit.transform.gameObject;
                            touchTarget.GetComponent<CardControl>().SetPhase("isPopUp", true);
                            touchPosition = (Vector2)hit.point - (Vector2)touchTarget.transform.position;
                            touchTarget.GetComponent<CardControl>().isHolding = true;
                            touchTarget.transform.position = new Vector3(touchTarget.transform.position.x, touchTarget.transform.position.y, -9);
                        }
                    }
                }
                if (isTouching)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 temp_pos = Camera.main.ScreenToWorldPoint(touch.position);
                        touchTarget.transform.position = new Vector3(temp_pos.x, temp_pos.y, touchTarget.transform.position.z) - (Vector3)touchPosition;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (Player.instance.curMana - touchTarget.GetComponent<CardBase>().cardCost >= 0)
                        {
                            CastCard();
                        }
                        else
                        {
                            Player.instance.WarningBattery();
                            Player.instance.Talk("마나가 부족해");
                            CastCancle();
                        }
                    }
                }
            }
            if (isCasting)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 rayPos = new Vector3(touch.position.x, touch.position.y, 0);
                Ray ray = Camera.main.ScreenPointToRay(rayPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        touchTarget.GetComponent<CardBase>().CastCard(hit.collider.transform.parent.gameObject);
                        CastEnd();
                    }
                }
            }
        }
        if (Input.touchCount >= 2)
        {
            CastCancle();
        }
    }

    private void CastCard()
    {
        touchTarget.GetComponent<CardControl>().isHolding = false;
        if (touchTarget.transform.position.y > 0)
        {
            if (!touchTarget.GetComponent<CardBase>().isAllAttack)
            {
                GuideText.instance.Guide("Select_Target ");
                touchTarget.transform.position = new Vector3(0, 0, -9);
                isCasting = true;
            }
            else
            {
                touchTarget.GetComponent<CardBase>().CastCard();
                isCasting = false;
                isTouching = false;
                SortManager.instance.Sort_Start();
                touchTarget = null;
            }
        }
        else if (!isCasting)
        {
            CastCancle();
        }
    }

    private void CastCancle()
    {
        if (touchTarget != null)
        {
            isCasting = false;
            isTouching = false;
            touchTarget.GetComponent<CardControl>().ReturnOrigin();
            touchTarget.GetComponent<CardControl>().SetPhase("isPopUp", false);
            touchTarget.GetComponent<CardControl>().isHolding = false;
            touchTarget = null;
            GuideText.instance.DeleteText();
        }
    }

    private void CastEnd()
    {
        Debug.Log("CastEnd");
        Destroy(touchTarget.gameObject);
        isCasting = false;
        isTouching = false;
        GuideText.instance.DeleteText();
        SortManager.instance.Sort_Start();
    }
}