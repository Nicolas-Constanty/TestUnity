using UnityEngine;
using System.Collections;

public class TryCast : MonoBehaviour {

    private Collider2D col;
    private PolygonCollider2D Pcol;
	// Use this for initialization
	void Start () {

        col = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (col.GetType().Name == "BoxCollider2D") 
        {
            Vector3[] points = new Vector3[4];
            Vector2 size = col.bounds.size;
            points[0] = col.offset + new Vector2(0.5f * size.x, 0.5f * size.y) + (Vector2)col.transform.position;
            points[1] = col.offset + new Vector2(-0.5f * size.x, 0.5f * size.y) + (Vector2)col.transform.position;
            points[2] = col.offset + new Vector2(0.5f * size.x, -0.5f * size.y) + (Vector2)col.transform.position;
            points[3] = col.offset + new Vector2(-0.5f * size.x, -0.5f * size.y) + (Vector2)col.transform.position;
            print(points[0]);
            print(points[1]);
            print(points[2]);
            print(points[3]);
        }
	}
}
