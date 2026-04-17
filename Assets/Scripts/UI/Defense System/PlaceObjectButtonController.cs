using UnityEngine;

public class PlaceObjectButtonController : BasicClickController, IUISelectable
{
    [SerializeField] private int objectIndex;
    [SerializeField] private UIButtonVisual visual;

    protected override void Awake()
    {
        base.Awake();

        if (visual == null)
            visual = GetComponent<UIButtonVisual>();
    }

    protected override void OnClick()
    {
        PlacementEvents.OnSelectPlacementObject?.Invoke(objectIndex);
    }

    public void OnSelected()
    {
        visual.SetHighlighted(true);
    }

    public void OnDeselected()
    {
        visual.SetHighlighted(false);
    }

    public void OnSubmit()
    {
        visual.PlayPressed();
    }
}