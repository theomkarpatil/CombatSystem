// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Managers
{
    public class AudioManager : Managers.Singleton<AudioManager>
    {
        [SerializeField] private AudioClip[] punchClips;
        [SerializeField] private AudioClip punchHoldClip;
        private int prevPunchClip = 0;

        [SerializeField] private AudioClip[] kickClips;
        [SerializeField] private AudioClip combatStart;
        private int punchClipIndex = 0;
        private int kickClipIndex = 0;

        public void PlayPunchClip(AudioSource source)
        {
            source.PlayOneShot(punchClips[GetRandomIndex(0, punchClips.Length)], 1.0f);
            punchClipIndex++;
        }

        public void PlayHoldPunchClip(AudioSource source)
        {
            source.PlayOneShot(punchHoldClip);
        }

        public void PlayKickClip()
        {

        }

        public void PlayCombatStart(AudioSource source)
        {
            source.PlayOneShot(combatStart, 1.0f);
        }

        private int GetRandomIndex(int start, int end)
        {
            int rng = prevPunchClip;
            while (rng == prevPunchClip)
            {
                rng = Random.Range(start, end);
            } 

            prevPunchClip = rng;
            return rng;
        }
     }
}