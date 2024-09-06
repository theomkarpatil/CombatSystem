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
 
 
    public class CameraMovement : MonoBehaviour
    {
        [Header("Following Variables")]
        [SerializeField] private Vector3 followOffset;
        [SerializeField] private float followSmoothing = 0.2f;
        private Vector3 followVelocity = Vector3.zero;

        [Header("Viewing Variables")]
        [SerializeField] private float minPitchValue;
        [SerializeField] private float maxPitchValue;
        [SerializeField] private float yawSensitivity;
        [SerializeField] private float pitchSensitivity;
        private Quaternion targetRotation;
        private Vector2 lookDir;
        private float yaw = 0.0f;
        private float pitch = 0.0f;

        [Header("Combat Variables")]
        [SerializeField] private Vector3 combatCameraAngle;
        [SerializeField] private Vector3 pivotCameraCombatAngle;
        [SerializeField] private float cameraSwitchSpeed;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float cameraRotationSpeed;

        [Header("Follow References")]
        [SerializeField] private Transform cameraPivot;
        private Transform target;
        private bool inCombat;

        private void Awake()
        {
            cameraPivot.transform.localPosition = Vector3.up * followOffset.y;
            Camera.main.transform.localPosition = Vector3.forward * followOffset.z;
        }

        private void OnEnable()
        {
            target = PlayerController.instance.transform;
            PlayerInputManager.instance.gameplayInputReader.cameraRotateEvent += OnCameraRotation;
        }

        private void LateUpdate()
        {
            cameraPivot.transform.localPosition = Vector3.up * followOffset.y;
            //Camera.main.transform.localPosition = Vector3.forward * followOffset.z;

            LookAtTarget();
            FollowTarget();
        }

        private void OnCameraRotation(Vector2 lookDir)
        {
            this.lookDir = lookDir;
        }

        private void LookAtTarget()
        {
            yaw += lookDir.x * pitchSensitivity;
            pitch += lookDir.y * yawSensitivity;

            pitch = Mathf.Clamp(pitch, minPitchValue, maxPitchValue);

            targetRotation = Quaternion.Euler(Vector3.up * yaw);
            transform.rotation = targetRotation;

            targetRotation = Quaternion.Euler(Vector3.right * pitch);
            cameraPivot.localRotation = targetRotation;
        }

        private void CombatCamera()
        {
            Quaternion _rot = Quaternion.Euler(target.eulerAngles.y * Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, _rot, cameraRotationSpeed * Time.deltaTime * 64.0f);
            cameraPivot.localRotation = Quaternion.Euler(target.eulerAngles.x * Vector3.right);
        }

        private void FollowTarget()
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref followVelocity, followSmoothing);
        }
    }
}