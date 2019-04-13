using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private GameObject targetMap;

    public Map curMap = null;
    public int getGold { get { return (int)(curMapLevel * 3.5f) + (int)(Random.Range(1f, 7.0f)); } }
    private bool canSelect;

    public int curMapLevel;
    public int curGold;
    private IEnumerator SelectCoroutine;
    public static Character instance;

    private void Awake()
    {
        instance = this;
        SelectCoroutine = _SelectMap();
    }

    // Use this for initialization
    private void Start()
    {
        if (curMap == null)
        {
            StartCoroutine(SelectFirstMap());
        }
    }

    public void SecletMap()
    {
        StartCoroutine(_SelectMap());
        Debug.Log("Enable");
    }

    public void SecletDisable()
    {
        //StopCoroutine(SelectCoroutine);
        //Debug.Log("Disable");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private IEnumerator _SelectMap()
    {
        Debug.Log("Select Start");
        bool targeted = false;
        while (!targeted)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPos = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Map")
                    {
                        if (curMap.linkedMaps.Contains(hit.collider.gameObject))
                        {
                            curMap = hit.collider.gameObject.GetComponent<Map>();

                            curMapLevel = curMap.level;
                            targeted = true;
                            CameraControll.instance.gameLayer[4] = transform.position.y;
                            StartCoroutine(curMap.SelectMap());
                        }
                    }
                }
            }

            yield return null;
        }

        Debug.Log("Select End");
    }

    private IEnumerator SelectFirstMap()
    {
        bool targeted = false;
        while (!targeted)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPos = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Map")
                    {
                        if (hit.collider.gameObject.GetComponent<Map>().level == 0)
                        {
                            curMap = hit.collider.gameObject.GetComponent<Map>();
                            targeted = true;
                            curMapLevel = curMap.level;
                            StartCoroutine(curMap.SelectMap());
                        }
                    }
                }
            }

            yield return null;
        }

        Debug.Log("Select End");
    }

    public IEnumerator MoveTo(GameObject go)
    {
        for (int i = 0; i < 50; i++)
        {
            transform.position = Vector2.Lerp(transform.position, go.transform.position, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}