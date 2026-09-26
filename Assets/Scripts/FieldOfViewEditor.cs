using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BoidsMovement))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        BoidsMovement fow = (BoidsMovement)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);

        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;

        foreach (GameObject visibleTarget in fow.visibleBoids)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.transform.position);
        }

    }
}
