using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
    [RequireComponent(typeof(AgentData))]
    [RequireComponent(typeof(AgentGroundMovement))]
    public class ZombieFlocking : EnemyMovementLogic
    {

        AgentData data;
        AgentGroundMovement movement;
        override protected void Start()
        {
            data = GetComponent<AgentData>();
            movement = GetComponent<AgentGroundMovement>();
            base.Start();
        }

        public void UpdateDir()
        {
            Vector3 dir = (
            GetAverageSurroundingVel() * data.CohesionStrength
            + GoAwayFromObstacles() * data.ObstacleAvoidanceStrength
            + GoAwayFromZombies() * data.SeparationStrength
            + GoToZombies() * data.AlignmentStrength
            + FollowScent() * data.ScentStrength
            ).normalized;
            

            movement.SetDirection(dir);
        }

        Vector3 GoAwayFromObstacles()
        {
            Vector3 averageDir = Vector3.zero;
            foreach (Collider o in data.closeObstacles)
            {
                Vector3 closestPointDir = o.ClosestPointOnBounds(transform.position) - transform.position;
                if (closestPointDir.magnitude < data.ObstacleAvoidanceDist)
                {
                    averageDir -= (closestPointDir).normalized / closestPointDir.magnitude;

                }
            }
            return averageDir.normalized;
        }

        Vector3 GoAwayFromZombies()
        {
            Vector3 averageDir = Vector3.zero;
            foreach (Rigidbody z in data.closeZombies)
            {
                Vector3 closestPointDir = z.transform.position - transform.position;
                averageDir -= (closestPointDir).normalized / closestPointDir.magnitude;
            }
            return averageDir.normalized;
        }

        Vector3 GoToZombies()
        {
            Vector3 averageDir = Vector3.zero;
            foreach (Rigidbody z in data.closeZombies)
            {
                Vector3 closestPointDir = z.transform.position - transform.position;
                averageDir += (closestPointDir).normalized;
            }
            return averageDir.normalized;
        }


        Vector3 FollowScent()
        {
            if (data.scent)
            {
                if (!ScentObstructedByWall())
                {
                    if (Vector3.Dot(data.scent.forward, data.scent.forward - data.transform.position) < 0)//If haven't passed the scent point yet. 
                    {
                        return (data.scent.forward + (data.scent.position - data.transform.position).normalized).normalized;
                    }

                    return data.scent.forward;
                }
            }
            return Vector3.zero;
        }

        bool ScentObstructedByWall()
        {
            RaycastHit[] hits;
            Vector3 scentDir = data.scent.position - data.transform.position;
            hits = Physics.RaycastAll(data.transform.position, scentDir, scentDir.magnitude);

            bool toReturn = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Obstacle"))
                {
                    toReturn = true;
                }
            }
            return toReturn;
        }

        Vector3 GetAverageSurroundingVel()
        {
            Vector3 avgVel = new Vector3(0f, 0f, 0f);
            foreach (Rigidbody r in data.closeZombies)
            {
                avgVel += r.velocity;
                Debug.DrawLine(transform.position, r.transform.position);
            }
            if (data.closeZombies.Count == 0)
            {
                return Vector3.zero;
            }
            return avgVel.normalized;
        }

    }
}