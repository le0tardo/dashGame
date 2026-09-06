using UnityEngine;

public class TestButtonAction : MonoBehaviour, IButtonAction
{
    public void ButtonAction()
    {
        transform.localScale = new Vector3(2, 2, 2);
    }
}
