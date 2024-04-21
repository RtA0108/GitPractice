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


    public VertexClickTest vertexClickTest;
    
    void Update()
    {
        int index = System.Array.IndexOf(vertexClickTest.clickedObjects, null);
        if(index > 5){
            // Example usage with 6 point correspondences
            double[] worldPoints = vertexClickTest.worldPoints;
            double[] imagePoints = vertexClickTest.imagePoints;

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
    }
}