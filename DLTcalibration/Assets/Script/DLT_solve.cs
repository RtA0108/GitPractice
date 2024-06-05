using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DLT_solve : MonoBehaviour
{
    [DllImport("DLT_Rezero.dll", EntryPoint = "DLT")]
    // Start is called before the first frame update
    private static extern void DLT(double[] worldPoints, double[] imagePoints, int numPoints, double[] projectionMatrix);
    [DllImport("DLT_Rezero.dll", EntryPoint = "projectPoints")]
    private static extern void projectPoints(double[] worldPoints, double[] projectionMatrix, double[] resultPoints);

    private GameObject LVManger;

    public VertexClickTest vertexClickTest;
    public CreateSphereAtVertex createSphereAtVertex;
    private int vertexCountDLT;
    void Awake()
    {

        vertexCountDLT = createSphereAtVertex.vertexCount;
        Debug.Log(vertexCountDLT);
        LVManger = GameObject.Find("LevelManager");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            int index = System.Array.IndexOf(vertexClickTest.clickedObjects, null);
            if (index > 5)
            {
                Debug.Log(index);
                // Example usage with 6 point correspondences
                double[] worldPoints = new double[18];
                for (int i = 0; i<index; i++) {
                    worldPoints[i*3] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.x;
                    worldPoints[i*3+1] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.y;
                    worldPoints[i*3+2] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.z;
                }
                Debug.Log("3D Matrix: " + string.Join(", ", worldPoints));

                double[] imagePoints = new double[12];
                for (int i = 0; i < index; i++)
                {
                    imagePoints[i * 2] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].screenCoordinate.x;
                    imagePoints[i * 2 + 1] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].screenCoordinate.y;
                }

                Debug.Log("2D Matrix: " + string.Join(", ", imagePoints));
                //double[] imagePoints = vertexClickTest.imagePoints;
                int numPoints = index;
                //worldPoints.Length / 3; // Assuming each 3D point has X, Y, Z coordinates

                // Allocate memory for the projection matrix
                double[] projectionMatrix = new double[12];
                double[] projectedPoints = new double[12];
                // Call the DLT function from the DLL
                DLT(worldPoints, imagePoints, numPoints, projectionMatrix);
                projectPoints(worldPoints, projectionMatrix, projectedPoints);
                // Print the projection matrix
                Debug.Log("Projection Matrix: " + string.Join(", ", projectionMatrix));
                Debug.Log("Projection Point Matrix: " + string.Join(", ", projectedPoints));
            }
            else
            {

                Debug.Log(index);
            }
        }
    }
}