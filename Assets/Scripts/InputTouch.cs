using UnityEngine;


public class InputTouch : MonoBehaviour
{
    public float SpeedZoom;
    
    private bool drag;
    private bool zoom;
    private Vector2 _startPosition;
    private Vector2 _lastPosition;
    private Vector2 _endPosition;
    private Vector2 _delta;
    
    private float _lastDistanceTouch;
    private Vector2 _initialTouch0Position;
    private Vector2 _initialTouch1Position;

    private void Touch()
    {
        if (Input.touchCount == 0)
        {
            _endPosition = _lastPosition;
            drag = false;
            zoom = false;
        }

        if (Input.touchCount == 1)
        {
            zoom = false;
            Touch touch0 = Input.GetTouch(0);
            if (!drag)
            {
                _startPosition = touch0.position;
                _lastPosition = touch0.position;
                drag = true;
            }

            if (drag && touch0.position != _lastPosition)
            {
                _delta = touch0.position - _lastPosition;
                _lastPosition = touch0.position;
                //Теперь можно тут это как то использовать
            }
        }

        if (Input.touchCount == 2)
        {
            drag = false;

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (!zoom)
            {
                _initialTouch0Position = touch0.position;
                _initialTouch1Position = touch1.position;
                _lastDistanceTouch = Vector3.Distance(_initialTouch0Position, _initialTouch1Position);
                zoom = true;
            }
            else
            {
                float _currentDistance = Vector3.Distance(touch0.position, touch1.position);
                float deltaDistance = _currentDistance - _lastDistanceTouch;
                _lastDistanceTouch = _currentDistance;
                Vector3 midle = new Vector3((_initialTouch0Position.x + _initialTouch1Position.x) / 2f,
                    (_initialTouch0Position.y + _initialTouch1Position.y) / 2f, 0);

                float delta = SpeedZoom * Time.deltaTime;

                if (deltaDistance > 0.01f)
                {
                    //Тут уже увеличиваем спрайт
                }

                if (deltaDistance < 0)
                {
                    //А Тут уменьшаем спрайт
                }
            }
        }
        else
        {
            zoom = false;
        }
    }
}