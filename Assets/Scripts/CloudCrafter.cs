using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private GameObject _cloudPrefab; // Шаблон для облаков
    
    private readonly Vector3 _cloudPosMin = new Vector3(-50, -5, 10);
    private readonly Vector3 _cloudPosMax = new Vector3(150, 100, 10);
    private const float _cloudScaleMin = 1; // Мин. масштаб каждого облака
    private const float _cloudScaleMax = 3; // Макс. масштаб каждого облака
    private const float _cloudSpeedMult = 1f; // Коэффицент скорости облаков
    private GameObject[] _cloudInstances;
    private const int _numClouds = 20; // Число облаков 

    private void Awake()
    {
        // Создать массив для хранения всех экземпляров облаков
        _cloudInstances = new GameObject[_numClouds];
        // Создать в цикле заданное кол-во облаков
        for (int i = 0; i < _numClouds; i++)
        {
            // Создать экземпляр cloudPrefab
            var cloud = Instantiate<GameObject>(_cloudPrefab, this.transform, true);
            // Выбрать местоположение для облака 
            Vector3 cloudPosition = Vector3.zero;
            cloudPosition.x = Random.Range(_cloudPosMin.x, _cloudPosMax.x);
            cloudPosition.y = Random.Range(_cloudPosMin.y, _cloudPosMax.y);
            // Масштабировать облако
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(_cloudScaleMin, _cloudScaleMax, scaleU);
            // Меньшие облака ( с меньшим значением scaleU ) должны быть ближе к земле
            cloudPosition.y = Mathf.Lerp(_cloudPosMin.y, cloudPosition.y, scaleU);
            // Меньшие облака должны быть дальше
            cloudPosition.z = 100 - 90 * scaleU;
            // Применить полученные значения координат и масштаба к облаку
            cloud.transform.position = cloudPosition;
            transform.localScale = Vector3.one * scaleVal;
            // Добавить облако в массив cloudInstances
            _cloudInstances[i] = cloud;
        }
    }

    private void Update()
    {
        // Обойти в цикле все созданные облака
        foreach (GameObject cloud in _cloudInstances)
        {
            // Получить масштаб и координаты облака
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // Увеличить скорость для ближних облаков
            cPos.x -= scaleVal * Time.deltaTime * _cloudSpeedMult;
            // Если облако сместилось слишком далеко влево...

            if (cPos.x <= _cloudPosMin.x)
            {
                // Переместить его далеко вправо
                cPos.x = _cloudPosMax.x;
            }

            // Применить новые координаты к облаку
            cloud.transform.position = cPos;
        }
    }
}
