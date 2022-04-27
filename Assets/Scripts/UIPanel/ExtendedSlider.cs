using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 拓展Slider，增加添加事件
/// </summary>
public class ExtendedSlider :Slider
{
    public UnityEvent pointerDown;
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        pointerDown?.Invoke();
    }
}
