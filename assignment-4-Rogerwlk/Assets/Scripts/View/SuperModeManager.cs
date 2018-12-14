using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

namespace CornellTech.View
{
    public class SuperModeManager : MonoBehaviour
    {
        //Serialized
        [SerializeField]
        protected Button AntiGravityButton, ThrowModeButton, MusicButton;

        [SerializeField]
        protected Color activeButtonColor = Color.green, inactiveButtonColor = Color.blue;

        AudioClip backgroundMusic;

        AudioSource audioSource;

        bool gravityButtonClicked = false, throwButtonClicked = false, musicButtonClicked = false;

        void Awake()
        {
            AntiGravityButton.onClick.AddListener(OnAntiGravityButtonClicked);
            ThrowModeButton.onClick.AddListener(OnThrowModeButtonClicked);
            MusicButton.onClick.AddListener(OnMusicButtonClicked);
        }

        // Use this for initialization
        void Start()
        {
            backgroundMusic = Resources.Load<AudioClip>("BackgroundMusic");
            GameObject obj = GameObject.FindGameObjectWithTag("BackgroundMusic");
            audioSource = obj.GetComponent<AudioSource>();
            audioSource.loop = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnAntiGravityButtonClicked()
        {
            

            if (!gravityButtonClicked)
            {
                Debug.Log("No gravity");
                gravityButtonClicked = true;
                AntiGravityButton.SetNormalColor(activeButtonColor);

                Physics.gravity = new Vector3(0f, 0f, 0f);
            }
            else
            {
                Debug.Log("gravity");
                gravityButtonClicked = false;
                AntiGravityButton.SetNormalColor(inactiveButtonColor);

                Physics.gravity = new Vector3(0f, 9.8f, 0f);
            }
        }

        void OnThrowModeButtonClicked()
        {
            Debug.Log("Throw mode");

            if (!throwButtonClicked)
            {
                throwButtonClicked = true;
                ThrowModeButton.SetNormalColor(activeButtonColor);
            }
            else
            {
                throwButtonClicked = false;
                ThrowModeButton.SetNormalColor(inactiveButtonColor);
            }
        }

        void OnMusicButtonClicked()
        {
            Debug.Log("Music");

            if (!musicButtonClicked)
            {
                musicButtonClicked = true;

                MusicButton.SetNormalColor(activeButtonColor);
                
                audioSource.PlayOneShot(backgroundMusic, 0.2f);
            }
            else
            {
                musicButtonClicked = false;

                MusicButton.SetNormalColor(inactiveButtonColor);

                audioSource.Stop();
            }
        }
    }
}