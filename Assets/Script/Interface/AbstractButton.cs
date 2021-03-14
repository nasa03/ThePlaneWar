using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractButton : MonoBehaviour
{
    [SerializeField] protected Image image;
    [SerializeField] protected GameObject button;
    protected ButtonType _buttonType = ButtonType.Normal;
    protected float _time = 0.0f;
    protected const int ProcessingMaxTime = 10;
    protected const int CoolingMaxTime = 10;

    protected enum ButtonType
    {
        Normal,
        Processing,
        Cooling
    }

    public abstract void ProcessingStart();
    public abstract void CoolingStart();
    public abstract void CoolingEnd();
}