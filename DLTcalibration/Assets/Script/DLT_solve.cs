using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class DLT_solve : MonoBehaviour
{
    [DllImport("DLT_Rezero.dll", EntryPoint = "DLT")]
    // Start is called before the first frame update
    private static extern void DLT(double[] worldPoints, double[] imagePoints, int numPoints, double[] projectionMatrix, double[] rtMatrix);
    [DllImport("DLT_Rezero.dll", EntryPoint = "projectPoints")]
    private static extern void projectPoints(double[] worldPoints, double[] projectionMatrix, double[] rtMatrix, double[] resultPoints);

    private GameObject LVManger;

    public VertexClickTest vertexClickTest;
    public CreateSphereAtVertex createSphereAtVertex;
    private int vertexCountDLT;
    private GameObject createdBox;
    public GameObject somethingMesh;
    private Camera projCam;
    private bool flag = false;

    void Awake()
    {

        vertexCountDLT = createSphereAtVertex.vertexCount;
        Debug.Log(vertexCountDLT);
        LVManger = GameObject.Find("LevelManager");
        projCam = GameObject.FindGameObjectWithTag("Project Camera").gameObject.GetComponent<Camera>();
    }


    void Update()
    {
        if (!flag)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {

                int index = System.Array.IndexOf(vertexClickTest.clickedObjects, null);
                if (index > 5)
                {
                    flag = true;
                    Debug.Log(index);
                    // Example usage with 6 point correspondences
                    double[] worldPoints = new double[18];
                    for (int i = 0; i < index; i++)
                    {
                        worldPoints[i * 3] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.x;
                        worldPoints[i * 3 + 1] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.y;
                        worldPoints[i * 3 + 2] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.z;
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
                    double[] rtMatrix = new double[12];
                    // Call the DLT function from the DLL
                    DLT(worldPoints, imagePoints, numPoints, projectionMatrix, rtMatrix);
                    projectPoints(worldPoints, projectionMatrix, rtMatrix, projectedPoints);
                    // Print the projection matrix
                    Debug.Log("Projection Matrix: " + string.Join(", ", projectionMatrix));
                    Debug.Log("Projection Point Matrix: " + string.Join(", ", projectedPoints));

                    //RT Matrix
                    // Print the RT matrix
                    Debug.Log("RT Matrix: " + string.Join(", ", rtMatrix));

                    // Extract translation vector from RT matrix
                    //Vector3 translation = new Vector3((float)rtMatrix[3], (float)rtMatrix[7], (float)rtMatrix[11]+1000);

                    Matrix4x4 camRotationMatrix = Matrix4x4.Rotate(projCam.transform.rotation);
                    Matrix4x4 rtMatrixMat = new Matrix4x4();
                    rtMatrixMat.SetColumn(0, new Vector4((float)rtMatrix[0], (float)rtMatrix[1], (float)rtMatrix[2], 0));
                    rtMatrixMat.SetColumn(1, new Vector4((float)rtMatrix[4], (float)rtMatrix[5], (float)rtMatrix[6], 0));
                    rtMatrixMat.SetColumn(2, new Vector4((float)rtMatrix[8], (float)rtMatrix[9], (float)rtMatrix[10], 0));
                    rtMatrixMat.SetColumn(3, new Vector4(0, 0, 0, 1));
                    //create new box with RT Matrix
                    Matrix4x4 combinedRotationMatrix = rtMatrixMat * camRotationMatrix;
                    Quaternion rotation = rtMatrixMat.rotation * Quaternion.Euler(0, -180, 0);
                    Quaternion rotationResult = rotation * projCam.transform.rotation;

                    Quaternion combinedRotation = combinedRotationMatrix.rotation;
                    Vector3 DLTTransformPosition = new Vector3((float)rtMatrix[3], (float)rtMatrix[7], 1000.0f + (float)rtMatrix[11]);

                    createdBox = Instantiate(somethingMesh, DLTTransformPosition, combinedRotation);
                    createdBox.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                }
                else
                {

                    Debug.Log(index);
                }
            }
        }
        else
        {
            int index = System.Array.IndexOf(vertexClickTest.clickedObjects, null);
            double[] worldPoints = new double[18];
            for (int i = 0; i < index; i++)
            {
                worldPoints[i * 3] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.x;
                worldPoints[i * 3 + 1] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.y;
                worldPoints[i * 3 + 2] = (double)LVManger.GetComponent<VertexClickTest>().verticesStruct[i].worldCoordinate.z;
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
            double[] rtMatrix = new double[12];
            // Call the DLT function from the DLL
            DLT(worldPoints, imagePoints, numPoints, projectionMatrix, rtMatrix);
            projectPoints(worldPoints, projectionMatrix, rtMatrix, projectedPoints);
            Matrix4x4 camRotationMatrix = Matrix4x4.Rotate(projCam.transform.rotation);
            Matrix4x4 rtMatrixMat = new Matrix4x4();
            rtMatrixMat.SetColumn(0, new Vector4((float)rtMatrix[0], (float)rtMatrix[1], (float)rtMatrix[2], 0));
            rtMatrixMat.SetColumn(1, new Vector4((float)rtMatrix[4], (float)rtMatrix[5], (float)rtMatrix[6], 0));
            rtMatrixMat.SetColumn(2, new Vector4((float)rtMatrix[8], (float)rtMatrix[9], (float)rtMatrix[10], 0));
            rtMatrixMat.SetColumn(3, new Vector4(0, 0, 0, 1));
            //create new box with RT Matrix
            Matrix4x4 combinedRotationMatrix = rtMatrixMat * camRotationMatrix;
            Quaternion rotation = rtMatrixMat.rotation * Quaternion.Euler(0, 180, 0);
            Quaternion rotationResult = rotation * projCam.transform.rotation;

            Quaternion combinedRotation = combinedRotationMatrix.rotation * Quaternion.Euler(0, 180, 0) * somethingMesh.transform.rotation;
            Vector3 DLTTransformPosition = new Vector3((float)rtMatrix[3], (float)rtMatrix[7], 1000.0f + (float)rtMatrix[11]);
            createdBox.transform.rotation = combinedRotation;
            createdBox.transform.position = DLTTransformPosition;
        }
    }

}