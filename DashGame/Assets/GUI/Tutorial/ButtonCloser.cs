using UnityEngine;

public class ButtonCloser : MonoBehaviour
{
    [SerializeField] GameObject closeThis;

    public void CloseThing()
    {
        if(closeThis!=null)closeThis.SetActive(false);
    }
}
