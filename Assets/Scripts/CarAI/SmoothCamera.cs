using System;
using UnityEngine;

namespace DefaultNamespace.CarAI
{
     public class SmoothCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0, 5, -6);
        public float positionDamping = 5f;
        public float rotationDamping = 5f;

        private Vector3 velocity = Vector3.zero;
        void FixedUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position =
                Vector3.SmoothDamp(transform.position
                    , desiredPosition
                    , ref velocity
                    , 1f / positionDamping);

            Vector3 directionToTarget = (target.position - transform.position)
                .normalized;
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation
                        , targetRotation
                        , Time.deltaTime * rotationDamping);
            }
        }
    }
}