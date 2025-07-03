using System;
using UnityEngine;

namespace DefaultNamespace.MultiThread
{
    public class ObjectMover : MonoBehaviour
    {
        private Vector3 _startPosition;
        private Vector3 _targetPosition;

        public Vector3 StartPosition => _startPosition;
        public Vector3 TargetPosition => _targetPosition;
        private void Start()
        {
            _startPosition = transform.position;
            var randomOffset = UnityEngine.Random.insideUnitSphere * 10;
            _targetPosition = _startPosition + randomOffset; 
            ObjectManager.instance.AddTo(this);
        }
        
        public void Tick()
        {
            // Move the object towards the target position
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, 2f * Time.deltaTime);
            if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            {
                transform.position = _startPosition;
            }
        }
    }
}