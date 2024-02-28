// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using OpenCvSharp;

// public class DLTscript : MonoBehaviour
// {
//     // Example 3D world points (replace with your own world coordinates)
//     private Point3f[] worldPoints = new Point3f[]
//     {
//         new Point3f(-1, -1, 0),
//         new Point3f(1, -1, 0),
//         new Point3f(1, 1, 0),
//         new Point3f(-1, 1, 0)
//     };

//     // Example 2D image points (replace with your own image coordinates)
//     private Point2f[] imagePoints = new Point2f[]
//     {
//         new Point2f(100, 100),
//         new Point2f(200, 100),
//         new Point2f(200, 200),
//         new Point2f(100, 200)
//     };

//     void Start()
//     {
//         // Perform DLT to estimate the camera projection matrix
//         Mat projectionMatrix = EstimateCameraProjectionMatrix(worldPoints, imagePoints);

//         // Use the projection matrix in Unity (e.g., set it to the camera's projection matrix)
//         Camera.main.projectionMatrix = ConvertToUnityMatrix(projectionMatrix);

//         // Optional: Log the estimated projection matrix
//         Debug.Log("Estimated Projection Matrix:\n" + projectionMatrix);
//     }

//     Mat EstimateCameraProjectionMatrix(Point3f[] worldPoints, Point2f[] imagePoints)
//     {
//         // Check if the number of points is valid for DLT
//         if (worldPoints.Length < 6 || worldPoints.Length != imagePoints.Length)
//         {
//             Debug.LogError("Invalid number of points for DLT.");
//             return new Mat();
//         }

//         // Build the linear system of equations
//         Mat A = BuildLinearSystem(worldPoints, imagePoints);

//         // Solve the linear system using least squares
//         Mat solution = A.Solve();

//         // Extract the projection matrix from the solution
//         Mat projectionMatrix = ExtractProjectionMatrix(solution);

//         return projectionMatrix;
//     }

//     Mat BuildLinearSystem(Point3f[] worldPoints, Point2f[] imagePoints)
//     {
//         int numPoints = worldPoints.Length;
//         Mat A = new Mat(numPoints * 2, 12, MatType.CV_64F);

//         for (int i = 0; i < numPoints; i++)
//         {
//             A.At<double>(i * 2, 0) = worldPoints[i].X;
//             A.At<double>(i * 2, 1) = worldPoints[i].Y;
//             A.At<double>(i * 2, 2) = worldPoints[i].Z;
//             A.At<double>(i * 2, 3) = 1;
//             A.At<double>(i * 2, 8) = -imagePoints[i].X * worldPoints[i].X;
//             A.At<double>(i * 2, 9) = -imagePoints[i].X * worldPoints[i].Y;
//             A.At<double>(i * 2, 10) = -imagePoints[i].X * worldPoints[i].Z;
//             A.At<double>(i * 2, 11) = -imagePoints[i].X;

//             A.At<double>(i * 2 + 1, 4) = worldPoints[i].X;
//             A.At<double>(i * 2 + 1, 5) = worldPoints[i].Y;
//             A.At<double>(i * 2 + 1, 6) = worldPoints[i].Z;
//             A.At<double>(i * 2 + 1, 7) = 1;
//             A.At<double>(i * 2 + 1, 8) = -imagePoints[i].Y * worldPoints[i].X;
//             A.At<double>(i * 2 + 1, 9) = -imagePoints[i].Y * worldPoints[i].Y;
//             A.At<double>(i * 2 + 1, 10) = -imagePoints[i].Y * worldPoints[i].Z;
//             A.At<double>(i * 2 + 1, 11) = -imagePoints[i].Y;
//         }

//         return A;
//     }

//     Mat ExtractProjectionMatrix(Mat solution)
//     {
//         Mat projectionMatrix = new Mat(3, 4, MatType.CV_64F);

//         for (int i = 0; i < 3; i++)
//         {
//             for (int j = 0; j < 4; j++)
//             {
//                 projectionMatrix.At<double>(i, j) = solution.At<double>(i * 4 + j, 0);
//             }
//         }

//         return projectionMatrix;
//     }

//     Matrix4x4 ConvertToUnityMatrix(Mat cvMatrix)
//     {
//         Matrix4x4 unityMatrix = new Matrix4x4();

//         for (int i = 0; i < 3; i++)
//         {
//             for (int j = 0; j < 4; j++)
//             {
//                 unityMatrix[i, j] = (float)cvMatrix.At<double>(i, j);
//             }
//         }

//         return unityMatrix;
//     }
// }