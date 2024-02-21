using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    // Doing this because I was getting some weird serialization bugs when
    //  AudioManager just had a Sound[]
    // Probably because it was in a prefab in resources or smth, idk

    [Header("Fill through Audio/Fill Sounds")]
    public Sound[] sounds;
}
