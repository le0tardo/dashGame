using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ChestActivator : MonoBehaviour
{
    [SerializeField] private int n = -1;
    private RoomBehaviour myRoom = null;
    [SerializeField] private GameObject chest;
    [SerializeField] ParticleSystem openParticles;
    private bool triggered = false;

    private void Awake()
    {
        chest.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            myRoom = other.GetComponent<RoomBehaviour>();
            if (myRoom != null)
            {
                n = myRoom.enemies.Count;
            }
        }
    }

    private void Update()
    {
        if (myRoom == null || n == -1 || triggered) return;

        n = myRoom.enemies.Count;

        if (n == 0)
        {
            chest.transform.SetParent(myRoom.transform, true);
            chest.SetActive(true);

            Animator chestAnim=chest.GetComponent<Animator>();
            chestAnim.SetTrigger("spawn");

            openParticles.Play();
            triggered = true;
            HitFxManager.inst.SpawnChestFX(transform.position, transform.rotation);
            this.gameObject.SetActive(false);
        }
    }
}