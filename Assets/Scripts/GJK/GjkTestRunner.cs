using System;
using UnityEngine;

namespace GJK
{
    public class GjkTestRunner : MonoBehaviour
    {
        public GjkPolygonShapeCollider[] shapes;

        private void Update()
        {
            if (shapes == null || shapes.Length < 2)
            {
                Debug.LogWarning("Please assign at least two GjkPolygonShapeCollider components to the shapes array.");
                return;
            }

            for (int i = 0; i < shapes.Length; i++)
            {
                for (int j = i + 1; j < shapes.Length; j++)
                {
                    var shapeA = shapes[i];
                    var shapeB = shapes[j];
                    if (GJKPhysic2D.IsColliding(shapeA, shapeB))
                    {
                        shapeB.OnShapeTriggerEnter(shapeA);
                        shapeA.OnShapeTriggerEnter(shapeB);
                    }
                    else
                    {
                        shapes[i].DebugColor = Color.green;
                        shapes[j].DebugColor = Color.green;
                    }
                }
            }
        }
    }
}