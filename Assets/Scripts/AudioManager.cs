using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    private static AudioManager Instance;

    [Header("Configuration")]
    [SerializeField] private bool mute;
    [SerializeField] [Range(0f,1f)] private float default_volume = 0.5f;

	//[Header("Music")]
	//[SerializeField] private AudioClip[] musics;
    private AudioSource _source;

	[Header("All sounds")]
    [SerializeField] private AudioClip[] sounds_questStarted;
    [SerializeField] private AudioClip[] sounds_heroDied;
    [SerializeField] private AudioClip[] sounds_greeting;
    [SerializeField] private AudioClip[] sounds_levelUp;
    [SerializeField] private AudioClip[] sounds_gold;
    [SerializeField] private AudioClip[] sounds_map;
    [SerializeField] private AudioClip[] sounds_rire;

	private void Awake() {
		if(Instance) {
            enabled = false;
            Debug.LogError("nooooooo pas 2 audiomanagers!");
            return;
        }
        Instance = this;
        _source = GetComponent<AudioSource>();
	}

    public static void SetMute(bool mute) {
		Instance.mute = mute;
        Instance._source.mute = mute;

        // delete children
        if(mute) {
			foreach(Transform child in Instance.transform) {
				Destroy(child.gameObject);
			}
		}
	}

    private AudioClip GetRandomSound(SoundEffect effect) {
		AudioClip[] array = effect switch {
			SoundEffect.QuestStarted => sounds_questStarted,
			SoundEffect.HeroDied => sounds_heroDied,
			SoundEffect.Greeting => sounds_greeting,
			SoundEffect.Levelup => sounds_levelUp,
			SoundEffect.Gold => sounds_gold,
			SoundEffect.MapEffect => sounds_map,
			SoundEffect.Laught => sounds_rire,
			_ => throw new System.Exception("unexpected sound effectvalue: " + effect),
		};
		if(array.Length == 0) {
            Debug.LogError("empty sound array ! (" + effect + ")");
            return null;
        }
        return array[Random.Range(0, array.Length)];
	}

    public enum SoundEffect {
        QuestStarted,
        HeroDied,
        Greeting,
        Levelup,
        Gold,
        MapEffect,
        Laught
    };

    public static void PlaySoundEffect(SoundEffect effect) {
        PlaySoundEffect(effect, Instance.default_volume);
	}

	public static void PlaySoundEffect(SoundEffect effect, float volume) {
        if(Instance.mute)
            return;
        
        // get the clip
        AudioClip clip = Instance.GetRandomSound(effect);
        if(clip == null)
            return;

        // Instanciate an object, play the sound.
        var go = new GameObject();
        var source = go.AddComponent<AudioSource>();
        source.PlayOneShot(clip, volume);
        go.transform.SetParent(Instance.transform);

        // destroy the GO after the duration.
        Destroy(go, clip.length);
    } 

}
