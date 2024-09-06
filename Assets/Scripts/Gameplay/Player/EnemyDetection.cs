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

namespace Sora.Gameplay.Combat
{
    public class EnemyDetection : MonoBehaviour
    {        
        public List<Transform> enemiesInRange = new List<Transform>();
        [SerializeField] private Events.SoraEvent enableCombatMode;
        [SerializeField] private Events.SoraEvent disableCombatMode;
        [SerializeField] private float noEnemiesInRangeBuffer;

        // aim direction varibles
        [Space]
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private float enemySearchRadius;
        [SerializeField] private float enemyAimRadius;
        private Transform aimTarget;
        [SerializeField] private Events.SoraEvent enemySelectedEvent;
        private Vector2 aimDirection;

        private bool isEnemyInRange;
        private Coroutine setEnemiesInRange;
        private SphereCollider collider;

        private void OnEnable()
        {
            collider = GetComponent<SphereCollider>();
            collider.radius = enemySearchRadius/2.0f;

            PlayerInputManager.instance.gameplayInputReader.cameraRotateEvent += OnAimingAtTarget;
        }

        private void OnDisable()
        {
            PlayerInputManager.instance.gameplayInputReader.cameraRotateEvent -= OnAimingAtTarget;
        }

        private void OnAimingAtTarget(Vector2 dir)
        {
            aimDirection = dir;
        }

        private void FixedUpdate()
        {
            Ray _ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            Debug.DrawRay(_ray.origin, _ray.direction * 5.0f, Color.red);

            if (enemiesInRange.Count > 0)
                SetATarget();
        }

        private void SetATarget()
        {
            Ray _ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit _hitInfo;

            if (Physics.BoxCast(Camera.main.transform.position, new Vector3(2.0f, 20.0f, 2.0f), Camera.main.transform.forward, out _hitInfo, Quaternion.identity, enemySearchRadius, enemyLayerMask))
            //if (Physics.Raycast(_ray, out _hitInfo, enemySearchRadius, enemyLayerMask))
            {
                aimTarget = _hitInfo.transform;
                enemySelectedEvent.InvokeEvent(this, aimTarget);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // add enemies that have been found to the list
            enemiesInRange.Add(other.transform);
            
            // when an enemy is found for the first time, start combat mode
            if (enemiesInRange.Count == 1 && !isEnemyInRange)
                enableCombatMode.InvokeEvent();

            isEnemyInRange = true;
            if (setEnemiesInRange != null)
                StopCoroutine(setEnemiesInRange);

        }

        private void OnTriggerExit(Collider other)
        {
            if (enemiesInRange.Contains(other.transform))
                enemiesInRange.Remove(other.transform);

            if (enemiesInRange.Count == 0)
                setEnemiesInRange = StartCoroutine(SetEnemiesInRange());
        }

        private IEnumerator SetEnemiesInRange()
        {
            yield return new WaitForSeconds(noEnemiesInRangeBuffer);

            disableCombatMode.InvokeEvent();
            isEnemyInRange = false;
        }
    }
}