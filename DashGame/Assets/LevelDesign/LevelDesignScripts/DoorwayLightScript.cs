using UnityEngine;
using System.Collections;

public class DoorwayLightScript : MonoBehaviour
{
    [SerializeField] Light[] doorwayLights;
    [SerializeField] LayerMask hittableLayer;
    [SerializeField] private float rayDistance = 5f;

    float fadeDuration = 0.25f;

    private void Start()
    {
        UpdateDoorwayLights();
    }


    public void SnapLightsToNewRoom(Vector3 pos)
    {
        transform.position = pos;
    }
    public void UpdateDoorwayLights()
    {
        if (doorwayLights == null || doorwayLights.Length == 0) return;

        for (int i = 0; i < doorwayLights.Length; i++)
        {
            if (doorwayLights[i] == null) continue;

            Vector3 origin = doorwayLights[i].transform.position;
            Vector3 direction = doorwayLights[i].transform.forward;

            bool hitSomething = Physics.Raycast(origin, direction, rayDistance, hittableLayer);
            doorwayLights[i].enabled = !hitSomething;

            Light thisLight= doorwayLights[i];
            thisLight.intensity = 0f;
            StartCoroutine(FadeLights(thisLight, 66f));
        }
    }

    IEnumerator FadeLights(Light light, float targetStrength)
    {
        if (light == null) yield break;

        float startIntensity = light.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Smoothly interpolate intensity over fadeDuration
            light.intensity = Mathf.Lerp(startIntensity, targetStrength, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure it snaps exactly to the target value at the end
        light.intensity = targetStrength;
    }
}
