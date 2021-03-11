using AirCoder.Core;
using UnityEditor;
using UnityEngine;

namespace AirCoder.Editor
{
    public class GameColliderGizmos 
    {
       /* [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(GameCollider scr, GizmoType gizmoType)
        {
            if(scr == null) return;
            Vector3 position = scr.targetGo.transform.position;

            if (Vector3.Distance(position, Camera.current.transform.position) > 10f)
                Gizmos.DrawIcon(position, "MyScript Gizmo.tiff");
            
            Gizmos.color = Color.green;
            Gizmos.DrawCube(position, scr.size);
        }*/
    }
}
