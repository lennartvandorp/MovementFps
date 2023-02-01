using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    public class AgentData : MonoBehaviour
    {
        [Header("Movement settings")]

        [SerializeField] float moveSpeed;
        public float MoveSpeed { get { return moveSpeed; } }

        [SerializeField] float acceleration;
        public float Acceleration { get { return acceleration; } }

        [SerializeField] float friction;
        public float Friction { get { return friction; } }

        [Header("Flocking settings")]
        [SerializeField] float obstacleAvoidanceDist;
        public float ObstacleAvoidanceDist { get { return obstacleAvoidanceDist; } }


        [HideInInspector] public List<Rigidbody> closeZombies;
        [HideInInspector] public List<Collider> closeObstacles;
        [HideInInspector] public Transform scent;

        [SerializeField] float cohesionStrength;
        [HideInInspector] public float CohesionStrength { get { return cohesionStrength; } }

        [SerializeField] float obstacleAvoidanceStrength;
        [HideInInspector] public float ObstacleAvoidanceStrength { get { return obstacleAvoidanceStrength; } }

        [SerializeField] float scentStrength;
        [HideInInspector] public float ScentStrength { get { return scentStrength; } }

        [SerializeField] float separationStrength;
        [HideInInspector] public float SeparationStrength { get { return separationStrength; } }


        [SerializeField] float alignmentStrength;
        [HideInInspector] public float AlignmentStrength { get { return alignmentStrength; } }


        private void Start()
        {
            closeZombies = new List<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Zombie")) { closeZombies.Add(other.attachedRigidbody); return; }
            if (other.CompareTag("Obstacle")) { closeObstacles.Add(other); return; }
            if (other.CompareTag("Scent")) { scent = other.transform; }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Zombie")) { closeZombies.Remove(other.attachedRigidbody); return; }
            if (other.CompareTag("Obstacle")) { closeObstacles.Remove(other); return; }
            if (other.CompareTag("Scent")) { if (other.transform == scent) { scent = null; } }
        }
    }
}