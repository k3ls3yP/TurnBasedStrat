using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 targetPos;
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;
    private Unit unit;

    private void Awake() {
        unit = GetComponent<Unit>();
        targetPos = transform.position;
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
    }
    public void Move(GridPosition gridPosition)
    {
        this.targetPos = LevelGrid.Instance.GetWorldPosition(gridPosition);;
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
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
}
