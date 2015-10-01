using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vertice
{
    public Vector3 pos {get;set;}
    public float   angle {get;set;}
}

public class NewLight : MonoBehaviour {

    public float angle = 15f;
    public float distance = 5;
    public int segments = 2;
    private Mesh mesh;
    private float inc;
    private List<Vertice> Vertex = new List<Vertice>();
    private int total_points;
	// Use this for initialization
	void Start () {
        
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {

        if (segments < 2)
        {
            segments = 2;
        }
        inc = angle / (segments - 1);
        float rot_euler_z = transform.rotation.eulerAngles.z;
        Vertex.Clear();
        Vertice pivot_vertice = new Vertice();
        pivot_vertice.angle = 0;
        pivot_vertice.pos = Vector3.zero;
        Vertex.Add(pivot_vertice);
        for (float i = 0; i <= angle; i += inc)
        {
            
            Vertice new_vertice = new Vertice();
            float pos_x = distance * Mathf.Cos(Mathf.Deg2Rad * (i + rot_euler_z));
            float pos_y = distance * Mathf.Sin(Mathf.Deg2Rad * (i + rot_euler_z));
            Vector3 point = new Vector3(pos_x, pos_y, 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformPoint(point) - transform.position, distance);
            if (hit)
            {
                if (hit.collider.GetType() == typeof(BoxCollider2D))
                {
                    new_vertice.pos = hit.point;
                    Vector2[] points = GetBoxCollider2DPoints((BoxCollider2D)hit.collider);
                    //Vertice new_sub_verice = new Vertice();
                    //for (int j = 0; j < points.Length; j++)
                    //{
                    //    RaycastHit2D sub_hit = Physics2D.Raycast(transform.position, transform.TransformPoint(points[j]) - transform.position, distance);
                    //    if (sub_hit.collider) {
                    //        new_sub_verice.pos = sub_hit.point;
                    //        Debug.DrawLine(transform.position, new_sub_verice.pos, Color.green);
                    //        new_sub_verice.pos = transform.InverseTransformPoint(new_sub_verice.pos);
                    //        Vertex.Add(new_sub_verice);
                    //    } 
                    //}
                }
                else if (hit.collider.GetType() == typeof(PolygonCollider2D))
                {
                    new_vertice.pos = point;
                    GetPolygonCollider2DPoints((PolygonCollider2D)hit.collider);
                }
            }
            else {
                new_vertice.pos = transform.TransformPoint(point);
            }
            Debug.DrawLine(transform.position, new_vertice.pos, Color.blue);
            new_vertice.pos = transform.InverseTransformPoint(new_vertice.pos);
            Vertex.Add(new_vertice);
        }
        total_points = Vertex.Count;
        //SortVertex();
        CreateMesh(ListToArray());
	}
    private Vector2[] GetBoxCollider2DPoints(BoxCollider2D col) {

        Vector2[] points = new Vector2[4];
        Vector2 size = col.bounds.size;
        points[0] = col.offset + new Vector2(0.5f * size.x, 0.5f * size.y) + (Vector2)col.transform.position;
        points[1] = col.offset + new Vector2(-0.5f * size.x, 0.5f * size.y) + (Vector2)col.transform.position;
        points[2] = col.offset + new Vector2(0.5f * size.x, -0.5f * size.y) + (Vector2)col.transform.position;
        points[3] = col.offset + new Vector2(-0.5f * size.x, -0.5f * size.y) + (Vector2)col.transform.position;
        return points;
    }

    private Vector2[] GetPolygonCollider2DPoints(PolygonCollider2D col) {

        int size = col.GetTotalPointCount();
        Vector2[] points = new Vector2[size];
        Vector2[] local_points = col.points;
        for (int i = 0; i < size; i++)
        {
            points[i] = col.transform.TransformPoint(local_points[i]);
        }
        print(points[0].x);
        print(points[0].y);
        return col.points;
    }

    private void CreateMesh(Vector3[] vertex)
    {
        mesh.vertices = vertex;
        int[] triangles = new int[total_points * 3];
        int j = 0;
        for (int i = 0; i < total_points; i++)
        {
            triangles[j] = i;
            triangles[j + 1] = i + 1;
            triangles[j + 2] = 0;
            j += 3;
        }
        mesh.triangles = triangles;
    }

    private void SortVertex() {
        Vertex.Sort((item1, item2) => (item2.angle.CompareTo(item1.angle)));
    }

    private Vector3[] ListToArray()
    {
        Vector3[] array_vertex = new Vector3[total_points + 1];
        for (int i = 0; i < total_points; i++)
        {
            array_vertex[i] = Vertex[i].pos;
        }
        return array_vertex;
    }
}
