using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    private GameObject touchTarget;
    private bool isTouching;
    private bool isCasting;
    public bool isMyTurn;
    public static TouchManager instance;

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
        if (isMyTurn)
            CardTouch();
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
                            touchTarget.transform.position = new Vector3(touchTarget.transform.position.x, touchTarget.transform.position.y, -8);
                        }
                    }
                }
                if (isTouching)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 temp_pos = Camera.main.ScreenToWorldPoint(touch.position);
                        touchTarget.transform.position = new Vector3(temp_pos.x, temp_pos.y, touchTarget.transform.position.z);
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
        if (touchTarget.transform.position.y > 0)
        {
            if (!touchTarget.GetComponent<CardBase>().isAllAttack)
            {
                GuideText.instance.Guide("대상을 지정하세요");
                touchTarget.transform.position = Vector2.zero;
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