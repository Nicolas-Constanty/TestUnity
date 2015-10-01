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
        GetComponent<MeshFilter>().mesh = mesh;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

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
        int j = 1;
        bool has_hit = false;
        for (float i = 0; i <= angle; i += inc)
        {

            float rot_euler_z = transform.rotation.eulerAngles.z;
            float pos_x = distance * Mathf.Cos(Mathf.Deg2Rad * (i + rot_euler_z));
            float pos_y = distance * Mathf.Sin(Mathf.Deg2Rad * (i + rot_euler_z));
            Vector3 point = new Vector3(pos_x, pos_y, 0);
            RaycastHit2D hit = Physics2D.Linecast(origin, point + origin);
            //Debug.DrawLine(origin, point + origin, Color.blue);
            Vector3 vertice;
            if (hit.collider != null)
            {
                vertice = hit.point - (Vector2)origin;
                //vertice.x = Mathf.Cos(Mathf.Deg2Rad * rot_euler_z) * (vertice.x - origin.x) - Mathf.Sin(Mathf.Deg2Rad * rot_euler_z) * (vertice.y - origin.y);
                //vertice.y = Mathf.Sin(Mathf.Deg2Rad * rot_euler_z) * (vertice.x - origin.x) + Mathf.Cos(Mathf.Deg2Rad * rot_euler_z) * (vertice.y - origin.y);
                //if (!has_hit && recursiv != 0)
                //{
                //    float new_angle = inc / (recursiv - 1);
                //    for (int h = 1; h < recursiv - 1; h++)
                //    {
                //        float calc_angle = i - inc + new_angle * (h + 1);
                //        pos_x = distance * Mathf.Cos(Mathf.Deg2Rad * (calc_angle + rot_euler_z));
                //        pos_y = distance * Mathf.Sin(Mathf.Deg2Rad * (calc_angle + rot_euler_z));
                //        point = new Vector3(pos_x + origin.x, pos_y + origin.y, origin.z);
                //        hit = Physics2D.Linecast(origin, point);
                //        //Debug.DrawLine(origin, point, Color.red);
                //        if (hit.collider != null)
                //        {
                //            vertex[j] = hit.point;
                //            j++;
                //            break;
                //        }
                //        else
                //        {
                //            vertex[j] = point;
                //        }
                //        j++;
                //    }
                //}
                has_hit = true;
            }
            else
            {
                vertice = point;
                //Calcule another ray for more accurency
                if (has_hit && recursiv != 0)
                {
                    float new_angle = inc / (recursiv - 1);
                    for (int h = 0; h < recursiv - 1; h++)
                    {
                        float calc_angle = i -inc + new_angle * (h + 1);
                        pos_x = distance * Mathf.Cos(Mathf.Deg2Rad * (calc_angle + rot_euler_z));
                        pos_y = distance * Mathf.Sin(Mathf.Deg2Rad * (calc_angle + rot_euler_z));
                        point = new Vector3(pos_x + origin.x, pos_y + origin.y, origin.z);
                        hit = Physics2D.Linecast(origin, point);
                        Debug.DrawLine(origin, point, Color.green);
                        if (hit.collider != null)
                        {
                            vertex[j] = hit.point;
                        }
                        else
                        {
                            vertex[j] = point;
                            j++;
                            break;
                        }
                        j++;
                    }
                }
                has_hit = false;
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
