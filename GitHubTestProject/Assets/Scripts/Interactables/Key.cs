using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Key : Interactable
{
    [Header("Interactions")]

    [Tooltip("The Door you want to unlock (Make sure the object has the Door Script component!)")]
    public Door door;

    [Tooltip("A collider for interaction")]
    public new BoxCollider collider;

    [Tooltip("The component that renders the key")]
    public MeshRenderer meshRenderer;

    [Tooltip("The Name that shows up in the interaction text")]
    public string keyName;

    [Header("Sound Effects")]

    [Tooltip("Where the sound is played from")]
    public AudioSource audioSource;

    [Tooltip("The sound that plays when you pick up the key")]
    public AudioClip pickupSound;

    public override string GetDescription()
    {
        return $"Right Click to pickup the <color=blue>{keyName}</color>.";
    }

    public override void Interact()
    {
        bool playSound = false;
        door.isLocked = false;
        door.CloseDoor(playSound);
        audioSource.PlayOneShot(pickupSound);
        collider.enabled = false;
        meshRenderer.enabled = false;
        StartCoroutine(CheckSound());
    }

    IEnumerator CheckSound()
    {
        yield return new WaitForSeconds(0.1f);
        if (!audioSource.isPlaying)
        {
            Destroy(this.gameObject);
        }
        StartCoroutine(CheckSound());
    }
}
