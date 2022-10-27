using UnityEngine;

public class AimLine : MonoBehaviour
{
    private LineRenderer _thisLine;
    private Transform _projectile;

    private void Awake()
    {
        _thisLine = GetComponent<LineRenderer>();
        _thisLine.enabled = false;
    }

    private void Update()
    {
        if (Slingshot.singletoneSlingshot.aimingMode)
        {
            _projectile = Slingshot.singletoneSlingshot.projectile.transform;
            _thisLine.SetPosition(1,_projectile.position);
            _thisLine.enabled = true;
        }
        else
        {
            _thisLine.enabled = false;
        }
    }


}
