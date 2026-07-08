using UnityEngine;

public class SpringBoardBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("player on spring board");
            PlayerMove player=other.gameObject.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.Jump();
            }
        }
    }
}
