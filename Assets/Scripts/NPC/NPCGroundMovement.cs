using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    [RequireComponent(typeof(NPCData))]
    public class NPCGroundMovement : MonoBehaviour
    {
        NPCData data;

        Rigidbody rb;
        private void Start()
        {
            data = GetComponent<NPCData>();
            rb = GetComponent<Rigidbody>();
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
        }

        /// <summary>
        /// Accelerates the agent towards a direction, but makes sure the agent doesnt go faster than a certain speed. 
        /// </summary>
        /// <param name="dir"></param>
        void GoInDirection(Vector3 dir)
        {
            if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > data.MoveSpeed &&// to make sure the agent doesn't go faster than max speed
            Vector3.Dot(rb.velocity.normalized, dir.normalized) > 0)
            {
                Vector3 adjustedMoveDir = dir.normalized - Vector3.Dot(rb.velocity.normalized, dir.normalized) * rb.velocity;
                rb.AddForce(new Vector3(adjustedMoveDir.x, rb.velocity.y, adjustedMoveDir.z));
            }
            else
            {
                rb.AddForce(new Vector3(dir.x, rb.velocity.y, dir.z) * Time.deltaTime * 150f);
            }
        }
    }
}