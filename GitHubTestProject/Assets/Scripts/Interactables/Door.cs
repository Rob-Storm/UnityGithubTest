using UnityEngine;
using UnityEngine.AI;

public class Door : Interactable
{
    [Header("General Stuff")]

    [Tooltip("Where the Door Rotates From")]
    public Transform hinge;

    [Tooltip("Does the door start open?")]
    public bool isOpen;

    [Tooltip("Does the door start locked? (Best that it starts closed too!)")]
    public bool isLocked;

    [Tooltip("The Closed Position, e.g. 90 degrees. NOT Relative")]
    public float startPosition;

    [Tooltip("The Opened Position, e.g. 90 degrees. NOT Relative")]
    public float endPosition;

    [Header("Sound and Text")]

    [Tooltip("Where the sounds are played from")]
    public AudioSource audioSource;

    [Tooltip("The sound that plays when you open or close the door")]
    public AudioClip useSound;

    [Tooltip("The sound that plays when the door is locked")]
    public AudioClip lockSound;

    [Tooltip("The name of the door that shows up in the interaction text")]
    public string doorName;

    [Tooltip("The message for when the door is locked that shows up in the interaciton text (e.g \"Electronically Locked\") DONT use punctuation as that is added at the end")]
    public string lockMessage = "Locked";

    [Tooltip("Ignore.")]
    public NavMeshObstacle obstacle;

    [Tooltip("Ignore.")]
    public LayerMask enemyLayer;

    private void Start()
    {
        obstacle.enabled = false;

        if (isLocked)
        {
            doorName = $"<color=blue>{doorName}</color>";
            return;
        }
        
        if( isOpen )
            hinge.rotation = Quaternion.Euler(0, endPosition, 0);

        else
            hinge.rotation = Quaternion.Euler(0, startPosition, 0);

        if(!isLocked)
        {
            doorName = "Door";
        }
    }

    void UpdateDoor()
    {
        if (!isLocked)
        {
            if (isOpen) OpenDoor();
            else CloseDoor();
        }
        else
        {
            audioSource.PlayOneShot(lockSound);
            return;
        }
    }

    public override string GetDescription()
    {
        if (isLocked)
        {
            return $"The {doorName} is <color=blue>{lockMessage}</color>.";
        }
        if (isOpen)
        {
            return $"Right Click to <color=red>close</color> the {doorName}.";
        }
        else
        {
            return $"Right Click to <color=green>open</color> the {doorName}.";
        }
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        UpdateDoor();
    }

    public void OpenDoor()
    {
        hinge.rotation = Quaternion.Euler(0, endPosition, 0);
        audioSource.PlayOneShot(useSound);
    }

    public void CloseDoor()
    {
        hinge.rotation = Quaternion.Euler(0, startPosition, 0);
        audioSource.PlayOneShot(useSound);
    }

    private void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.layer == enemyLayer) 
        {
            Debug.Log("Enemy did shit");
            Invoke(nameof(OpenDoor), 1);
        }
    }
}