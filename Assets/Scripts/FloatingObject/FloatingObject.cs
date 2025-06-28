using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    [SerializeField] private Transform[] floatingPoints;
    [SerializeField] private float underWaterDrag = 0.6f;
    [SerializeField] private float underWaterAngularDrag = 1;
    [SerializeField] private float airDrag = 0;
    [SerializeField] private float airAngularDrag = 0.05f;
    [SerializeField] private float floatPower = 5;

    [SerializeField] private float waterHeight = 0;

    private Rigidbody _rb;
    private bool _isUnderWater;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        int countUnderWaterPoint = 0 ;
        foreach (var point in floatingPoints)
        {
            float diff = point.position.y - waterHeight;

            if (diff < 0)
            {
                _rb.AddForceAtPosition(
                    Vector3.up * (floatPower * Mathf.Abs(diff)),
                    point.position,
                    ForceMode.Acceleration
                );
                countUnderWaterPoint++;
                if (!_isUnderWater)
                {
                    _isUnderWater = true;
                    SetStage(true);
                }
            }
        }
        
        if (_isUnderWater && countUnderWaterPoint == 0)
        {
            _isUnderWater = false;
            SetStage(false);
        }
    }

    private void SetStage(bool underWater)
    {
        if (underWater)
        {
            _rb.linearDamping = underWaterDrag;
            _rb.angularDamping = underWaterAngularDrag;
        }
        else
        {
            _rb.linearDamping = airDrag;
            _rb.angularDamping = airAngularDrag;
        }
    }
}