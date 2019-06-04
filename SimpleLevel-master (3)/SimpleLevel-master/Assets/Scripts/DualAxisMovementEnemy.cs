﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualAxisMovementEnemy : BaseEnemy
{

    public List<Vector2> patrolPoints;
    [Tooltip("When this is turned on the enemy will reverse back through the patrol points after reaching the last point.")]
    public bool Trace;

    private int direction = 1;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        // If the enemy is set to patrol, but has no points, it can't patrol
        if (enemyMovementType == MovementPattern.Patrol && patrolPoints.Count < 1)
            enemyMovementType = MovementPattern.Stay;

        // If the enemy is set to Patrol and Chase, but has no patrol points, we set it to just chase
        if (enemyMovementType == MovementPattern.PatrolAndChase && patrolPoints.Count < 1)
            enemyMovementType = MovementPattern.Chase;
    }

    // Moves the player toward the destination based on their movement speed
    protected void MoveTowards(Vector2 destination)
    {
        enemyRenderer.flipX = destination.x > transform.localPosition.x;

        transform.localPosition = Vector2.MoveTowards(transform.localPosition, destination, movementSpeed * Time.deltaTime);
    }

    protected void Patrol()
    {
        MoveTowards(patrolPoints[patrolPointIndex]);

        // If we are at the destination, lets set the next destination
        if (Mathf.Abs(patrolPoints[patrolPointIndex].x - transform.localPosition.x) < .1 && Mathf.Abs(patrolPoints[patrolPointIndex].y - transform.localPosition.y) < .1)
        {
            patrolPointIndex += direction;

            // If we just arrived at the last point, start over
            if (patrolPointIndex == patrolPoints.Count)
            {
                if (Trace)
                {
                    direction = -1;
                    patrolPointIndex += direction;
                }
                else
                    patrolPointIndex = 0;
            }
            else if (patrolPointIndex == -1)
            {
                direction = 1;
                patrolPointIndex += direction;
            }
        }
    }
}