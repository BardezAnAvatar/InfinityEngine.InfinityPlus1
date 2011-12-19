
#include "Matrix3x2F.h"

using namespace Bardez::Projects::DirectX::Direct2D;

#pragma region Properties
/// <summary>Represents the matrix, conceptually in order of Array[Row][Column]</summary>
array<System::Single, 2>^ Matrix3x2F::Matrix::get()
{
	return this->matrix;
}

/// <summary>Represents the matrix, conceptually in order of Array[Row][Column]</summary>
void Matrix3x2F::Matrix::set(array<System::Single, 2>^ value)
{
	this->matrix = value;
}

/// <summary>Exposes the value in the first row and first column of the matrix</summary>
System::Single Matrix3x2F::Row1_Col1::get()
{
	return this->matrix[0, 0];
}

/// <summary>Exposes the value in the first row and first column of the matrix</summary>
void Matrix3x2F::Row1_Col1::set(System::Single value)
{
	this->matrix[0, 0] = value;
}

/// <summary>Exposes the value in the first row and second column of the matrix</summary>
System::Single Matrix3x2F::Row1_Col2::get()
{
	return this->matrix[0, 1];
}

/// <summary>Exposes the value in the first row and second column of the matrix</summary>
void Matrix3x2F::Row1_Col2::set(System::Single value)
{
	this->matrix[0, 1] = value;
}

/// <summary>Exposes the value in the second row and first column of the matrix</summary>
System::Single Matrix3x2F::Row2_Col1::get()
{
	return this->matrix[1, 0];
}

/// <summary>Exposes the value in the second row and first column of the matrix</summary>
void Matrix3x2F::Row2_Col1::set(System::Single value)
{
	this->matrix[1, 0] = value;
}

/// <summary>Exposes the value in the second row and second column of the matrix</summary>
System::Single Matrix3x2F::Row2_Col2::get()
{
	return this->matrix[1, 1];
}

/// <summary>Exposes the value in the second row and second column of the matrix</summary>
void Matrix3x2F::Row2_Col2::set(System::Single value)
{
	this->matrix[1, 1] = value;
}

/// <summary>Exposes the value in the third row and first column of the matrix</summary>
System::Single Matrix3x2F::Row3_Col1::get()
{
	return this->matrix[2, 0];
}

/// <summary>Exposes the value in the third row and first column of the matrix</summary>
void Matrix3x2F::Row3_Col1::set(System::Single value)
{
	this->matrix[2, 0] = value;
}

/// <summary>Exposes the value in the third row and second column of the matrix</summary>
System::Single Matrix3x2F::Row3_Col2::get()
{
	return this->matrix[2, 1];
}

/// <summary>Exposes the value in the third row and second column of the matrix</summary>
void Matrix3x2F::Row3_Col2::set(System::Single value)
{
	this->matrix[2, 1] = value;
}
#pragma endregion


