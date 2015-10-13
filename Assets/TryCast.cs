using UnityEngine;
using System.Collections;

public class TryCast : MonoBehaviour {

    private Collider2D col;
    private PolygonCollider2D Pcol;
    private float speed = 0.02f;
	// Use this for initialization
	void Start () {

        col = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0);
        }
        //if (col.GetType().Name == "BoxCollider2D") 
        //{
        //    Vector3[] points = new Vector3[4];
        //    Vector2 size = col.bounds.size;
        //    BoxCollider2D collider = (BoxCollider2D)col;
        //    float top = collider.offset.y + (collider.size.y / 2f);
        //    float btm = collider.offset.y - (collider.size.y / 2f);
        //    float left = collider.offset.x - (collider.size.x / 2f);
        //    float right = collider.offset.x + (collider.size.x / 2f);

        //    Vector3 topLeft = transform.TransformPoint(new Vector3(left, top, 0f));
        //    Vector3 topRight = transform.TransformPoint(new Vector3(right, top, 0f));
        //    Vector3 btmLeft = transform.TransformPoint(new Vector3(left, btm, 0f));
        //    Vector3 btmRight = transform.TransformPoint(new Vector3(right, btm, 0f));
        //    points[0] = transform.TransformPoint((Vector3)(col.offset + new Vector2(size.x / 2f, size.y / 2f)));
        //    //points[1] = col.transform.TransformPoint(col.offset + new Vector2(size.x, size.y));
        //    //points[2] = col.transform.TransformPoint(col.offset + new Vector2(size.x, size.y));
        //    //points[3] = col.transform.TransformPoint(col.offset + new Vector2(size.x, size.y));
        //    print("(" + topLeft.x.ToString() + ", " + topLeft.y + ", " + topLeft.z.ToString() + ")");
        //    //print("(" + points[1].x.ToString() + ", " + points[1].y + ", " + points[1].z.ToString() + ")");
        //    //print("(" + points[2].x.ToString() + ", " + points[2].y + ", " + points[2].z.ToString() + ")");
        //    //print("(" + points[3].x.ToString() + ", " + points[3].y + ", " + points[3].z.ToString() + ")");
        //}
	}
}
