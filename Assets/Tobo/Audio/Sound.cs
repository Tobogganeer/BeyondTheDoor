using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sound")]
public class Sound : ScriptableObject
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float maxDistance = 35f;
    [SerializeField] private AudioCategory category = AudioCategory.SFX;
    [SerializeField] private float volume = 1.0f;
    [SerializeField] private float minPitch = 0.85f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private bool is2d = false;

    public ID SoundID => name;
    public AudioClip[] Clips => clips;
    public float MaxDistance => maxDistance;
    public AudioCategory Category => category;
    public float Volume => volume;
    public float MinPitch => minPitch;
    public float MaxPitch => maxPitch;
    public bool Is2d => is2d;

    public static Sound From(ID id)
    {
        return AudioManager.GetSound(id);
    }

    public static Audio Override(ID id)
    {
        return From(id).Override();
    }

    public Audio Override()
    {
        return GetAudio();
    }

    public Audio GetAudio()
    {
        return new Audio(this);
    }

    public static bool Exists(ID id) => AudioManager.SoundExists(id);

    #region Play
    public void Play(Vector3 position)
    {
        AudioManager.Play(this, position);
    }

    public void Play2D()
    {
        AudioManager.Play2D(this);
    }

    public void PlayLocal(Vector3 position)
    {
        AudioManager.PlayLocal(this, position);
    }

    public void PlayLocal2D()
    {
        AudioManager.PlayLocal2D(this);
    }
    #endregion

    public class ID
    {
        // Yes, it's a wrapper of a string to be a bit more functional
        public string Value { get; private set; }

        public static ID None => "none";

        public ID(string value) => Value = value.Trim().ToLower();

        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object obj)
        {
            if (obj is not ID && obj is not string)
                return false;

            ID other = (ID)obj;

            return this.Value == other.Value;
        }
        public static implicit operator string(ID id) => id.Value;
        public static implicit operator ID(string str) => new ID(str);


        public Audio Override() => Sound.Override(this);
        public PooledAudioSource Play(Vector3 position) => AudioManager.Play(this, position);
        public PooledAudioSource Play2D() => AudioManager.Play2D(this);
        public PooledAudioSource PlayLocal(Vector3 position) => AudioManager.PlayLocal(this, position);
        public PooledAudioSource PlayLocal2D() => AudioManager.PlayLocal2D(this);
        public bool Exists() => Sound.Exists(this);
    }

    public static Sound CreateInternal(List<AudioClip> clips, bool is2D, AudioCategory category)
    {
        Sound s = CreateInstance<Sound>();

        s.clips = clips.ToArray();
        s.is2d = is2D;
        s.category = category;

        return s;
    }
}
