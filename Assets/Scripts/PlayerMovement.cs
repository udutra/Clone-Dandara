using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera myCam;
    private Vector2 destinationPoint;
    private Animator anim;
    private bool clicked, foundPlatform;
    public GameObject effects, indicatorArrow;
    public Transform arrowHold, startPos;
    public LineRenderer lineRenderer;
    public float maxDistance = 7.5f;
    public float speed = 5f;
    public float distancePoint = 0.5f;
    public bool onGround;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        myCam = Camera.main;
    }

    private void Start()
    {
        onGround = true;
        destinationPoint = transform.position;
    }

    private void Update()
    {

        anim.SetBool("onGround", onGround);

        if (Input.GetButtonDown("Fire1") && onGround)
        {
            effects.SetActive(true);
            clicked = true;
        }

        if (clicked)
        {
            Vector2 mousePosition = myCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2) transform.position;

            arrowHold.up = direction;

            lineRenderer.SetPosition(0, startPos.position);

            RaycastHit2D hit = Physics2D.Raycast(startPos.position, startPos.up, maxDistance);

            if (hit.collider != null)
            {
                Platform platform = hit.collider.GetComponent<Platform>();

                if (platform != null)
                {
                    lineRenderer.SetPosition(1, hit.point);
                    destinationPoint = platform.Point(hit.point);
                    indicatorArrow.transform.position = platform.Point(hit.point) - ((Vector2) indicatorArrow.transform.up * 0.25f);
                    indicatorArrow.SetActive(true);
                    foundPlatform = true;
                }
                else
                {
                    lineRenderer.SetPosition(1, hit.point);
                    indicatorArrow.SetActive(false);
                    foundPlatform = false;
                }
                

            }
            else
            {
                lineRenderer.SetPosition(1, startPos.position + (startPos.up * maxDistance));
                indicatorArrow.SetActive(false);
                foundPlatform = false;
            }

            if(arrowHold.localEulerAngles.z <= 270 && arrowHold.localEulerAngles.z >= 90)
            {
                effects.SetActive(false);
                foundPlatform = false;
            }
            else
            {
                effects.SetActive(true);
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (foundPlatform)
                {
                    Flip(hit.transform.eulerAngles.z);
                    onGround = false;
                }
                foundPlatform = false;
                clicked = false;
                effects.SetActive(false);
            }
        }

        if (!onGround)
        {
            transform.position = Vector2.Lerp(transform.position, destinationPoint, speed * Time.deltaTime);
        }

        float distance = Vector2.Distance(transform.position, destinationPoint);

        if (distance <= distancePoint && !onGround)
        {
            onGround = true;
            transform.position += transform.up * 0.5f;
        }
    }

    private void Flip(float rot)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0,0,rot));
    }
}