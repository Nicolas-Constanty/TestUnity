using UnityEngine;
using System.Collections;

public class IA_Robot : MonoBehaviour {

    public float speed;
    private bool init = false;
    private Vector2[] limit_x = new Vector2[2];
    private bool right = true;
    private float size;
    private bool wait = false;
	// Update is called once per frame
    void Start() {
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * getDir(right), scale.y, scale.z);
        size = GetComponent<Collider2D>().bounds.size.x / 2;
    }
    private int getDir(bool b) {
        return b ? -1 : 1;
    }
	void Update () {

        if (init)
        {
            float distance_left = Mathf.Sqrt(Mathf.Pow(limit_x[0].x - transform.position.x + size, 2));
            float distance_right = Mathf.Sqrt(Mathf.Pow(limit_x[1].x - transform.position.x - size, 2));
            print(distance_right);
            if (!wait && ((distance_left <= 0.1f && right) || (distance_right <= 0.1f && !right)))
            {
                right = !right;
                Vector3 scale = transform.localScale;
                transform.localScale = new Vector3(scale.x * -1, scale.y , scale.z);
            }
            if (right)
            {
                transform.Translate(Vector2.left * Time.deltaTime * speed);
            }
            else if (!right)
            {
                transform.Translate(Vector2.right * Time.deltaTime * speed);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D col) {

        if (col.collider.CompareTag("Ground")) {
            init = true;
            if (col.collider.GetType() == typeof(PolygonCollider2D)) {
                PolygonCollider2D poly = (PolygonCollider2D) col.collider;
                Vector2[] points = poly.points;
                limit_x[0] = col.transform.TransformPoint(points[0]);
                limit_x[1] = col.transform.TransformPoint(points[0]);
                Vector2 point;
                for (int i = 1; i < points.Length; i++)
                {
                    point = points[i];
                    Vector2 wpoint = col.transform.TransformPoint(point);
                    if (wpoint.x < limit_x[0].x)
                    {
                        limit_x[0] = wpoint;
                    }
                    if (wpoint.x > limit_x[1].x)
                    {
                        limit_x[1] = wpoint;
                    }
                }
            }
            else if (col.collider.GetType() == typeof(BoxCollider2D))
            {
                BoxCollider2D box = (BoxCollider2D) col.collider;
                Vector2[] points = new Vector2[4];
                Vector2 size = box.size;
                points[0] = box.transform.TransformPoint(box.offset + new Vector2(0.5f * size.x, 0.5f * size.y));
                points[1] = box.transform.TransformPoint(box.offset + new Vector2(-0.5f * size.x, 0.5f * size.y));
                points[2] = box.transform.TransformPoint(box.offset + new Vector2(0.5f * size.x, -0.5f * size.y));
                points[3] = box.transform.TransformPoint(box.offset + new Vector2(-0.5f * size.x, -0.5f * size.y));
                limit_x[0] = points[0];
                limit_x[1] = points[0];
                Vector2 point;
                for (int i = 1; i < 4; i++)
                {
                    point = points[i];
                    if (point.x < limit_x[0].x)
                    {
                        limit_x[0] = point;
                    }
                    if (point.x > limit_x[1].x)
                    {
                        limit_x[1] = point;
                    }
                }
            }
            print(limit_x[0]);
            print(limit_x[1]);
        }
    }
}
