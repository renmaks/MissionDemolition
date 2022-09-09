using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set In Inspector")]
    [SerializeField] private GameObject cloudSphere;

    private const int _numSpheresMin = 6;
    private const int _numSpheresMax = 10;
    private readonly Vector3 _sphereOffsetScale = new Vector3(5, 2, 1);
    private readonly Vector2 _sphereScaleRangeX = new Vector2(4, 8);
    private readonly Vector2 _sphereScaleRangeY = new Vector2(3, 4);
    private readonly Vector2 _sphereScaleRangeZ = new Vector2(2, 4);
    private const float _scaleYMin = 2f;
    private List<GameObject> _spheres;


    private void Start()
    {
        _spheres = new List<GameObject>();

        int num = Random.Range(_numSpheresMin, _numSpheresMax);
        for(int i = 0; i < num; i++)
        {
            var sp = Instantiate<GameObject>(cloudSphere);
            _spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(this.transform);

            // Выбрать случайное местоположение
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= _sphereOffsetScale.x;
            offset.y *= _sphereOffsetScale.y;
            offset.z *= _sphereOffsetScale.z;
            spTrans.localPosition = offset;

            // Выбрать случайный масштаб
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(_sphereScaleRangeX.x, _sphereScaleRangeX.y);
            scale.y = Random.Range(_sphereScaleRangeY.x, _sphereScaleRangeY.y);
            scale.z = Random.Range(_sphereScaleRangeZ.x, _sphereScaleRangeZ.y);

            // Скорректировать масштаб y по расстоянию x от центра
            scale.y *= 1 - (Mathf.Abs(offset.x) / _sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, _scaleYMin);

            spTrans.localScale = scale;
        }
    }
}
