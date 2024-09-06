// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Gameplay 
{
 
 
    public class CameraPivot : MonoBehaviour
    {
        [SerializeField] private Vector3 cameraPositionOffset;
        [SerializeField] private Quaternion cameraRotationOffset;

        private void Start()
        {
            Transform cam = Camera.main.transform;
            cam.SetParent(transform);

            cam.localPosition = cameraPositionOffset;
            cam.localRotation = cameraRotationOffset;
        }

        private void Update()
        {
            Camera.main.transform.localPosition = cameraPositionOffset; 
        }
    }
}