using UnityEngine;

public class GUInterface : MonoBehaviour
{
    [SerializeField] private BodyPhysics bodyPhysics;
    [SerializeField] private Rigidbody targetRigidbody;

    private void OnGUI()
    {
        if (GUILayout.Button("Apply Unity Force") && targetRigidbody != null)
        {
            targetRigidbody.AddForce(transform.right * 10, ForceMode.Force);
        }

        if (GUILayout.Button("Apply Custom Force") && bodyPhysics != null)
        {
            bodyPhysics.AddForce(transform.right * 10);
        }
    }
}
