using UnityEngine;

public class CubicCurveExemple : MonoBehaviour
{
    [SerializeField] Transform[] points; 
    Vector3[] savedPointsPosition = new Vector3[4];
    [SerializeField] GameObject squar;

    float t = 0f;
    bool tDir = true;

    [Range(0, 10)]
    [SerializeField] float speed;

    [SerializeField] LineRenderer lineRenderer;
    int numPoints = 50;
    Vector3[] positions = new Vector3[50];

    void Start() {
        lineRenderer.positionCount = numPoints;
    }

    void Update() {
        if(speed != 0) {

            if(tDir)
                t += Time.deltaTime*speed;

            if(t >= 1) 
                tDir = false;
            else if(t <= 0)
                tDir = true;

            if(!tDir) 
                t -= Time.deltaTime*speed;
        }

        squar.transform.position = BezierCubicAlgorithm(t);

        for(int i=0; i<points.Length; i++) {
            if(points[i].position != savedPointsPosition[i]) {
                for(int j=0; j<points.Length; j++) {
                    savedPointsPosition[j] = points[j].position;
                }
                
                DrawCubicCurve();
            }
        }
    }

    void DrawCubicCurve() {
        for(int i=0; i<numPoints; i++) {
            float _t = i / (float)numPoints;
            positions[i] = BezierCubicAlgorithm(_t);
        }
        lineRenderer.SetPositions(positions);
    }

    Vector3 BezierCubicAlgorithm(float currentT) {
        Vector3 a = Vector3.Lerp(points[0].position, points[1].position, currentT);
        Vector3 b = Vector3.Lerp(points[1].position, points[2].position, currentT);
        Vector3 c = Vector3.Lerp(points[2].position, points[3].position, currentT);
        Vector3 d = Vector3.Lerp(a, b, currentT);
        Vector3 e = Vector3.Lerp(b, c, currentT);

        return Vector3.Lerp(d, e, currentT);
    }

    void OnDrawGizmos() {
        if(points == null || points.Length != 4) return;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(points[0].position, points[1].position);
        Gizmos.DrawLine(points[2].position, points[3].position);
    }
}