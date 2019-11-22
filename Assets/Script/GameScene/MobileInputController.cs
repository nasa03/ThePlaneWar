using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MobileInputController : MonoBehaviour {

    public RectTransform Background;
    public RectTransform Knob;
    [Header("Input Values")]
    public float Horizontal = 0;
    public float Vertical = 0;

    public float offset;
    Vector2 PointPosition;

    // Update is called once per frame
    void Update () {

        if (Input.touchCount != 1)
            return;

        Touch[] touches = Input.touches;
        foreach (Touch touch in touches)
        {
            if (touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended)
            {
                Vector2 position = touch.position;

                if (position.x > Background.position.x - (Background.rect.width / 2) && position.x < Background.position.x + (Background.rect.width / 2)
                && position.y > Background.position.y - (Background.rect.height / 2) && position.y < Background.position.y + (Background.rect.height / 2))
                {
                    PointPosition = new Vector2((position.x - Background.position.x) / ((Background.rect.size.x - Knob.rect.size.x) / 2), (position.y - Background.position.y) / ((Background.rect.size.y - Knob.rect.size.y) / 2));
                    PointPosition = (PointPosition.magnitude > 1.0f) ? PointPosition.normalized : PointPosition;
                    Knob.transform.position = new Vector2((PointPosition.x * ((Background.rect.size.x - Knob.rect.size.x) / 2) * offset) + Background.position.x, (PointPosition.y * ((Background.rect.size.y - Knob.rect.size.y) / 2) * offset) + Background.position.y);
                }
            }
            else
            {
                PointPosition = new Vector2(0f, 0f);
                Knob.transform.position = Background.position;
            }
        }


        Horizontal = PointPosition.x;
        Vertical = PointPosition.y;

        CrossPlatformInputManager.SetAxis("Horizontal", Horizontal);
        CrossPlatformInputManager.SetAxis("Vertical", Vertical);
    }
}
