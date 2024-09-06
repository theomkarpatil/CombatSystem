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
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator anim;

        private void OnEnable()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {           
            anim.SetFloat("speed", Utility.SoraMath.Rescale(0.0f, PlayerController.instance.maxSprintingSpeed, PlayerController.instance.GetCurrentSpeed()));
        }

        public void OnEnterringCombatMode(Component Invoker, object data)
        {
            anim.SetLayerWeight(0, 0.0f);
            anim.SetLayerWeight(1, 1.0f);
        }

        public void OnExittingCombatMode(Component Invoker, object data)
        {
            anim.SetLayerWeight(0, 1.0f);
            anim.SetLayerWeight(1, 0.0f);
        }
    }
}