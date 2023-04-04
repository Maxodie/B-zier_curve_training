using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform point0, point2, pointEnd0;

    [Range(0.0000001f, 100f)]
    [SerializeField] float translateTime = 0.2f;

    int numPoints = 50;
    Vector3[] positions = new Vector3[50];

    bool isHold = false;
    bool canThrowing = true;
    [HideInInspector] public bool isMoving = false;

    Vector3 startHoldPos;

    void Start() {
        lineRenderer.positionCount = numPoints;
        lineRenderer.enabled = false;

        DrawQuadCurve();
    }

    void Update() {
        if(canThrowing) {
            if(Input.GetKeyDown(KeyCode.Mouse0)) {
                isHold = true;
                startHoldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.enabled = true;
            }
            else if(Input.GetKeyUp(KeyCode.Mouse0)) {
                isHold = false;
                canThrowing = false;
                lineRenderer.enabled = false;
            }
            if(isHold)
                MovePoints();
        }
        else {
            if(!isMoving)
                StartCoroutine(MovBall());
        }
    }

    IEnumerator MovBall() {
        isMoving = true;
        float t = 0f;
        Vector3 lastMov = new Vector3();
        while(t < 1f) {
            t += Time.deltaTime*translateTime;
            lastMov = CalculateQuadraticBezierPoint(t, point0.position, point2.position, pointEnd0.position);
            transform.localPosition = lastMov;
            yield return null;

            if(!isMoving)
                break;
        }
        isMoving = false;
        t = 0;
        canThrowing = true;
    }

    void MovePoints() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        point0.position = transform.position;

        float xdif = startHoldPos.x-mousePos.x;
        float ydif = startHoldPos.y-mousePos.y;

        Vector3 point2Dif = new Vector3(xdif, ydif);
        point2.position = transform.position + point2Dif*2;

        pointEnd0.position = new Vector3(point2.position.x+xdif, point0.position.y);
        DrawQuadCurve();
    }

    void DrawQuadCurve() {
        for(int i=0; i<numPoints; i++) {
            float t = i / (float)numPoints;
            positions[i] = CalculateQuadraticBezierPoint(t, point0.position, point2.position, pointEnd0.position);
        }
        lineRenderer.SetPositions(positions);
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
        // B(t) = (1-t)²p0 + 2(1-t)tp1 + t²p2

        float u = 1-t;
        return u*u*p0 + 2*u*t*p1 + t*t*p2;
    }
}
