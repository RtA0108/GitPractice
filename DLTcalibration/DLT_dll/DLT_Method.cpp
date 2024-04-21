#include "pch.h"
#include "DLT_Method.h"


using namespace cv;

extern "C"
{
    __declspec(dllexport) void ComputeDLTParameters(double* points3D, double* points2D, double* projectionMatrix, int numPoints)
    {
        // Check if there are at least 6 points
        if (numPoints < 6) {
            std::cerr << "Error: Insufficient points for DLT computation." << std::endl;
            return;
        }

        // Create vectors to store 3D and 2D points
        std::vector<Point3d> objectPoints;
        std::vector<Point2d> imagePoints;

        // Populate vectors with the provided points
        for (int i = 0; i < numPoints; ++i) {
            objectPoints.push_back(Point3d(points3D[3 * i], points3D[3 * i + 1], points3D[3 * i + 2]));
            imagePoints.push_back(Point2d(points2D[2 * i], points2D[2 * i + 1]));
        }

        // Compute DLT using solvePnP function
        Mat rvec, tvec;
        solvePnP(objectPoints, imagePoints, Mat(), Mat(), rvec, tvec);

        // Compose projection matrix from rotation and translation vectors
        Mat R;
        Rodrigues(rvec, R);

        projectionMatrix[0] = R.at<double>(0, 0);
        projectionMatrix[1] = R.at<double>(0, 1);
        projectionMatrix[2] = R.at<double>(0, 2);
        projectionMatrix[3] = tvec.at<double>(0);

        projectionMatrix[4] = R.at<double>(1, 0);
        projectionMatrix[5] = R.at<double>(1, 1);
        projectionMatrix[6] = R.at<double>(1, 2);
        projectionMatrix[7] = tvec.at<double>(1);

        projectionMatrix[8] = R.at<double>(2, 0);
        projectionMatrix[9] = R.at<double>(2, 1);
        projectionMatrix[10] = R.at<double>(2, 2);
        projectionMatrix[11] = tvec.at<double>(2);
    }
}
//extern "C" {
//	__declspec(dllexport) void DLT(const Mat& worldPointsMat, const Mat& imagePointsMat, Mat& projectionMatrix) {
//		// Check if the number of points is valid for DLT (at least 6 points required)
//		if (worldPointsMat.rows < 6 || worldPointsMat.rows != imagePointsMat.rows) {
//			std::cout << "Invalid number of points provided for DLT." << std::endl;
//			return;
//		}
//
//		int numPoints = worldPointsMat.rows;
//
//		// Construct matrix A for DLT
//		Mat A(2 * numPoints, 12, CV_64F); //CV_64F: 64bit floating-point number:double
//		for (int i = 0; i < numPoints; ++i) {
//			// Extract coordinates of world and image points
//			const double* worldPoint = worldPointsMat.ptr<double>(i); // ptr<type>(i) = i 행의 1번째 원소 주소를 반환. (type으로 지정한 자료형의 포인터를 반환)
//			const double* imagePoint = imagePointsMat.ptr<double>(i);
//
//			double X = worldPoint[0];
//			double Y = worldPoint[1];
//			double Z = worldPoint[2];
//			double u = imagePoint[0];
//			double v = imagePoint[1];
//
//			// Populate matrix A with values based on point correspondences
//			// (Following the Direct Linear Transform (DLT) equations)
//			// at (행, 열)
//			// DLT equations 에 따라 첫 행(x)은 4,5,6,7이 0의 요소를 가지고 두번째 행(y)은 0,1,2,3이 0의 요소를 가짐. 포인트 개수만큼 반복(2I * 12 행렬)
//			A.at<double>(2 * i, 0) = X;
//			A.at<double>(2 * i, 1) = Y;
//			A.at<double>(2 * i, 2) = Z;
//			A.at<double>(2 * i, 3) = 1.0;
//			A.at<double>(2 * i, 8) = -u * X;
//			A.at<double>(2 * i, 9) = -u * Y;
//			A.at<double>(2 * i, 10) = -u * Z;
//			A.at<double>(2 * i, 11) = -u;
//
//			A.at<double>(2 * i + 1, 4) = X;
//			A.at<double>(2 * i + 1, 5) = Y;
//			A.at<double>(2 * i + 1, 6) = Z;
//			A.at<double>(2 * i + 1, 7) = 1.0;
//			A.at<double>(2 * i + 1, 8) = -v * X;
//			A.at<double>(2 * i + 1, 9) = -v * Y;
//			A.at<double>(2 * i + 1, 10) = -v * Z;
//			A.at<double>(2 * i + 1, 11) = -v;
//		}
//
//		// Perform Singular Value Decomposition (SVD) to solve for the projection matrix
//		
//		Mat u, w, vt;
//		SVDecomp(A, w, u, vt);
//		/*
//		Mat projectionMatrixVec;
//		solve(A, projectionMatrixVec, DECOMP_SVD);*/
//		
//		// Extract the solution (last column of Vt) which represents the projection matrix
//		Mat solution = vt.row(vt.rows - 1);
//
//		// Reshape the solution vector into a 3x4 projection matrix
//		// SVD를 통하여 구한 벡터를 3x4 행렬로 변환. 이후 Decomposition을 구해야 하는건가? 안구하고 Projection 행렬만 사용하고 가능한건가?
//		projectionMatrix = Mat(3, 4, CV_64F);
//		for (int i = 0; i < 12; ++i) {
//			projectionMatrix.at<double>(i / 4, i % 4) = solution.at<double>(0, i);
//		}
//		
//
//	}
//
//	__declspec(dllexport) Mat projectPoints(const Mat& worldPoints, const Mat& projectionMatrix) {
//		int numPoints = worldPoints.rows;
//
//		// Create a matrix to store the projected points
//		Mat projectedPoints(numPoints, 2, CV_64F);
//
//		for (int i = 0; i < numPoints; ++i) {
//			Mat worldPoint = worldPoints.row(i).t();
//			Mat homogeneousWorldPoint;
//			vconcat(worldPoint, Mat::ones(1, 1, CV_64F), homogeneousWorldPoint);
//
//			// Projecting the 3D world point onto the 2D image plane
//			Mat projectedPoint = projectionMatrix * homogeneousWorldPoint;
//			projectedPoint /= projectedPoint.at<double>(2); // Normalize by dividing by the third component (homogeneous coordinate)
//
//			// Extracting 2D coordinates from the projected homogeneous coordinates
//			projectedPoints.at<double>(i, 0) = projectedPoint.at<double>(0);
//			projectedPoints.at<double>(i, 1) = projectedPoint.at<double>(1);
//		}
//
//		return projectedPoints;
//	}
//}