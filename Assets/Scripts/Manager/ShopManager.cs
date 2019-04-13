using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public GameObject textMesh;

    private void OnEnable()
    {
        SetItems();
    }

    private void OnDisable()
    {
        DeleteItem();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        BuyItem();
    }

    private void SetItems()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject item = Instantiate(items[Random.Range(0, items.Count)], transform);
            item.transform.position = new Vector3((i * 6) - 6, 15);
            GameObject itemCost = Instantiate(textMesh, item.transform);
            item.GetComponent<CardItem>().go_Cost = itemCost;
            itemCost.GetComponent<TextMesh>().text = item.GetComponent<CardItem>().cost + " Gold";
            itemCost.transform.position = (Vector2)(item.transform.position) - Vector2.down * 3;
        }
    }

    private void DeleteItem()
    {
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            Destroy(transform.GetChild(i).gameObject, 1f);
        }
    }

    private void BuyItem()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 rayPos = new Vector3(touch.position.x, touch.position.y, 0);
                Ray ray = Camera.main.ScreenPointToRay(rayPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Card")
                    {
                        if (GameManager.instance.curGold >= hit.collider.gameObject.GetComponent<CardItem>().cost)
                        {
                            GameManager.instance.curGold -= hit.collider.gameObject.GetComponent<CardItem>().cost;
                            hit.collider.gameObject.transform.parent = DeckManager.instance.transform;
                            hit.collider.gameObject.transform.position = DeckManager.instance.transform.position;
                            hit.collider.gameObject.GetComponent<CardItem>().ResetBase();

                            GameManager.instance.PopUpMsg("I bought " + hit.collider.GetComponent<CardBase>().cardName);
                        }
                        else
                            GameManager.instance.PopUpMsg("Not To Enugh Money");
                    }
                }
            }
        }
    }
}