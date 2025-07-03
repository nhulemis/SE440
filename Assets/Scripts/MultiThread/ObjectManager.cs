using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace DefaultNamespace.MultiThread
{
    public class ObjectManager : MonoBehaviour
    {
        public static ObjectManager instance;
        
        public List<ObjectMover> objectsToMove;
        
        NativeArray<ObjectMoverData> objectMoverDataArray;
        TransformAccessArray transformAccessArray;
        public bool useJobSystem = true;
        
        public float waitForSeconds = 2f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void OnDestroy()
        {
            if (objectMoverDataArray.IsCreated)
            {
                objectMoverDataArray.Dispose();
            }
            if (transformAccessArray.isCreated)
            {
                transformAccessArray.Dispose();
            }
        }

        private void Update()
        {
            waitForSeconds -= Time.deltaTime;
            if (waitForSeconds <= 0)
            {
                if(useJobSystem)
                    UpdateByJob();
                else
                    UpdateByBatch();
            }
        }

        private void UpdateByJob()
        {
            var job = new ObjectMoverJob
            {
                objectMovers = objectMoverDataArray,
                deltaTime = Time.deltaTime
            };
            var jobHandle = job.Schedule(transformAccessArray);
            jobHandle.Complete();
        }

        private void UpdateByBatch()
        {
            for (int i = 0; i < objectsToMove.Count; i++)
            {
                objectsToMove[i].Tick();
            }
        }

        public void AddTo(ObjectMover objectMover)
        {
            objectsToMove.Add(objectMover);
            if (objectsToMove.Count % 1000 == 0)
            {
                if (objectMoverDataArray.IsCreated)
                {
                    objectMoverDataArray.Dispose();
                }
                if (transformAccessArray.isCreated)
                {
                    transformAccessArray.Dispose();
                }
                transformAccessArray = new TransformAccessArray(objectsToMove.Count);
                objectMoverDataArray = new NativeArray<ObjectMoverData>(objectsToMove.Count, Allocator.Persistent);
                for (int i = 0; i < objectsToMove.Count; i++)
                {
                    var obj = objectsToMove[i];
                    transformAccessArray.Add(obj.transform);
                    objectMoverDataArray[i] = new ObjectMoverData
                    {
                        startPosition = obj.StartPosition,
                        targetPosition = obj.TargetPosition // Random target position
                    };
                }
            }
        }

        private struct ObjectMoverData
        {
            public Vector3 startPosition;
            public Vector3 targetPosition;
        }
        
        [BurstCompile]
        private struct ObjectMoverJob : IJobParallelForTransform
        {
            public NativeArray<ObjectMoverData> objectMovers;
            public float deltaTime;
            public void Execute(int index, TransformAccess transform)
            {
                ObjectMoverData data = objectMovers[index];
                
                // Move the object towards the target position
                transform.position = Vector3.MoveTowards(transform.position, data.targetPosition, 2f * deltaTime);
                
                if (Vector3.Distance(transform.position, data.targetPosition) < 0.1f)
                {
                    transform.position = data.startPosition; // Reset to start position
                }
            }
        }
    }
}