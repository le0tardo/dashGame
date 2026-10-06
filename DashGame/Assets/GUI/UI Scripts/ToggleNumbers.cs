using TMPro;
using UnityEngine;

public class ToggleNumbers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void ToggleText()
    {
        if (text != null)
        {

            print("awdwf");
            text.enabled = !text.enabled;
        }

    }
}
