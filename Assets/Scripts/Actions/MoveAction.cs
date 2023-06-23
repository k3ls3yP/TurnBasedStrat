using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 targetPos;
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;


    protected override void Awake() {
        base.Awake();
        targetPos = transform.position;
    }
    
    private void Update() 
    {
        if(!isActive)
        {
            return;
        }
        Vector3 moveDirection = (targetPos - transform.position).normalized;
        float stoppingDistance = .1f;
        Vector3 currentPos = transform.position;
        if (Vector3.Distance(currentPos, targetPos) > stoppingDistance)
            {
                
                float moveSpeed = 4f;  
                transform.position += moveDirection * moveSpeed * Time.deltaTime;

                
                unitAnimator.SetBool("isWalking", true);
            }
        else
        {
            isActive = false;
            unitAnimator.SetBool("isWalking", false);
            onActionComplete();
        }
        
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        this.targetPos = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;

    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                if(unitGridPosition == testGridPosition)
                {
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }

        }
        
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
