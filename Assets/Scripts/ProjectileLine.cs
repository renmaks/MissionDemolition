using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    public static ProjectileLine SingletoneProjectileLine; // Одиночка

    private const float _minDistance = 0.1f;
    private LineRenderer _line;
    private GameObject _poi;
    private List<Vector3> _points;
    
    private void Awake()
    {
        SingletoneProjectileLine = this; // Установить ссылку на объект-одиночку
        
        // Получить ссылку на LineRenderer
        _line = GetComponent<LineRenderer>();
        // Выключить LineRenderer, пока он не понадобится
        _line.enabled = false;
        // Инициализировать список точек
        _points = new List<Vector3>();
    }

    // Это свойство ( метод, маскирующийся под поле )
    public GameObject poi
    {
        get => (_poi);
        set
        {
            _poi = value;
            if (_poi != null)
            {
                // Если поле _poi содержит действительную ссылку, сбросить все остальные параметры в исходное состояние
                _line.enabled = false;
                _points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // Этот метод можно вызвать непосредственно, чтобы стереть линию
    public void Clear()
    {
        _poi = null;
        _line.enabled = false;
        _points = new List<Vector3>();
    }

    private void AddPoint()
    {
        // Вызывается для добавления точки в линии
        Vector3 pt = _poi.transform.position;
        if (_points.Count > 0 && (pt - lastPoint).magnitude < _minDistance )
        {
            // Если точка недостаточно далека от предыдущей, просто выйти
            return;
        }

        if (_points.Count == 0)
        {
            // Если это точка запуска...
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // Для определения 
            // ...добавить новый фрагмент линии, чтобы помочь лучше прицелиться в будущем
            _points.Add(pt + launchPosDiff);
            _points.Add(pt);
            _line.positionCount = 2;
            // Установить первые две точки
            _line.SetPosition(0, _points[0]);
            _line.SetPosition(1, _points[1]);
            // Включить LineRenderer
            _line.enabled = true;
        }
        else
        {
            // Обычная последовательность добавления точки
            _points.Add(pt);
            _line.positionCount = _points.Count;
            _line.SetPosition(_points.Count - 1, lastPoint);
            _line.enabled = true;
        }
    }

    // Возвращает местоположение последней добавленной точки
    private Vector3 lastPoint
    {
        get
        {
            if (_points == null)
            {
                // Если точек нет, вернуть Vector3.zero
                return (Vector3.zero);
            }
            return (_points[^1]);
        }
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            // Если свойство poi содержит пустое значение, найти интересующий объект
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; // Выйти, если интересующий объект не найден
                }
            }
            else
            {
                return;
            }
        }
        // Если интересующий объект найден, попытаться добавить точку с его координатами в каждом FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            // Если FollowCam.POI содержит null, записать null в poi
            poi = null;
        }
    }
}
