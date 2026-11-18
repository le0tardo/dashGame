using UnityEngine;

public class CurrentLobbyPet : MonoBehaviour
{
    [SerializeField] Transform petPosition;
    [SerializeField] GameObject currentPet;

    public void SpawnPet(GameObject newPet)
    {
        if(currentPet!=null)Destroy(currentPet);
        currentPet = Instantiate (newPet, petPosition.position, Quaternion.identity);
    }
}
