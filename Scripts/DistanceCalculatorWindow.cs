using UnityEditor;
using UnityEngine;

namespace JaxterMG.SimpleRuler.DistanceCalculator
{

    public class DistanceCalculatorWindow : EditorWindow
    {
        GameObject point1;
        GameObject point2;

        [MenuItem("Window/Distance Calculator")]
        public static void ShowWindow()
        {
            GetWindow<DistanceCalculatorWindow>("Distance Calculator");
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            GUILayout.Label("Select the objects to calculate distance", EditorStyles.boldLabel);

            point1 = EditorGUILayout.ObjectField("Point 1", point1, typeof(GameObject), true) as GameObject;
            point2 = EditorGUILayout.ObjectField("Point 2", point2, typeof(GameObject), true) as GameObject;

            if (GUILayout.Button("Calculate Distance"))
            {
                if (point1 && point2)
                {
                    float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
                    EditorUtility.DisplayDialog("Distance", "The distance is: " + distance, "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Both points are required!", "OK");
                }
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (point1 && point2)
            {
                DrawArrow(point1.transform.position, point2.transform.position, Color.red, 0.01f, 135f);
                float distance = Vector3.Distance(point1.transform.position, point2.transform.position);
                Vector3 labelPos = Vector3.Lerp(point1.transform.position, point2.transform.position, 0.5f);
                GUIStyle labelStyle = new GUIStyle();
                labelStyle.normal.textColor = Color.white;
                labelStyle.fontSize = 16;
                labelStyle.fontStyle = FontStyle.Bold;
                Handles.Label(labelPos, $"Distance: {distance*1000:F0}", labelStyle);
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