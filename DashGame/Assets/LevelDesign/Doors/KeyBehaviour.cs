using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    [SerializeField] ParticleSystem pickupParticle;
    [SerializeField] AudioClip pickupAudio;
    MeshRenderer mesh;
    [SerializeField] bool pickedUp=false;
    private void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("player ociked up key");

            if (pickedUp) return;
            pickedUp = true;
            LevelManager.inst.AddKey();
            //Destroy(this.gameObject);
            if (pickupParticle != null) pickupParticle.gameObject.SetActive(true); pickupParticle.Play();
            if (pickupAudio != null) AudioManager.inst.PlayCustomSound(pickupAudio, 0.5f);
            Invoke(nameof(Sleep), 0.25f);
        }
    }

    void Sleep()
    {
        this.gameObject.SetActive(false);
    }
}
