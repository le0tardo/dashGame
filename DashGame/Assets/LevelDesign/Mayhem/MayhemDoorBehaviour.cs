using UnityEngine;

public class MayhemDoorBehaviour : MonoBehaviour
{
    Animator doorAnimator;
    BoxCollider doorCollider;
    [SerializeField] int remainingEnemies;
    [SerializeField] ParticleSystem dust;

    private void Awake()
    {
        doorCollider = GetComponent<BoxCollider>();
        doorCollider.enabled = false;

        doorAnimator=GetComponent<Animator>();
    }

    public void CloseDoor()
    {
        doorCollider.enabled=true;
        doorAnimator.SetTrigger("close");
    }

    public void OpenDoor()
    {
        doorCollider.enabled = false;
        doorAnimator.SetTrigger("open");
    }

    public void Dust()
    {
        if(dust!=null)dust.Play();
    }
}
