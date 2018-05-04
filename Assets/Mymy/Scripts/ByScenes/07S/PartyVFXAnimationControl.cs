﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scene07Party
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PartyVFXAnimationControl : MonoBehaviour {

        [Serializable]
        public class SphereAnimationPackage
        {
            [SerializeField]
            private PartyOptionEnum PartyOption;
            [SerializeField]
            private string resourceFormatPath;
            [SerializeField]
            public float FrameRate;
            [SerializeField]
            private int numberOfFrames;
            [SerializeField]
            public bool IsRepeat;

            public Texture2D[] Frames;
            public bool IsFinishedPlaying
            {
                get { return m_IsFinishedPlaying; }
                set { m_IsFinishedPlaying = value; }
            }
            private bool m_IsFinishedPlaying = true;
            public float AnimationStartTime { get; set; }

            public void InitializeSphereAnimationPackage()
            {
                if (string.IsNullOrEmpty(resourceFormatPath))
                {
                    resourceFormatPath = defaultResourceFormatPath;
                }

                Frames = new Texture2D[numberOfFrames];
                
                for (int i = 0; i < numberOfFrames; i++)
                {
                    //Folder Frame Index Starts with <00001>
                    string texturePath = string.Format(resourceFormatPath, i+1);
                    Frames[i] = Resources.Load<Texture2D>(texturePath);
                }
            }
        }


        [SerializeField]
        private SphereAnimationPackage[] m_SphereVFXAnimations = new SphereAnimationPackage[3];
        [SerializeField]
        private MeshRenderer m_SphereMeshRenderer;

        private const string defaultResourceFormatPath = "s7-02a-once/B_{0:d5}";

        private void Awake()
        {
            if (!m_SphereMeshRenderer)
            {
                m_SphereMeshRenderer = GetComponent<MeshRenderer>();
            }

            
        }

        private void Start()
        {
            foreach (SphereAnimationPackage VFXPackage in m_SphereVFXAnimations)
            {
                VFXPackage.InitializeSphereAnimationPackage();
            }
            SetAnimationFrame(m_SphereVFXAnimations[0], 359);
        }

        public IEnumerator PlayPartyVFX(PartyOptionEnum FXType)
        {
            switch (FXType)
            {
                case PartyOptionEnum.Drink:
                    yield return StartCoroutine(PlayFXAnim(m_SphereVFXAnimations[0]));
                    break;
                case PartyOptionEnum.Dart:
                    yield return StartCoroutine(PlayFXAnim(m_SphereVFXAnimations[1]));
                    break;
                case PartyOptionEnum.Pool:
                    yield return StartCoroutine(PlayFXAnim(m_SphereVFXAnimations[2]));
                    break;
                default:
                    yield return null;
                    break;
            }
        }

        private IEnumerator PlayFXAnim(SphereAnimationPackage sphereAnim)
        {
            if (sphereAnim.IsFinishedPlaying)
            {
                int currentFrame = 0;
                float frameLength = 1.0f / sphereAnim.FrameRate;

                if (sphereAnim.IsRepeat)
                {
                    //sphereAnim.AnimationStartTime = Time.time;
                    sphereAnim.IsFinishedPlaying = false;

                    while (!sphereAnim.IsFinishedPlaying)
                    {
                        SetAnimationFrame(sphereAnim, currentFrame % sphereAnim.Frames.Length);
                        currentFrame++;
                        yield return new WaitForSeconds(frameLength);
                    }
                    sphereAnim.IsFinishedPlaying = true;
                    //currentFrame = (int)(((Time.time - sphereAnim.AnimationStartTime) * sphereAnim.FrameRate) % sphereAnim.Frames.Length);
                    //if (currentFrame >= sphereAnim.Frames.Length)
                    //{
                    //    currentFrame = sphereAnim.Frames.Length;
                    //}

                }
                else
                {
                    sphereAnim.IsFinishedPlaying = false;
                    for (; currentFrame < sphereAnim.Frames.Length; currentFrame++)
                    {
                        SetAnimationFrame(sphereAnim, currentFrame);
                        yield return new WaitForSeconds(frameLength);
                    }

                    sphereAnim.IsFinishedPlaying = true;
                    //currentFrame = (int)(Time.time * frameRate);
                    //if (currentFrame >= frames.Length)
                    //{
                    //    isFinishedPlaying = true;
                    //    animationSphere.SetActive(false);
                    //}
                    //else
                    //{
                    //    SetAnimationFrame(currentFrame);
                    //}

                    //print(currentFrame);
                }
            }

            //yield return null;
        }

        public void StopAnimation(SphereAnimationPackage sphereAnim)
        {
            sphereAnim.IsFinishedPlaying = true;
        }

        public void StopAnimation(SphereAnimationPackage sphereAnim, int frameToShowAfterStop)
        {
            StopAnimation(sphereAnim);
            if (frameToShowAfterStop < sphereAnim.Frames.Length)
            {
                SetAnimationFrame(sphereAnim, frameToShowAfterStop);
            }
        }

        private void SetAnimationFrame(SphereAnimationPackage sphereAnim, int i)
        {
            m_SphereMeshRenderer.material.SetTexture("_MainTex", sphereAnim.Frames[i]);
        }

        public IEnumerator HandlePartyOptionOnSelected(PartySelection partyOption)
        {
            yield return null;
        }
    }
}