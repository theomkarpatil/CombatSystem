// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Gameplay.Input
{
    public class PlayerInputManager : Managers.Singleton<PlayerInputManager>
    {
        public PlayerKeyBindings pkb { private set; get; }
        public GameplayInputReader gameplayInputReader;

        private void Awake()
        {
            if (pkb == null)
                pkb = new PlayerKeyBindings();
        }

        public void EnablePlayerInput()
        {
            gameplayInputReader.Enable();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }
}