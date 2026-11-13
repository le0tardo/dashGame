using System.Collections.Generic;
using UnityEngine;

public class TabardStitcher : MonoBehaviour
{
    public SkinnedMeshRenderer targetCharacterRenderer;
    public SkinnedMeshRenderer clothingRenderer;

    void Start()
    {
        if (targetCharacterRenderer != null && clothingRenderer != null)
        {
            StitchClothing(clothingRenderer, targetCharacterRenderer);
        }
    }

    public void StitchClothing(SkinnedMeshRenderer sourceClothing, SkinnedMeshRenderer targetBody)
    {
        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>(); //idk wtf this is
        foreach (Transform bone in targetBody.bones)
        {
            boneMap[bone.name] = bone;
        }
        
        //new bones apperantly
        Transform[] newBones = new Transform[sourceClothing.bones.Length];
        for (int i = 0; i < sourceClothing.bones.Length; i++)
        {
            string boneName = sourceClothing.bones[i].name;
            if (boneMap.TryGetValue(boneName, out Transform matchedBone))
            {
                newBones[i] = matchedBone;
            }
            else
            {
                Debug.LogWarning("some bone not found on target character.");
            }
        }

        sourceClothing.bones = newBones;
        if (sourceClothing.rootBone != null && boneMap.TryGetValue(sourceClothing.rootBone.name, out Transform matchedRoot))
        {
            sourceClothing.rootBone = matchedRoot;
        }

        sourceClothing.transform.SetParent(targetBody.transform.parent);
    }
}
