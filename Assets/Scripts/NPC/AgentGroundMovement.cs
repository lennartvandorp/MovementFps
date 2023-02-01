using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    [RequireComponent(typeof(AgentData))]
    public class AgentGroundMovement : MonoBehaviour
    {
        AgentData data;

        Rigidbody rb;

        AgentSenses senses;
        private void Start()
        {
            data = GetComponent<AgentData>();
            rb = GetComponent<Rigidbody>();
            senses = GetComponent<AgentSenses>();
            moveDirection = transform.forward;
        }

        public void SetDirection(Vector3 dir)
        {
            moveDirection = dir;
        }

        Vector3 moveDirection;
        private void FixedUpdate()
        {
            GoInDirection(moveDirection);
            CompensateGroundAngle();
        }

        /// <summary>
        /// Accelerates the agent towards a direction, but makes sure the agent doesnt go faster than a certain speed. 
        /// </summary>
        /// <param name="dir"></param>
        void GoInDirection(Vector3 dir)
        {
            dir = new Vector3(dir.x, 0f, dir.z);
            if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > data.MoveSpeed &&// to make sure the agent doesn't go faster than max speed
            Vector3.Dot(rb.velocity.normalized, dir.normalized) > 0)//So backwards inputs arent nullified
            {
                Vector3 horVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                Vector3 adjustedMoveDir = dir.normalized - Vector3.Dot(horVel.normalized, dir.normalized) * horVel.normalized;
                rb.AddForce(new Vector3(adjustedMoveDir.x, 0f, adjustedMoveDir.z) * data.Acceleration);
            }
            else
            {
                rb.AddForce(new Vector3(dir.x, 0f, dir.z) * data.Acceleration);
            }

            if (dir == Vector3.zero && senses.HitGround())
            {
                DoFriction();
            }
        }

        void DoFriction()
        {
            rb.velocity = new Vector3(rb.velocity.x * (Mathf.Pow(.99f, data.Friction)), rb.velocity.y, rb.velocity.z * (Mathf.Pow(.99f, data.Friction)));
        }

        Vector3 angledGroundForce = Vector3.zero;
        void CompensateGroundAngle()
        {
            rb.AddForce(-angledGroundForce / Time.fixedDeltaTime);
            angledGroundForce = Vector3.zero;
        }


        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider == senses.GroundCollider)
            {
                angledGroundForce = new Vector3(collision.impulse.x, 0f, collision.impulse.z);
            }
        }
    }
}