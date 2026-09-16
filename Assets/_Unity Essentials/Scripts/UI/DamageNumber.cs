using System;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageNumber : MonoBehaviour
{

    [SerializeField] private TMP_Text damageText;

    private Transform mainCameraPos;
    private FloatingDirection floatingDirection;
    private float floatingSpeed;
    private float fadeSpeed = 1f;


    private enum FloatingDirection
    {
        Up,
        UpLeft,
        UpUpLeft,
        UpLeftLeft,
        UpRight,
        UpUpRight,
        UpRightRight
    }

    void Start()
    {
        mainCameraPos = Camera.main.transform;

        floatingDirection = (FloatingDirection)Random.Range(0, System.Enum.GetValues(typeof(FloatingDirection)).Length);
        floatingSpeed = Random.Range(0.5f, 1f);
    }

    void Update()
    {
        Float();
        FadeAndDestroy();
    }

    void LateUpdate()
    {
        FaceCamera();
    }

    private void Float()
    {
        Vector3 direction = Vector3.zero;

        switch (floatingDirection)
        {
            case FloatingDirection.Up:
                direction = Vector3.up;
                break;
            case FloatingDirection.UpLeft:
                direction = Vector3.up + Vector3.left;
                break;
            case FloatingDirection.UpUpLeft:
                direction = Vector3.up * 2 + Vector3.left;
                break;
            case FloatingDirection.UpLeftLeft:
                direction = Vector3.up + Vector3.left * 2;
                break;
            case FloatingDirection.UpRight:
                direction = Vector3.up + Vector3.right;
                break;
            case FloatingDirection.UpUpRight:
                direction = Vector3.up * 2 + Vector3.right;
                break;
            case FloatingDirection.UpRightRight:
                direction = Vector3.up + Vector3.right * 2;
                break;
        }

        transform.position += direction.normalized * floatingSpeed * Time.deltaTime;
    }

    private void FadeAndDestroy()
    {
        damageText.alpha -= fadeSpeed * Time.deltaTime;

        if (damageText.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FaceCamera()
    {
        transform.rotation = mainCameraPos.rotation;
    }

    public void SetText(float damage)
    {
        damageText.text = damage.ToString();
    }
}
