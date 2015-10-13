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
    private List<Collider2D> colList = new List<Collider2D>();
	// Use this for initialization
	void Start () {
        
        mesh = new Mesh();
        mesh.Optimize();
        GetComponent<MeshFilter>().mesh = mesh;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

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
        int layer_mask = ~LayerMask.GetMask("LumColl");
        for (float i = 0; i <= angle; i += inc)
        {
            
            Vertice new_vertice = new Vertice();
            float pos_x = distance * Mathf.Cos(Mathf.Deg2Rad * (i + rot_euler_z));
            float pos_y = distance * Mathf.Sin(Mathf.Deg2Rad * (i + rot_euler_z));
            Vector3 point = new Vector3(pos_x, pos_y, 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, point - transform.position, distance, layer_mask);
            if (hit)
            {
                if (hit.collider.GetType() == typeof(BoxCollider2D))
                {
                    new_vertice.pos = hit.point;
                    Vector2[] points = GetBoxCollider2DPoints((BoxCollider2D)hit.collider);
                    //Vertice new_sub_vertice = new Vertice();
                    List<Vertice> Vert = new List<Vertice>();
                    for (int j = 0; j < points.Length; j++)
                    {
                        RaycastHit2D sub_hit = Physics2D.Raycast(transform.position, points[j] - (Vector2)transform.position);
                        if (sub_hit.collider && approxHit(points[j], sub_hit.point))
                        {
                            Vertice new_sub_vertice = new Vertice();
                            new_sub_vertice.pos = sub_hit.point;
                            new_sub_vertice.angle = Mathf.Atan2(sub_hit.point.y, sub_hit.point.x);
                            Debug.DrawLine(transform.position, new_sub_vertice.pos, Color.green);
                            new_sub_vertice.pos = transform.InverseTransformPoint(new_sub_vertice.pos);
                            Vert.Add(new_sub_vertice);
                        }
                    }
                    SortVertex(Vert);
                    float last_angle = Vert[Vert.Count - 1].angle * Mathf.Rad2Deg;
                    float sub_pos_x = (distance) * Mathf.Cos(Vert[0].angle);
                    float sub_pos_y = (distance) * Mathf.Sin(Vert[0].angle);
                    Vector3 sub_point = new Vector3(sub_pos_x, sub_pos_y, 0);
                    Debug.DrawLine(Vert[0].pos, sub_point, Color.yellow);
                    sub_pos_x = (distance) * Mathf.Cos(Vert[Vert.Count - 1].angle);
                    sub_pos_y = (distance) * Mathf.Sin(Vert[Vert.Count - 1].angle);
                    sub_point = new Vector3(sub_pos_x, sub_pos_y, 0);
                    Debug.DrawLine(Vert[Vert.Count - 1].pos, sub_point, Color.red);
                    Vertex.AddRange(Vert);
                    print(last_angle);
                    while (i <= angle && i < last_angle)
                    {
                        i += inc;
                    }
                }
                else if (hit.collider.GetType() == typeof(PolygonCollider2D))
                {
                    new_vertice.pos = point;
                    Vector2[] points = GetPolygonCollider2DPoints((PolygonCollider2D)hit.collider);
                    List<Vertice> Vert = new List<Vertice>();
                    for (int j = 0; j < points.Length; j++)
                    {
                        RaycastHit2D sub_hit = Physics2D.Raycast(transform.position, points[j] - (Vector2)transform.position);
                        if (sub_hit.collider && approxHit(points[j], sub_hit.point))
                        {
                            Vertice new_sub_vertice = new Vertice();
                            new_sub_vertice.pos = sub_hit.point;
                            new_sub_vertice.angle = Mathf.Atan2(sub_hit.point.y, sub_hit.point.x);
                            Debug.DrawLine(transform.position, new_sub_vertice.pos, Color.green);
                            new_sub_vertice.pos = transform.InverseTransformPoint(new_sub_vertice.pos);
                            Vert.Add(new_sub_vertice);
                        }
                    }
                    SortVertex(Vert);
                    float last_angle = Vert[Vert.Count - 1].angle * Mathf.Rad2Deg;
                    float sub_pos_x = (distance) * Mathf.Cos(Vert[0].angle);
                    float sub_pos_y = (distance) * Mathf.Sin(Vert[0].angle);
                    Vector3 sub_point = new Vector3(sub_pos_x, sub_pos_y, 0);
                    Debug.DrawLine(Vert[0].pos, sub_point, Color.yellow);
                    sub_pos_x = (distance) * Mathf.Cos(Vert[Vert.Count - 1].angle);
                    sub_pos_y = (distance) * Mathf.Sin(Vert[Vert.Count - 1].angle);
                    sub_point = new Vector3(sub_pos_x, sub_pos_y, 0);
                    Debug.DrawLine(Vert[Vert.Count - 1].pos, sub_point, Color.red);
                    Vertex.AddRange(Vert);
                    print(last_angle);
                    while (i <= angle && i < last_angle)
                    {
                        i += inc;
                    }
                }
            }
            else {
                new_vertice.pos = transform.TransformPoint(point);
                Debug.DrawLine(transform.position, new_vertice.pos, Color.blue);
                new_vertice.pos = transform.InverseTransformPoint(new_vertice.pos);
                Vertex.Add(new_vertice);
            }
        }
        total_points = Vertex.Count;
        //SortVertex(Vertex);
        print(Vertex.Count);
        CreateMesh(ListToArray());
	}

    private bool approxHit(Vector2 point, Vector2 hit) {

 
        return (point == hit || (hit.x < 0.005f + point.x && hit.x > -0.005f + point.x && hit.y < 0.005f + point.y && hit.y > -0.005f + point.y)) ?true:false;
    }

    private Vector2[] GetBoxCollider2DPoints(BoxCollider2D col) {

        Vector2[] points = new Vector2[4];
        Vector2 size = col.size;
        points[0] = col.transform.TransformPoint(col.offset + new Vector2(0.5f * size.x, 0.5f * size.y));
        points[1] = col.transform.TransformPoint(col.offset + new Vector2(-0.5f * size.x, 0.5f * size.y));
        points[2] = col.transform.TransformPoint(col.offset + new Vector2(0.5f * size.x, -0.5f * size.y));
        points[3] = col.transform.TransformPoint(col.offset + new Vector2(-0.5f * size.x, -0.5f * size.y));
        if (!colList.Contains(col))
        {
            colList.Add(col);
            CircleCollider2D circle;
            GameObject empty = new GameObject();
            empty.transform.parent = col.transform;
            empty.layer = LayerMask.NameToLayer("LumColl");
            for (int i = 0; i < 4; i++)
            {
                circle = empty.AddComponent<CircleCollider2D>();
                circle.radius = 0.005f;
                circle.offset = empty.transform.InverseTransformPoint(points[i]);
            }
        }
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
        if (!colList.Contains(col))
        {
            colList.Add(col);
            CircleCollider2D circle;
            GameObject empty = new GameObject();
            empty.transform.parent = col.transform;
            empty.layer = LayerMask.NameToLayer("LumColl");
            for (int i = 0; i < points.Length; i++)
            {
                circle = empty.AddComponent<CircleCollider2D>();
                circle.radius = 0.005f;
                circle.offset = empty.transform.InverseTransformPoint(points[i]);
            }
        }
        return points;
    }

    //private void CreateMesh(Vector3[] vertex)
    //{
    //    mesh.vertices = vertex;
    //    int[] triangles = new int[total_points * 3];
    //    int j = 0;
    //    for (int i = 0; i < total_points; i++)
    //    {
    //        triangles[j] = 0;
    //        triangles[j + 1] = i;
    //        triangles[j + 2] = i + 1;
    //        j += 3;
    //    }
    //    mesh.triangles = triangles;
    //}

    private void CreateMesh(Vector3[] vertex)
    {
        mesh.vertices = vertex;
        int[] triangles = new int[total_points * 3];
        int j = 0;
        for (int i = 0; i < total_points; i += 1)
        {
            triangles[j] = 0;
            triangles[j + 2] = i;
            triangles[j + 1] = i + 1;
            j += 3;
        }
        mesh.triangles = triangles;
    }

    private void SortVertex(List<Vertice> vertex) {
        vertex.Sort((item1, item2) => (item1.angle.CompareTo(item2.angle)));
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
