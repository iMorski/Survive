using System;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [NonSerialized] public Rigidbody Rigidbody;
    [NonSerialized] public Animator Animator;
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    private Vector3 Position;

    public void ChangePosition(Vector2 Direction)
    {
        if (Direction != new Vector2(0.0f, 0.0f))
        {
            Position = Vector3.Normalize(new Vector3(
                Direction.x, 0.0f, Direction.y));

            SmoothRotation();
            SmoothAnimatorSpeed(1.0f);
        }
        else
        {
            SmoothPosition();
            SmoothAnimatorSpeed(0.0f);
        }

        Rigidbody.velocity = Position * SmoothSpeed();
    }
    
    private Vector3 PositionVelocity;
    
    private void SmoothPosition()
    {
        if (Position != new Vector3(0.0f, 0.0f, 0.0f)) Position = 
            Vector3.SmoothDamp(Position, new Vector3(0.0f, 0.0f, 0.0f),
                ref PositionVelocity, CharacterContainer.Instance.MoveSmoothTime);
    }

    private Vector3 RotationDirection;
    private Vector3 RotationVelocity;

    private void SmoothRotation()
    {
        if (RotationDirection != Position) RotationDirection = 
            Vector3.SmoothDamp(RotationDirection, Position,
                ref RotationVelocity, CharacterContainer.Instance.MoveSmoothTime);
        
        transform.rotation = Quaternion.LookRotation(RotationDirection);
    }
    
    private float AnimatorSpeed;
    private float AnimatorVelocity;

    private void SmoothAnimatorSpeed(float Value)
    {
        if (AnimatorSpeed != Value) AnimatorSpeed = 
            Mathf.SmoothDamp(AnimatorSpeed, Value,
                ref AnimatorVelocity, CharacterContainer.Instance.MoveSmoothTime);
        
        Animator.SetFloat("Speed", AnimatorSpeed);
    }
    
    private float SpeedCurrent;
    private float SpeedVelocity;
    
    private float SmoothSpeed()
    {
        float MoveSpeed = CharacterContainer.Instance.MoveSpeed;
        float MoveSmoothTime = CharacterContainer.Instance.MoveSmoothTime;
        
        if (SpeedCurrent < MoveSpeed) SpeedCurrent =
            Mathf.SmoothDamp(SpeedCurrent, MoveSpeed,
                ref SpeedVelocity, MoveSmoothTime);

        return SpeedCurrent * Time.fixedDeltaTime;
    }
}