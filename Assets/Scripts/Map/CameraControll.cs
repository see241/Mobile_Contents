using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControll : MonoBehaviour
{
    public static CameraControll instance;
    public float speed;
    private float appllySpeed;
    private float lastdeltaY;
    public float[] gameLayer = new float[5] { 15, 0, 30, 45, 60 };
    public Image blackImage;
    public Image loadingImage;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        StartCoroutine(CameraFadeOut());
    }

    // Update is called once per frame
    private void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        if (GameManager.instance.eState == GameState.Map)
        {
            Vector2 movedPos = Vector2.zero;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                movedPos = Input.GetTouch(0).deltaPosition;
                appllySpeed = speed * movedPos.y;
                transform.Translate(0, -appllySpeed * Time.deltaTime, 0);
                lastdeltaY = movedPos.y;
                appllySpeed = lastdeltaY * speed;
            }
            else
            {
                appllySpeed *= 0.85f;
                transform.Translate(0, -appllySpeed * Time.deltaTime, 0);
            }
            if (transform.position.y < 0 + 60)
            {
                transform.position = new Vector3(transform.position.x, 0 + 60, transform.position.z);
            }
            if (transform.position.y > 23 + 60)
            {
                transform.position = new Vector3(transform.position.x, 23 + 60, transform.position.z);
            }
        }
    }

    public void SetLayer(int i)
    {
        transform.position = new Vector3(transform.position.x, gameLayer[i], transform.position.z);
    }

    public IEnumerator CameraFadeIn()
    {
        for (int i = 0; i < 50; i++)
        {
            blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, blackImage.color.a + 0.02f);
            loadingImage.color = new Color(loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, loadingImage.color.a + 0.02f);
            yield return new WaitForSeconds(0.01f);
        }

        blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, 1);
    }

    public IEnumerator CameraFadeOut()
    {
        for (int i = 0; i < 50; i++)
        {
            blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, blackImage.color.a - 0.02f);
            loadingImage.color = new Color(loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, loadingImage.color.a - 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator FadeInout()
    {
        yield return StartCoroutine(CameraFadeIn());
        yield return StartCoroutine(CameraFadeOut());
    }
}