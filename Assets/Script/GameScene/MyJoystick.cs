using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class MyJoystick : ScrollRect
{
    float mRadius = 0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        var contentPostion = this.content.anchoredPosition;
        if (contentPostion.magnitude > mRadius)
        {
            contentPostion = contentPostion.normalized * mRadius;
            SetContentAnchoredPosition(contentPostion);
        }
    }

    private void Update()
    {
        float x = content.anchoredPosition.x / mRadius;
        float y = content.anchoredPosition.y / mRadius;
        CrossPlatformInputManager.SetAxis("Horizontal", x);
        CrossPlatformInputManager.SetAxis("Vertical", y);
    }
}
