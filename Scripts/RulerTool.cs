#if UNITY_2019_1_OR_NEWER
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
namespace JaxterMG.SimpleRuler.Ruler
{
    [EditorTool("Ruler Tool")]
    public class RulerTool : EditorTool
    {
        GameObject point1, point2;

        void OnEnable()
        {
            ToolManager.activeToolChanged += ActiveToolChanged;
            ActiveToolChanged();
        }

        void OnDisable()
        {
            ToolManager.activeToolChanged -= ActiveToolChanged;
            DestroyPoints();
        }

        void ActiveToolChanged()
        {
            var isToolActive = ToolManager.IsActiveTool(this);
            if (isToolActive)
            {
                Selection.activeGameObject = null;
            }
            else
            {
                DestroyPoints();
            }
        }

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            GameObject point1 = GameObject.Find("Point 1");
            GameObject point2 = GameObject.Find("Point 2");

            if (point1)
            {
                DestroyImmediate(point1);
            }

            if (point2)
            {
                DestroyImmediate(point2);
            }
        }

        void DestroyPoints()
        {
            if (point1)
            {
                DestroyImmediate(point1);
            }

            if (point2)
            {
                DestroyImmediate(point2);
            }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                RaycastHit hit;
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (!point1)
                    {
                        point1 = new GameObject("Point 1");
                        point1.transform.position = hit.point;
                    }
                    else if (!point2)
                    {
                        point2 = new GameObject("Point 2");
                        point2.transform.position = hit.point;
                    }
                }
            }

            if (e.type == EventType.MouseDown && e.button == 2)
            {
                DestroyPoints();
            }


            if (point1 && point2)
            {
                DrawArrow(point1.transform.position, point2.transform.position, Color.red, 0.01f, 135f);
                float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
                Vector3 labelPos = Vector3.Lerp(point1.transform.position, point2.transform.position, 0.5f);
                GUIStyle labelStyle = new GUIStyle();
                labelStyle.normal.textColor = Color.red;
                labelStyle.fontSize = 16;
                labelStyle.fontStyle = FontStyle.Bold;
                Handles.Label(labelPos, $"Distance: {distance:F2}", labelStyle);
            }

        }
        void DrawArrow(Vector3 from, Vector3 to, Color color, float arrowHeadLengthMultiplier = 0.01f, float arrowHeadAngle = 135.0f)
        {
            Handles.color = color;
            Handles.DrawLine(from, to);

            Vector3 direction = to - from;
            float arrowLength = direction.magnitude;

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Handles.DrawLine(to, to + right * arrowHeadLengthMultiplier * arrowLength);
            Handles.DrawLine(to, to + left * arrowHeadLengthMultiplier * arrowLength);
            Handles.DrawLine(from, from - right * arrowHeadLengthMultiplier * arrowLength);
            Handles.DrawLine(from, from - left * arrowHeadLengthMultiplier * arrowLength);
        }
    }
}
#endif