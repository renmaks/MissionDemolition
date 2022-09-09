using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static GameObject POI; // Ссылка на интересующий объект

    private const float _easing = 0.05f;
    private readonly Vector2 _minXY = Vector2.zero;
    private Camera _cam;

    [Header("Set Dynamically")]
    [SerializeField] private float _camZ; // Желаемая координата Z камеры


    private void Awake()
    {
        _cam = Camera.main;
        _camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        SetInterestPosition(out Vector3 destination);

        // Ограничить X и Y минимальными значениями
        destination.x = Mathf.Max(_minXY.x, destination.x);
        destination.y = Mathf.Max(_minXY.y, destination.y);
        // Определить точку между текущим местоположением камеры и destination
        destination = Vector3.Lerp(transform.position, destination, _easing);
        // Принудительно установить значение desination.z равным camZ, чтобы отодвинуть камеру подальше
        destination.z = _camZ;
        // Поместить камеру в позицию destination
        transform.position = destination;
        // Изменить размер orthographicSize камеры, чтобы земля оставалась в поле зрения
        _cam.orthographicSize = destination.y + 10;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private static void SetInterestPosition(out Vector3 destination)
    {
        if (POI != null)
        {
            // Получить позицию интересующего объекта
            destination = POI.transform.position;
            // Если интересующий объект - снаряд, убедиться, что он остановился
            if (POI.CompareTag("Projectile"))
            {
                // Если он не двигается
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    // Вернуть исходные настройки поля зрения камеры
                    POI = null;
                    // в следующем кадре
                    return;
                }
            }
        }
        else
        {
            destination = Vector3.zero;
        }
    }
}
