using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyPetManager : MonoBehaviour
{
    public static LobbyPetManager inst;

    [SerializeField] List<PetObject> pets;
    [SerializeField] List<GameObject> petPrefabs;

    [SerializeField] public PetObject currentPet=null;

    [Header("Juice")]
    [SerializeField] Light petLight;
    [SerializeField] ParticleSystem petParticle;

    private void Awake()
    {
        inst = this;
    }

    public void SetPet(PetObject pet)
    {
        currentPet = pet;
        petLight.enabled = true;
        petParticle.Play();
    }

    public void ClearPet(PetObject pet)
    {
        //clear prefab??
        //        petLight.enabled = false;
    }
}
