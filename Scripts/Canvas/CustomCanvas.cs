namespace Minh_Trong.Scripts.UIAutoAnimation;

[DefaultExecutionOrder(order: -1)]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class CustomCanvas : MonoBehaviour
{
    [SerializeField]
    private Vector2 aspectRange = new Vector2(0.45f, 0.75f);

    [SerializeField]
    private Vector2 matchRange = new Vector2(0.3f, 1);

    private Canvas       _canvas;
    private CanvasScaler _canvasScaler;

    public Canvas Canvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }
            return _canvas;
        }
    }

    public CanvasScaler CanvasScaler
    {
        get
        {
            if (_canvasScaler == null)
            {
                _canvasScaler = GetComponent<CanvasScaler>();
            }
            return _canvasScaler;
        }
    }

    public void SetupCamera()
    {
        if (Canvas.worldCamera == null)
        {
            Canvas.worldCamera = Camera.main;
        }
    }

    private void Awake()
    {
        CanvasScaler.matchWidthOrHeight = GetMatch(Canvas.worldCamera.aspect);
    }

    private float GetMatch(float aspect)
    {
        aspect = Mathf.Clamp(aspect, aspectRange.x, aspectRange.y);
        float percentage = (aspect - aspectRange.x) / (aspectRange.y - aspectRange.x);
        return matchRange.x + percentage * (matchRange.y - matchRange.x);
    }
}