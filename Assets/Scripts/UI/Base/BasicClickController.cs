using UnityEngine;
using UnityEngine.EventSystems;



    [RequireComponent(typeof(OnClickButtonHelper))]
    public abstract class BasicClickController : MonoBehaviour, IPointerClickHandler
    {
        protected OnClickButtonHelper buttonHelper;

        protected virtual void Awake()
        {
            buttonHelper = GetComponent<OnClickButtonHelper>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!buttonHelper.Interactable)
                return;

            OnClick();
        }

        protected abstract void OnClick();
    }

