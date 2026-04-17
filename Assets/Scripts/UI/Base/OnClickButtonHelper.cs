using UnityEngine;
using UnityEngine.UI;


    [DisallowMultipleComponent]
    public class OnClickButtonHelper : Image
{
    [SerializeField] private bool interactable = true;

    public bool Interactable
    {
        get => interactable;
        set
        {
            interactable = value;
            raycastTarget = value;
        }
    }
}
