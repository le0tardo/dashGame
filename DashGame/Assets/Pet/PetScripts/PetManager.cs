using UnityEngine;

public class PetManager : MonoBehaviour
{
    SessionData sessionData;
    [SerializeField] PetObject pet;

    [SerializeField] Transform petTransform;

    private void Start()
    {
        sessionData = LevelManager.inst.sessionData;

       if(pet==null) pet=sessionData.pet;

        if(pet!=null)Instantiate(pet.petPrefab, petTransform);
    }
}
