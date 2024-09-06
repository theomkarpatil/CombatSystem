// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora 
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        private Animator anim;
        private Transform aimTarget;


        private void OnEnable()
        {
            anim = GetComponent<Animator>();
        }

        public void OnTakingHit(Component invoker, object data)
        {
            if(aimTarget == transform.parent)
                anim.Play("TakeHit");
        }

        public void OnSelectingAimTarget(Component invoker, object data)
        {
            aimTarget = data as Transform;
        }
    }
}