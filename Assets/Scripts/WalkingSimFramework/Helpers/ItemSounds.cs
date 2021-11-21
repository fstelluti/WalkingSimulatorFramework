using UnityEngine;

namespace WalkingSimFramework.Helpers
{
    /// <summary>
    /// Contains sounds for interacting with items
    /// </summary>
    [System.Serializable]
    public class ItemSounds
    {
        // For now, just list all needed sounds here
        [Space]
        [SerializeField] private AudioClip pickUpSound;
        [SerializeField] private AudioClip putBackSound;

        [Space]
        [SerializeField] private AudioClip equipmentUseSuccessfulSound;
        [SerializeField] private AudioClip equipmentUseUnsuccessfulSound;

        [Space]
        [SerializeField] private AudioClip blockedSound;

        [Space]
        [SerializeField] private AudioClip openSound;
        [SerializeField] private AudioClip closedSound;

        AudioSource m_audioSource;

        bool m_isPickUp = true;
        bool m_isOpening = true;

        public AudioSource ItemAudioSource { get => m_audioSource; }

        public AudioClip PickUpSound { get => pickUpSound; set => pickUpSound = value; }

        /// <summary>
        /// Creates a temp gameobject to attach an AudioSource component
        /// </summary>
        public void InitSoundItem(GameObject _item)
        {
            if(!AnySoundClipsValid())
            {
                return;
            }

            GameObject _audioObject = new GameObject("Item Audio");
            _audioObject.transform.position = _item.transform.position;
            _audioObject.transform.parent = _item.transform;

            m_audioSource = _audioObject.AddComponent<AudioSource>();
            m_audioSource.playOnAwake = false;
            m_audioSource.spatialBlend = 1.0f;
        }

        private bool AnySoundClipsValid()
        {
            bool isSomeClipValid = false;

            isSomeClipValid |= pickUpSound != null;
            isSomeClipValid |= putBackSound != null;
            isSomeClipValid |= equipmentUseSuccessfulSound != null;
            isSomeClipValid |= equipmentUseUnsuccessfulSound != null;
            isSomeClipValid |= openSound != null;
            isSomeClipValid |= closedSound != null;
            isSomeClipValid |= blockedSound != null;

            return isSomeClipValid;
        }

        public void TogglePickupPutBackSound()
        {
            if (m_audioSource == null)
            {
                return;
            }

            if (m_isPickUp)
            {
                if(pickUpSound != null)
                {
                    m_audioSource.clip = pickUpSound;
                }
            }
            else
            {
                if (putBackSound != null)
                {
                    m_audioSource.clip = putBackSound;
                }
            }

            m_audioSource.Play();
            m_isPickUp = !m_isPickUp;
        }

        public void ToggleOpenCloseSound()
        {
            if (m_audioSource == null)
            {
                return;
            }

            if (m_isOpening)
            {
                if (openSound != null)
                {
                    m_audioSource.clip = openSound;
                }
            }
            else
            {
                if (closedSound != null)
                {
                    m_audioSource.clip = closedSound;
                }
            }

            m_audioSource.Play();
            m_isOpening = !m_isOpening;
        }


        public void PlayEquipmentUsedSuccessfullySound()
        {
            PlayClip(equipmentUseSuccessfulSound);
        }

        public void PlayEquipmentUsedUnSuccessfullySound()
        {
            PlayClip(equipmentUseUnsuccessfulSound);
        }

        public void PlayBlockedSound()
        {
            PlayClip(blockedSound);
        }

        private void PlayClip(AudioClip _clip)
        {
            if (m_audioSource == null || _clip == null)
            {
                return;
            }

            m_audioSource.clip = _clip;
            m_audioSource.Play();
        }
    }
}
