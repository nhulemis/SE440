using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class CarController : MonoBehaviour
    {
        public enum WheelType
        {
            FrontLeft,
            FrontRight,
            RearLeft,
            RearRight
        }
        
        [Serializable]
        public struct Wheel
        {
            public Transform wheelTransform;
            public WheelCollider wheelCollider;
            public WheelType wheelType;
            public float maxSteerAngle;
            public float motorTorque;
        }
        
        [SerializeField] private List<Wheel> wheels;
        
        [SerializeField] private float maxSpeed = 100f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float brakeForce = 50f;

        [SerializeField] private bool canControl;
        
        private float currentSpeed = 0f;
        
        private void Update()
        {
            HandleInput();
            UpdateWheels();
        }

        private void UpdateWheels()
        {
            foreach (var wheel in wheels)
            {
                // Update wheel rotation
                float rotationAngle = currentSpeed * Time.deltaTime;
                wheel.wheelTransform.Rotate(Vector3.right, rotationAngle);
                
                // Update wheel position
                Vector3 position;
                Quaternion rotation;
                wheel.wheelCollider.GetWorldPose(out position, out rotation);
                wheel.wheelTransform.position = position;
                wheel.wheelTransform.rotation = rotation;
            }
        }

        public void ApplyInput(float horizontalInput, float verticalInput)
        {
            // Steer front wheels
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.steerAngle = horizontalInput * wheel.maxSteerAngle;
                float speed = verticalInput * wheel.motorTorque * Time.deltaTime * acceleration; 
                currentSpeed += speed;
                currentSpeed = Mathf.Clamp(currentSpeed, -10f, maxSpeed);
                wheel.wheelCollider.motorTorque = currentSpeed;
            }
            // Apply brake force
            if (Input.GetKey(KeyCode.Space))
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = brakeForce;
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0f;
                }
            }
        }

        private void HandleInput()
        {
            if(!canControl) return;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            ApplyInput(horizontalInput, verticalInput);
        }
    }
}