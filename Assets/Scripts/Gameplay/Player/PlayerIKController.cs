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
    [RequireComponent(typeof(Animator))]
    public class PlayerIKController : MonoBehaviour
    {
        [Header("Head Rotation Properties")]
        [SerializeField] private float maxLookAtAngle;
        [SerializeField] private float minLookAtWeight;
        [SerializeField] private float lookAtLerpRate;
        private float lookAtWeight;

        private Animator anim;
        Transform aimTarget;

        private void OnEnable()
        {
            anim = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (aimTarget)
                SetHeadRotation();
        }

        private void SetHeadRotation()
        {
            Transform _target = aimTarget.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Neck);
            float _angle = Mathf.Abs(Vector3.SignedAngle(transform.forward, _target.position, Vector3.up));

            if (_angle < maxLookAtAngle)
            {
                float _targetWeight = Mathf.Max(_angle / maxLookAtAngle, minLookAtWeight);
                lookAtWeight = Mathf.Lerp(lookAtWeight, _targetWeight, lookAtLerpRate);
                anim.SetLookAtWeight(lookAtWeight);
                anim.SetLookAtPosition(_target.position);
            }
            else
            {
                lookAtWeight = Mathf.Lerp(lookAtWeight, 0.0f, lookAtLerpRate);
                anim.SetLookAtWeight(0.0f);
            }
        }

        public void OnSelectingAimTarget(Component invoker, object data)
        {
            aimTarget = data as Transform;
        }
    }
}