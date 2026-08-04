using UnityEngine;

public class SpringBoardBehaviour : MonoBehaviour
{
    [SerializeField] AudioClip springSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove player=other.gameObject.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.Jump();
                AudioManager.inst.PlayCustomSound(springSound,0.25f);
            }
        }
    }
}
