using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPos;

    private void Update() {

        float stoppingDistance = .1f;
        Vector3 currentPos = transform.position;
        if (Vector3.Distance(currentPos, targetPos) > stoppingDistance)
            {
                Vector3 moveDirection = (targetPos - transform.position).normalized;
                float moveSpeed = 4f;  
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
