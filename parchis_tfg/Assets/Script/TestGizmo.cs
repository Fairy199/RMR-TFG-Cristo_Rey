using UnityEngine;

public class TestGizmo : MonoBehaviour
{
    public Transform target;

    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
