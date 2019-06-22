using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera myCam;
    private bool clicked;
    public GameObject effects;
    public Transform arrowHold, startPos;
    public LineRenderer lineRenderer;
    public float maxDistance = 7.5f;

    private void Start()
    {
        myCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
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
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, startPos.position + (startPos.up * maxDistance));
            }

            if (Input.GetButtonUp("Fire1"))
            {
                clicked = false;
                effects.SetActive(false);
            }
        }
    }
}