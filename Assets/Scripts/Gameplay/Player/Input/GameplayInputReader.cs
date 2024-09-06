// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Sora.Gameplay.Input
{
    [CreateAssetMenu(fileName = "GameplayInputReader", menuName = "Sora/Input System/GameplayInputReader")]

    public class GameplayInputReader : ScriptableObject, PlayerKeyBindings.IPlayerMotionActions, PlayerKeyBindings.ICameraControlActions, PlayerKeyBindings.IPlayerCombatActions
    {
        // Player Motion Events
        public event UnityAction<Vector2> playerMoveEvent;
        public event UnityAction sprintPerformedEvent;
        public event UnityAction sprintCanceledEvent;

        // Camera Control Events
        public event UnityAction<Vector2> cameraRotateEvent;

        // Combat Events
        public event UnityAction primaryAttackPerformedEvent;
        public event UnityAction primaryAttackCanceledEvent;
        public event UnityAction secondaryAttackPerformedEvent;
        public event UnityAction secondaryAttackCanceledEvent;

        public void Enable()
        {
            PlayerInputManager.instance.pkb.PlayerMotion.Enable();
            PlayerInputManager.instance.pkb.PlayerMotion.SetCallbacks(this);
            PlayerInputManager.instance.pkb.CameraControl.Enable();
            PlayerInputManager.instance.pkb.CameraControl.SetCallbacks(this);

            // not enabled because we enable it when needed
            PlayerInputManager.instance.pkb.PlayerCombat.SetCallbacks(this);
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (playerMoveEvent != null)
                playerMoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (sprintPerformedEvent != null && context.phase == InputActionPhase.Performed)
                sprintPerformedEvent.Invoke();
            if (sprintPerformedEvent != null && context.phase == InputActionPhase.Canceled)
                sprintCanceledEvent.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (cameraRotateEvent != null)
                cameraRotateEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAttackPrimary(InputAction.CallbackContext context)
        {
            if (primaryAttackPerformedEvent != null && context.phase == InputActionPhase.Performed)
                primaryAttackPerformedEvent.Invoke();
            if (primaryAttackCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                primaryAttackCanceledEvent.Invoke();
        }

        public void OnAttackSecondary(InputAction.CallbackContext context)
        {
            if (secondaryAttackPerformedEvent != null && context.phase == InputActionPhase.Performed)
                secondaryAttackPerformedEvent.Invoke();
            if (secondaryAttackCanceledEvent != null && context.phase == InputActionPhase.Canceled)
                secondaryAttackCanceledEvent.Invoke();
        }
    }
}