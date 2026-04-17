using UnityEngine;
using UnityEngine.UI;



    [RequireComponent(typeof(Button))]
    public abstract class OnClickButtonHandler : MonoBehaviour
{
    protected Button _button;

    protected virtual void Awake() => _button = GetComponent<Button>();

    protected virtual void Start() => _button.onClick.AddListener(ButtonAction);

    protected void OnDestroy() => _button.onClick.RemoveListener(ButtonAction);

    protected abstract void ButtonAction();
}
