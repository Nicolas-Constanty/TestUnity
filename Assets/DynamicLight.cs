using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DynamicLight : MonoBehaviour {

    public float angle = 15f;
    public float distance = 5;
    public int segments = 2;
    public bool is_static;
    public int recursiv;
    private float inc;
    private Action[] funcs = new Action[2];
    private Mesh mesh;
	// Use this for initialization
	void Start () {
        if (segments < 2)
        {
            segments = 2;
        }
        inc = angle / (segments - 1);
        funcs[0] = () => SetDynamique();
        funcs[1] = () => SetStatic();
        mesh = new Mesh();
        mesh.Optimize();
        mesh.name = mesh.GetInstanceID().ToString();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
	
	// Update is called once per frame
	void Update () {

        funcs[is_static ? 1 : 0].Invoke();
	}

    private void SetStatic() {

        SetRaycast();
    }
    private void SetDynamique()
    {
        if (segments < 2)
        {
            segments = 2;
        }
        inc = angle / (segments - 1);
        SetRaycast();
    }

    private void SetRaycast() {

        Vector3[] vertex = new Vector3[10000];
        Vector3 origin = transform.GetChild(0).position;
        float rot_euler_z = transform.rotation.eulerAngles.z;
        int layer_mask = ~LayerMask.GetMask("LumColl");
        int j = 1;
        bool has_hit = false;
        for (float i = 0; i <= angle; i += inc)
        {
            float pos_x = distance * Mathf.Cos(Mathf.Deg2Rad * (i + rot_euler_z));
            float pos_y = distance * Mathf.Sin(Mathf.Deg2Rad * (i + rot_euler_z));
            Vector3 point = new Vector3(pos_x, pos_y, 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformPoint(point) - transform.position, distance, layer_mask);
            Vector3 vertice;
            if (hit.collider != null)
            {
                vertice = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                vertice = point;
            }
            vertex[j] = vertice;
            j++;
        }
        CreateMesh(vertex, j);
    }

    private void CreateMesh(Vector3[] vertex, int count)
    {
        mesh.vertices = vertex;
        int[] triangles = new int[count * 3];
        int j = 0;
        for (int i = 0; i < count; i++)
        {
            triangles[j] = i;
            triangles[j + 1] = i + 1;
            triangles[j + 2] = 0;
            j += 3;
        }
        mesh.triangles = triangles;
    }
}
