using UnityEngine;

public class RigidbodySleep : MonoBehaviour
{
    private void Start()
    {
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.Sleep();
        }
    }
}
