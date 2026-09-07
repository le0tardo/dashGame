using UnityEngine;

public class TestButtonAction : MonoBehaviour, IButtonAction
{
    public void ButtonAction(bool pressed)
    {
        if(pressed)transform.localScale = new Vector3(2, 2, 2);
        else transform.localScale = new Vector3(1, 1, 1);
    }
}
