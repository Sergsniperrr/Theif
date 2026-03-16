using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : DynamicJoystick, IPointCreator
{
    private const float Angle = 135;
    
    private float _angleRad;
    private Vector3 _direction = Vector3.zero;
    
    public Vector3 GetTargetPosition()
    {
        _angleRad = Angle * Mathf.Deg2Rad;
        
        _direction.x = Horizontal * Mathf.Cos(_angleRad) - Vertical * Mathf.Sin(_angleRad);
        _direction.z = Horizontal * Mathf.Sin(_angleRad) + Vertical * Mathf.Cos(_angleRad);
        
        return transform.position + _direction;
    }
}
