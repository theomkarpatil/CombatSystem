// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sora.Gameplay.Input;

namespace Sora.Gameplay
{
    public enum EPlayerMovementState
    {
        IDLE,
        WALKING,
        SPRINTING,
        CROUCHING,
        PRONE,
        JUMPING
    }
    
    public class PlayerController : Managers.Singleton<PlayerController>
    {
        [Header("Movement Variables")]
        [SerializeField] private float _maxWalkingSpeed;
        [SerializeField] private float _maxSprintingSpeed;
        public float maxWalkingSpeed { private set; get; }
        public float maxSprintingSpeed { private set; get; }
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float turnRateWalking;
        [SerializeField] private float turnRateSprinting;
        public EPlayerMovementState movementState { private set; get; }
        private bool sprint;
        private Vector3 inputDirection;
        private Vector3 movementDirection;
        private float smoothingSpeed;

        private Rigidbody rBody;

        private void OnEnable()
        {
            rBody = GetComponent<Rigidbody>();
            movementState = EPlayerMovementState.IDLE;
            PlayerInputManager.instance.EnablePlayerInput();

            PlayerInputManager.instance.gameplayInputReader.playerMoveEvent += OnPlayerMovement;
            PlayerInputManager.instance.gameplayInputReader.sprintPerformedEvent += OnSprintEnabled;
            PlayerInputManager.instance.gameplayInputReader.sprintCanceledEvent += OnSprintDisabled;
        }

        private void OnDisable()
        {
            PlayerInputManager.instance.gameplayInputReader.playerMoveEvent -= OnPlayerMovement;
            PlayerInputManager.instance.gameplayInputReader.sprintPerformedEvent -= OnSprintEnabled;
            PlayerInputManager.instance.gameplayInputReader.sprintCanceledEvent -= OnSprintDisabled;
        }

        private void OnValidate()
        {
            maxWalkingSpeed = _maxWalkingSpeed;
            maxSprintingSpeed = _maxSprintingSpeed;
        }

        private void OnPlayerMovement(Vector2 dir)
        {
            inputDirection = new Vector3(dir.x, 0.0f, dir.y);
        }

        private void OnSprintEnabled()
        {
            sprint = true;
        }

        private void OnSprintDisabled()
        {
            sprint = false;
        }


        private void FixedUpdate()
        {
            CalculateMovementDirection();
            MovePlayer();
        }

        private void CalculateMovementDirection()
        {
            // Calculate facing direction to apply forward motion in
            if (!Utility.SoraMath.WithinEpsilon(inputDirection.magnitude, 0.1f))
            {
                float _targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

                float _facingAngle;
                if (movementState == EPlayerMovementState.SPRINTING)
                    _facingAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref smoothingSpeed, turnRateSprinting);
                else
                    _facingAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref smoothingSpeed, turnRateWalking);

                transform.rotation = Quaternion.Euler(0.0f, _facingAngle, 0.0f);

                movementDirection = (Quaternion.Euler(0.0f, _targetAngle, 0.0f) * Vector3.forward).normalized;
            }
            else
                movementDirection = Vector3.zero;
        }

        private void MovePlayer()
        {
            if (Utility.SoraMath.WithinEpsilon(rBody.velocity.sqrMagnitude, 0.1f))
            {
                movementState = EPlayerMovementState.IDLE;
            }

            // if the input is very minute decellerate the player
            if(Utility.SoraMath.WithinEpsilon(movementDirection.magnitude, 0.1f) && rBody.velocity.sqrMagnitude != 0.0f)
            {
                rBody.velocity *= deceleration;
            }
            // walking
            else if (!sprint && !Utility.SoraMath.WithinEpsilon(movementDirection.magnitude, 0.1f))
            {
                movementState = EPlayerMovementState.WALKING;
                if (rBody.velocity.sqrMagnitude <= maxWalkingSpeed)
                    rBody.AddForce(Time.deltaTime * 100.0f * acceleration * movementDirection);
            }
            // sprinting
            else if (!Utility.SoraMath.WithinEpsilon(movementDirection.magnitude, 0.1f))
            {
                movementState = EPlayerMovementState.SPRINTING;

               if (rBody.velocity.sqrMagnitude <= maxSprintingSpeed)
                    rBody.AddForce(Time.deltaTime * 100.0f * acceleration * movementDirection);
            }
        }
        
        public float GetCurrentSpeed()
        {
            return rBody.velocity.sqrMagnitude;
        }
    }
}