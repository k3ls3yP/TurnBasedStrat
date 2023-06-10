using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPos;
    [SerializeField] private Animator unitAnimator;
    private GridPosition gridPosition;


    private void Awake() {
        targetPos = transform.position;
    }
    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }
    private void Update() 
    {


        float stoppingDistance = .1f;
        Vector3 currentPos = transform.position;
        if (Vector3.Distance(currentPos, targetPos) > stoppingDistance)
            {
                Vector3 moveDirection = (targetPos - transform.position).normalized;
                float moveSpeed = 4f;  
                transform.position += moveDirection * moveSpeed * Time.deltaTime;

                float rotateSpeed = 10f;

                transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
                
                unitAnimator.SetBool("isWalking", true);
            }
        else
        {
            unitAnimator.SetBool("isWalking", false);
        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }

        
    }

    public void Move(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
