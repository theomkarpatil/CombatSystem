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
using Sora.Managers;

namespace Sora.Gameplay.Combat
{
    public class CombatController : Singleton<CombatController>
    {
        [SerializeField] private float comboTimer;
        [SerializeField] private float holdAttackTimer;
        [SerializeField] private float combatDistance;

        // primary attacks
        [Space]
        [SerializeField] private int maxPrimaryAttacks;
        private float primaryComboTimer;
        private bool countPrimaryComboTimer;
        private int primaryIndex = 1;
        private bool awaitingPrimaryHold;
        private float primaryHoldTImer;

        // secondary attacks
        [Space]
        [SerializeField] private int maxSecondaryAttacks;
        private float secondaryComboTimer;
        private bool countSecondaryComboTimer;
        private int secondaryIndex = 0;
        private bool awaitingSecondaryHold;
        private float secondaryHoldTImer;

        private Animator anim;
        private AudioSource audioSource;
        private bool performingHoldAttack;
        private Transform aimTarget;
        private bool moveTowardsEnemy;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            audioSource = GetComponentInChildren<AudioSource>();
        }

        private void OnEnable()
        {
            PlayerInputManager.instance.gameplayInputReader.primaryAttackPerformedEvent += OnPrimaryAttackPerformed;
            PlayerInputManager.instance.gameplayInputReader.primaryAttackCanceledEvent += OnPrimaryAttackCanceled;
            PlayerInputManager.instance.gameplayInputReader.secondaryAttackPerformedEvent += OnSecondaryAttack;
            PlayerInputManager.instance.gameplayInputReader.secondaryAttackCanceledEvent += OnSecondaryAttackCanceled;
        }

        private void OnDisable()
        {
            PlayerInputManager.instance.gameplayInputReader.primaryAttackPerformedEvent -= OnPrimaryAttackPerformed;
            PlayerInputManager.instance.gameplayInputReader.primaryAttackCanceledEvent -= OnPrimaryAttackCanceled;
            PlayerInputManager.instance.gameplayInputReader.secondaryAttackPerformedEvent -= OnSecondaryAttack;
            PlayerInputManager.instance.gameplayInputReader.secondaryAttackCanceledEvent -= OnSecondaryAttackCanceled;            
        }
    
        private void OnPrimaryAttackPerformed()
        {
            awaitingPrimaryHold = true;
        }

        private void OnPrimaryAttackCanceled()
        {
            if (primaryHoldTImer >= holdAttackTimer)
            {
                PerformHoldPunch();
            }
            else if(!performingHoldAttack)
                PerformPunch();

            performingHoldAttack = false;
            awaitingPrimaryHold = false;
        }

        private void OnSecondaryAttack()
        {
            awaitingSecondaryHold = true;
        }

        private void OnSecondaryAttackCanceled()
        {

        }

        private void Update()
        {
            if (countPrimaryComboTimer)
            {
                primaryComboTimer += Time.deltaTime;
                if (primaryComboTimer >= comboTimer)
                {
                    countPrimaryComboTimer = false;
                    primaryIndex = 0;
                }
            }

            if(awaitingPrimaryHold)
            {
                primaryHoldTImer += Time.deltaTime;
                if (primaryHoldTImer >= holdAttackTimer)
                {
                    PerformHoldPunch();
                    performingHoldAttack = true;
                }
            }

            if(moveTowardsEnemy && Vector3.Distance(transform.position, aimTarget.position) > combatDistance)
            {

            }
        }

        private void PerformPunch()
        {
            primaryComboTimer = 0.0f;
            primaryIndex++;
            countPrimaryComboTimer = true;

            if(aimTarget && Vector3.Distance(transform.position, aimTarget.position) > combatDistance)
            {
                anim.SetTrigger("primaryRun");
                moveTowardsEnemy = true;
            }
            else
                anim.SetTrigger($"primary{(primaryIndex % maxPrimaryAttacks) + 1}");


            AudioManager.instance.PlayPunchClip(audioSource); 
        }

        private void PerformHoldPunch()
        {
            if (!awaitingPrimaryHold)
                return;

            anim.SetTrigger($"primaryHold");
            awaitingPrimaryHold = false;
            primaryHoldTImer = 0.0f;
            performingHoldAttack = false;
            AudioManager.instance.PlayHoldPunchClip(audioSource);
        }
        
        public void OnCombatStart(Component invoker, object data)
        {
            PlayerInputManager.instance.pkb.PlayerCombat.Enable();
            AudioManager.instance.PlayCombatStart(audioSource);
        }

        public void OnCombatExit(Component invoker, object data)
        {
            PlayerInputManager.instance.pkb.PlayerCombat.Disable();
        }

        public void OnSelectingEnemy(Component invoker, object data)
        {
            aimTarget = data as Transform;
        }
    }
}