#pragma region Construction
/// <summary>Default constructor</summary>
Matrix3x2F::Matrix3x2F()
{
	this->Populate(0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
}

/// <summary>Data copy constructor</summary>
Matrix3x2F::Matrix3x2F(array<System::Single, 2>^ srcMatrix)
{
	this->Populate(srcMatrix[0, 0], srcMatrix[0, 1], srcMatrix[1, 0], srcMatrix[1, 1],  srcMatrix[2, 0], srcMatrix[2, 1]);
}

/// <summary>Definition constructor</summary>
/// <param name="Row1_Col1">The value in the first row and first column of the matrix</param>
/// <param name="Row1_Col2">The value in the first row and second column of the matrix</param>
/// <param name="Row2_Col1">The value in the second row and first column of the matrix</param>
/// <param name="Row2_Col2">The value in the second row and second column of the matrix</param>
/// <param name="Row3_Col1">The value in the third row and first column of the matrix</param>
/// <param name="Row3_Col2">The value in the third row and second column of the matrix</param>
Matrix3x2F::Matrix3x2F(System::Single Row1_Col1, System::Single Row1_Col2, System::Single Row2_Col1, System::Single Row2_Col2, System::Single Row3_Col1, System::Single Row3_Col2)
{
	this->Populate(Row1_Col1, Row1_Col2, Row2_Col1, Row2_Col2, Row3_Col1, Row3_Col2);
}

/// <summary>Unmanaged constructor</summary>
/// <param name="srcMatrix">D2D1_COLOR_F source structure</param>
Matrix3x2F::Matrix3x2F(D2D1_MATRIX_3X2_F srcMatrix)
{
	this->Populate(srcMatrix._11, srcMatrix._12, srcMatrix._21, srcMatrix._22, srcMatrix._31, srcMatrix._32);
}

/// <summary>Unmanaged constructor</summary>
/// <param name="srcMatrix">D2D1::Matrix3x2F source class pointer</param>
Matrix3x2F::Matrix3x2F(D2D1::Matrix3x2F* srcMatrix)
{
	this->Populate(srcMatrix->_11, srcMatrix->_12, srcMatrix->_21, srcMatrix->_22, srcMatrix->_31, srcMatrix->_32);
}
#pragma endregion


#pragma region Methods
/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
D2D1_MATRIX_3X2_F Matrix3x2F::ToUnmanaged()
{
	D2D1_MATRIX_3X2_F newMatrix;

	newMatrix._11 = this->matrix[0, 0];
	newMatrix._12 = this->matrix[0, 1];
	newMatrix._21 = this->matrix[1, 0];
	newMatrix._22 = this->matrix[1, 1];
	newMatrix._31 = this->matrix[2, 0];
	newMatrix._32 = this->matrix[2, 1];

	return newMatrix;
}

/// <summary>Converts the managed class reference to an unmanaged equivalent class</summary>
/// <returns>An unmanaged Direct2D class equivalent</returns>
D2D1::Matrix3x2F* Matrix3x2F::ToUnmanagedClass()
{
	D2D1::Matrix3x2F* matrix = new D2D1::Matrix3x2F(this->matrix[0, 0], this->matrix[0, 1], this->matrix[1, 0], this->matrix[1, 1], this->matrix[2, 0], this->matrix[2, 1]);
	return matrix;
}

/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
/// <returns>An unmanaged Direct2D struct equivalent</returns>
void Matrix3x2F::FromUnmanagedClass(D2D1::Matrix3x2F* matrix)
{
	this->Populate(matrix->_11, matrix->_12, matrix->_21, matrix->_22, matrix->_31, matrix->_32);
}

/// <summary>Populates the matrix with the specified values</summary>
/// <param name="Row1_Col1">The value in the first row and first column of the matrix</param>
/// <param name="Row1_Col2">The value in the first row and second column of the matrix</param>
/// <param name="Row2_Col1">The value in the second row and first column of the matrix</param>
/// <param name="Row2_Col2">The value in the second row and second column of the matrix</param>
/// <param name="Row3_Col1">The value in the third row and first column of the matrix</param>
/// <param name="Row3_Col2">The value in the third row and second column of the matrix</param>
void Matrix3x2F::Populate(System::Single Row1_Col1, System::Single Row1_Col2, System::Single Row2_Col1, System::Single Row2_Col2, System::Single Row3_Col1, System::Single Row3_Col2)
{
	this->matrix = gcnew array<System::Single, 2>(3, 2) { { Row1_Col1, Row1_Col2 }, { Row2_Col1, Row2_Col2 }, { Row3_Col1, Row3_Col2 } };
}

/// <summary>Detects if both objects are equal Matrix3x2F instances</summary>
/// <returns>true if both are Matrix3x2F and all matrix elements are even, false otherwise.</returns>
System::Boolean Matrix3x2F::Equals(System::Object^ instance)
{
	System::Boolean equal = false;
	if (instance != nullptr)
	{
		Matrix3x2F^ castInstance = dynamic_cast<Matrix3x2F^>(instance);	//does an "as" cast, which is null on failure
		if (castInstance != nullptr)
		{
			//enumerate the checks
			equal = 
				this->Row1_Col1 == castInstance->Row1_Col1
				&& this->Row1_Col2 == castInstance->Row1_Col2
				&& this->Row2_Col1 == castInstance->Row2_Col1
				&& this->Row2_Col2 == castInstance->Row2_Col2
				&& this->Row3_Col1 == castInstance->Row3_Col1
				&& this->Row3_Col2 == castInstance->Row3_Col2;
		}
	}

	return equal;
}
										
/// <summary>Multiplies this matrix with the specified matrix and returns the result.</summary>
/// <param name="left">The matrix to multiply against the right.</param>
/// <param name="right">The matrix to multiply against the left.</param>
/// <returns>The result of multiplying this matrix and the specified matrix.</returns>
Matrix3x2F^ Matrix3x2F::operator*(Matrix3x2F^ left, Matrix3x2F^ right)
{
	D2D1::Matrix3x2F* matrixA = left->ToUnmanagedClass();
	D2D1::Matrix3x2F* matrixB = right->ToUnmanagedClass();
	
	D2D1::Matrix3x2F matrix = ((*matrixA)*(*matrixB));

	delete matrixA;	//native pointer
	delete matrixB;	//native pointer
	
	return gcnew Matrix3x2F(&matrix);
}

#pragma region Direct2D Matrix3x2F methods
/// <summary>Calculates the determinant of the matrix.</summary>
/// <returns>The determinant of this matrix.</returns>
System::Single Matrix3x2F::Determinant()
{
	//instantiate the matrix
	D2D1::Matrix3x2F* matrix = this->ToUnmanagedClass();
	System::Single determinant = matrix->Determinant();
	delete matrix;	//ah, native pointers

	return determinant;
}

/// <summary>Creates an identity matrix.</summary>
/// <returns>An identity matrix.</returns>
/// <remarks>
///		Thank you, Illinois education system. I was so smart I was able to skip matrices,
///		and now I don't know them. I had to run the native code to see what it did.
///		returns
///			[1, 0]
///			[0, 1]
///			[0, 0] impied 1 in this corner, but truncated. yay!
/// </remarks>
Matrix3x2F^ Matrix3x2F::Identity()
{
	return gcnew Matrix3x2F(1.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F);
}

/// <summary>Inverts the matrix, if it is invertible.</summary>
/// <returns>true if the matrix was inverted; otherwise, false.</returns>
System::Boolean Matrix3x2F::Invert()
{
	D2D1::Matrix3x2F* matrix = this->ToUnmanagedClass();	//instantiate the matrix
	System::Boolean couldInvert = matrix->Invert();
	this->FromUnmanagedClass(matrix);
	delete matrix;	//native pointer

	return couldInvert;
}

/// <summary>Indicates whether this matrix is the identity matrix.</summary>
/// <returns>true if the matrix is an identity matrix; otherwise, false.</returns>
System::Boolean Matrix3x2F::IsIdentity()
{
	return this->Equals(Matrix3x2F::Identity());
}

/// <summary>Indicates whether the matrix is invertible.</summary>
/// <returns>true if the matrix is invertible; otherwise, false.</returns>
System::Boolean Matrix3x2F::IsInvertible()
{
	D2D1::Matrix3x2F* matrix = this->ToUnmanagedClass();	//instantiate the matrix
	System::Boolean canInvert = matrix->IsInvertible();
	delete matrix;	//native pointer

	return canInvert;
}

/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
/// <param name="angle">The rotation angle in degrees. A positive angle creates a clockwise rotation, and a negative angle creates a counterclockwise rotation.</param>
/// <param name="center">The point about which the rotation is performed.</param>
Matrix3x2F^ Matrix3x2F::Rotation(System::Single angle, Point2dF^ center)
{
	D2D1::Matrix3x2F matrix = D2D1::Matrix3x2F::Rotation(angle, center->ToUnmanaged());
	return gcnew Matrix3x2F(&matrix);
}

/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
/// <param name="angle">The rotation angle in degrees. A positive angle creates a clockwise rotation, and a negative angle creates a counterclockwise rotation.</param>
Matrix3x2F^ Matrix3x2F::Rotation(System::Single angle)
{
	D2D1::Matrix3x2F matrix = D2D1::Matrix3x2F::Rotation(angle);
	return gcnew Matrix3x2F(&matrix);
}

/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
/// <param name="size">The x-axis and y-axis scale factors of the scale transformation.</param>
/// <param name="center">The point about which the scale is performed.</param>
/// <returns>A rotated 3x2 Matrix.</returns>
Matrix3x2F^ Matrix3x2F::Scale(SizeF^ size, Point2dF^ center)
{
	D2D1::Matrix3x2F matrix = D2D1::Matrix3x2F::Scale(size->ToUnmanaged(), center->ToUnmanaged());
	return gcnew Matrix3x2F(&matrix);
}

/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
/// <param name="x">The x-axis scale factor of the scale transformation.</param>
/// <param name="y">The y-axis scale factor of the scale transformation.</param>
/// <param name="center">The point about which the scale is performed.</param>
/// <returns>A rotated 3x2 Matrix.</returns>
Matrix3x2F^ Matrix3x2F::Scale(System::Single x, System::Single y, Point2dF^ center)
{
	D2D1::Matrix3x2F matrix = D2D1::Matrix3x2F::Scale(x, y, center->ToUnmanaged());
	return gcnew Matrix3x2F(&matrix);
}
					
/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
/// <param name="a">The first matrix to multiply.</param>
/// <param name="b">The second matrix to multiply.</param>
void Matrix3x2F::SetProduct(Matrix3x2F^ a, Matrix3x2F^ b)
{
	D2D1::Matrix3x2F* matrix = this->ToUnmanagedClass();	//instantiate the matrix
	D2D1::Matrix3x2F* matrixA = a->ToUnmanagedClass();
	D2D1::Matrix3x2F* matrixB = b->ToUnmanagedClass();

	matrix->SetProduct(*matrixA, *matrixB);

	delete matrix;	//native pointer
	delete matrixA;	//native pointer
	delete matrixB;	//native pointer
}

/// <summary>Creates a skew transformation that has the specified x-axis and y-axis values and center point.</summary>
/// <param name="angleX">The x-axis skew angle, which is measured in degrees counterclockwise from the y-axis.</param>
/// <param name="angleY">The y-axis skew angle, which is measured in degrees clockwise from the x-axis.</param>
/// <param name="center">The point about which the skew is performed.</param>
/// <returns>The new skew transformation.</returns>
Matrix3x2F^ Matrix3x2F::Skew(System::Single angleX, System::Single angleY, Point2dF^ center)
{
	D2D1::Matrix3x2F matrix = D2D1::Matrix3x2F::Skew(angleX, angleY, center->ToUnmanaged());
	return gcnew Matrix3x2F(&matrix);
}

/// <summary>Creates a skew transformation that has the specified x-axis and y-axis values and center point.</summary>
/// <param name="angleX">The x-axis skew angle, which is measured in degrees counterclockwise from the y-axis.</param>
/// <param name="angleY">The y-axis skew angle, which is measured in degrees clockwise from the x-axis.</param>
/// <returns>The new skew transformation.</returns>
Matrix3x2F^ Matrix3x2F::Skew(System::Single angleX, System::Single angleY)
{
	D2D1::Matrix3x2F matrix = D2D1::Matrix3x2F::Skew(angleX, angleY);
	return gcnew Matrix3x2F(&matrix);
}

/// <summary>Uses this matrix to transform the specified point and returns the result.</summary>
/// <param name="point">The point to transform.</param>
/// <returns>The transformed point.</returns>
Point2dF^ Matrix3x2F::TransformPoint(Point2dF^ point)
{
	D2D1::Matrix3x2F* matrix;
	matrix = this->ToUnmanagedClass();	//instantiate the matrix
	D2D1_POINT_2F pointF = matrix->TransformPoint(point->ToUnmanaged());
	delete matrix;	//native pointer

	return gcnew Point2dF(pointF);
}

/// <summary>Creates a translation transformation that has the specified x and y displacements.</summary>
/// <param name="size">The distance to translate along the x-axis and the y-axis.</param>
/// <returns>A transformation matrix that translates an object the specified horizontal and vertical distance.</returns>
Matrix3x2F^ Matrix3x2F::Translation(SizeF^ size)
{
	D2D1::Matrix3x2F matrix = D2D1::Matrix3x2F::Translation(size->ToUnmanaged());
	return gcnew Matrix3x2F(&matrix);
}

/// <summary>Creates a translation transformation that has the specified x and y displacements.</summary>
/// <param name="x">The distance to translate along the x-axis.</param>
/// <param name="y">The distance to translate along the y-axis.</param>
/// <returns>A transformation matrix that translates an object the specified horizontal and vertical distance.</returns>
Matrix3x2F^ Matrix3x2F::Translation(System::Single x, System::Single y)
{
	D2D1::Matrix3x2F matrix = D2D1::Matrix3x2F::Translation(x, y);
	return gcnew Matrix3x2F(&matrix);
}
#pragma endregion
#pragma endregion
