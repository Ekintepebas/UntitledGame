using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 mouseStart;
    private Vector3 mouseEnd;

    private MonsterRigidy monster;


    public LineRenderer lineRenderer;
    public LineRenderer forceFieldRenderer; 

    private bool isDrawing = false;

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        forceFieldRenderer.positionCount = 1;
        forceFieldRenderer.enabled = false; 
    }


    void Update()
    {
        //Left Click
        if (Input.GetMouseButtonDown(0) && !isDrawing)
        {
            Debug.Log("Left");
            mouseStart = Input.mousePosition;
            mouseStart.z = 15.0f;
            isDrawing = true;

            StartVector(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (isDrawing)
        {

            if (Input.GetMouseButtonDown(0))
            {
                ReleaseForce();
                isDrawing = false;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                isDrawing = false;
                lineRenderer.enabled = false;
                forceFieldRenderer.enabled = false;
            }

            mouseEnd = Input.mousePosition;

            // end of line is displayed here
            Vector3 lineVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineVector.z = 15f;
            lineRenderer.SetPosition(1, lineVector);

            // start of force field display
            
            forceFieldRenderer.enabled = true;
            Vector3 forceVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            forceVector.z = 15f; 
            forceFieldRenderer.SetPosition(0, forceVector);
            ForceDisplay();

        }
        else
        {
            forceFieldRenderer.positionCount = 1;
            forceFieldRenderer.enabled = false;
        }
        
    }

    private void ReleaseForce()
    {

        Vector3 forceVector = new Vector3((mouseEnd.x - mouseStart.x), (mouseEnd.y - mouseStart.y));
        CastRay(Camera.main.ScreenToWorldPoint(mouseEnd), forceVector,2);
        //monster.ReceiveForce(forceVector);
    }

    private void ForceDisplay()
    {
        Vector3 forceVector = new Vector3((mouseEnd.x - mouseStart.x), (mouseEnd.y - mouseStart.y));
        forceVector.z = 15f;
        CastRay2(Camera.main.ScreenToWorldPoint(mouseEnd), forceVector, 2, 1);
    }

    private void StartVector(Vector3 vector)
    {
        vector.z = 15f;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, vector);
    }

    private void CastRay(Vector2 start,Vector3 direction,int bouncesLeft)
    {
        if (bouncesLeft == 0)
            return;

        Debug.Log(start);
        Debug.Log(direction);
        RaycastHit2D hit = Physics2D.Raycast(start, direction);
        if (hit.collider.CompareTag("Ball"))
        {
            monster = hit.collider.GetComponent<MonsterRigidy>();
            monster.ReceiveForce(direction);
            
            return;
        }
        Vector3 newDirection = Vector3.Reflect(direction, hit.normal);
        CastRay(hit.point, newDirection, bouncesLeft - 1);
    }

    private void CastRay2(Vector2 start, Vector3 direction, int bouncesLeft, int bounce)
    {   
        if (bouncesLeft == 0)
            return;
        RaycastHit2D hit = Physics2D.Raycast(start, direction);
        if(hit.collider != null)
        {
            forceFieldRenderer.positionCount = bounce + 1;
        
            if (hit.collider.CompareTag("Ball"))
            {
                Vector3 forceVector = hit.point;

                forceFieldRenderer.SetPosition(bounce, forceVector);

                return;
            }
            if (hit.collider.CompareTag("Borders"))
            {
            
                Vector3 newDirection = Vector3.Reflect(direction, hit.normal);
                forceFieldRenderer.SetPosition(bounce, hit.point);
                CastRay2(hit.point, newDirection, bouncesLeft - 1, bounce + 1);
            }
        }



    }
}
