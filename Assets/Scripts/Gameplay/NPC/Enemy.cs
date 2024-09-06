// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Gameplay.NPC
{ 
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject selectedMarker;
        [SerializeField] private Events.SoraEvent enemyTakesHit;

        public void OnSelectingTargetEnemy(Component invoker, object data)
        {
            if (data as Transform == transform)
            {
                selectedMarker.SetActive(true);
            }
            else
                selectedMarker.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("PlayerHit"))
            {
                enemyTakesHit.InvokeEvent();
            }
        }
    }
}