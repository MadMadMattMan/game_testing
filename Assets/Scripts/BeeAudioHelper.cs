using UnityEngine;

public class BeeAudioHelper : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip beeSpawn, beeTakeoff, beeLoop;

    [SerializeField] beeState beeState;
    beeState pastState = beeState.Empty;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if (pastState != beeState)
            Checks();

        if (beeState != beeState.Empty && !audioSource.isPlaying) {
            audioSource.clip = beeLoop;
            audioSource.loop = true;
            audioSource.Play();
        }

        pastState = beeState;
    }

    void Checks() {
        switch (beeState) {
            case beeState.DropOff:

                break;

            case beeState.SpawnIn:
                audioSource.clip = beeSpawn;
                audioSource.loop = false;
                audioSource.Play();
                break;
        }
    }
}

[System.Serializable] enum beeState {
    Empty,
    DropOff,
    FlyAway,
    SpawnIn,
    Hover
}
