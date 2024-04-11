using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using BeyondTheDoor;
using BeyondTheDoor.UI;

namespace BeyondTheDoor
{
    // This class isn't in the dialogue asmdef so it can access the audio manager
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private bool playLineAudio = true;
        [SerializeField] private Key nextKey = Key.Space;

        private const float MinTimeBetweenClicks = 0.05f;
        private float timer;

        PooledAudioSource playingAudio;

        void Start()
        {
            DialogueGUI.OnLineStart += OnLineStart;
            DialogueGUI.OnLineStop += OnLineStop;
        }

        private void OnLineStart(LineID line)
        {
            if (!playLineAudio)
                return;

            Sound.ID id = line.ToString();
            if (id.Exists())
                playingAudio = id.PlayLocal2D();
        }

        private void OnLineStop(LineID line)
        {
            if (playingAudio != null && playingAudio.isActiveAndEnabled)
                playingAudio.Stop();
        }

        void Update()
        {
            timer += Time.deltaTime;

            // Advance if the 'next' key or mouse are pressed
            if (Keyboard.current[nextKey].wasReleasedThisFrame || Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (timer >= MinTimeBetweenClicks)
                {
                    DialogueGUI.Next();
                    timer = 0;
                }
            }
        }
    }
}
