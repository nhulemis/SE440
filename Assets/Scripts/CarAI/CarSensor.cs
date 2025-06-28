using UnityEngine;

namespace DefaultNamespace.CarAI
{
    public class CarSensor : MonoBehaviour
    {
        public float forwardDistance = 10f;
        public float sideAngle = 30f;

        public float forwardSpeed = 1f;
        public float sideSpeed = 1f;
        public int sideSensorCount = 5; // Number of side sensors
        
        public CarController carController;
        
        private void FixedUpdate()
        {
            RaycastHit hit;
            Vector3 forwardDirection = transform.forward * forwardDistance;
            float forwardSpeedAdjusted = forwardSpeed;
            float sideSpeedAdjusted = 0f;
            bool isDetectedObstacle = false;
            // Forward sensor
            if (Physics.Raycast(transform.position, forwardDirection, out hit, forwardDistance))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                Debug.Log("Forward obstacle detected at: " + hit.point);
                isDetectedObstacle = true;
            }
            else
            {
                forwardSpeedAdjusted = forwardSpeed;
                Debug.DrawRay(transform.position, forwardDirection, Color.green);
            }

            // Side sensors
            for (int i = -sideSensorCount; i <= sideSensorCount; i++)
            {
                if (i == 0) continue; // Skip the forward direction
                Vector3 sideDirection = Quaternion.Euler(0, i * sideAngle, 0) * forwardDirection;
                if (Physics.Raycast(transform.position, sideDirection, out hit, forwardDistance))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    Debug.Log("Side obstacle detected at: " + hit.point);
                    isDetectedObstacle = true;
                    sideSpeedAdjusted = i * sideSpeed * -1f;
                    break;
                }
                else
                {
                    Debug.DrawRay(transform.position, sideDirection, Color.green);
                    sideSpeedAdjusted = sideSpeed;
                }
            }
            
            if (isDetectedObstacle)
            {
                // If an obstacle is detected, adjust the car's speed
                carController.ApplyInput(sideSpeedAdjusted , forwardSpeedAdjusted);
            }
            else
            {
                // If no obstacles are detected, maintain the forward speed
                carController.ApplyInput(0f , forwardSpeed);
            }
        }
    }
}