

#ifndef Bardez_Projects_DirectX_Direct2D_Matrix3x2F
#define Bardez_Projects_DirectX_Direct2D_Matrix3x2F

#include <D2d1.h>
#include <D2D1Helper.h>
#include "Point2dF.h"
#include "SizeF.h"

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a managed equivalent for the Direct2D D2D1_MATRIX_3X2_F structure</summary>
				/// <remarks>
				///		See http://msdn.microsoft.com/en-us/library/windows/desktop/dd372289%28v=VS.85%29.aspx for struct details.
				///	</remarks>
				public ref class Matrix3x2F
				{
				#pragma region Fields
				protected:
					/// <summary>Represents the matrix, conceptually in order of Array[Row][Column]</summary>
					array<System::Single, 2>^ matrix;
				#pragma endregion


				#pragma region Properties
				public:
					/// <summary>Represents the matrix, conceptually in order of Array[Row][Column]</summary>
					property array<System::Single, 2>^ Matrix
					{
						array<System::Single, 2>^ get();
						void set(array<System::Single, 2>^ value);
					}

					/// <summary>Exposes the value in the first row and first column of the matrix</summary>
					property System::Single Row1_Col1
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Exposes the value in the first row and second column of the matrix</summary>
					property System::Single Row1_Col2
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Exposes the value in the second row and first column of the matrix</summary>
					property System::Single Row2_Col1
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Exposes the value in the second row and second column of the matrix</summary>
					property System::Single Row2_Col2
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Exposes the value in the third row and first column of the matrix</summary>
					property System::Single Row3_Col1
					{
						System::Single get();
						void set(System::Single value);
					}

					/// <summary>Exposes the value in the third row and second column of the matrix</summary>
					property System::Single Row3_Col2
					{
						System::Single get();
						void set(System::Single value);
					}
				#pragma endregion


				#pragma region Construction
				public:
					/// <summary>Default constructor</summary>
					Matrix3x2F();

					/// <summary>Data copy constructor</summary>
					Matrix3x2F(array<System::Single, 2>^ srcMatrix);

					/// <summary>Definition constructor</summary>
					/// <param name="Row1_Col1">The value in the first row and first column of the matrix</param>
					/// <param name="Row1_Col2">The value in the first row and second column of the matrix</param>
					/// <param name="Row2_Col1">The value in the second row and first column of the matrix</param>
					/// <param name="Row2_Col2">The value in the second row and second column of the matrix</param>
					/// <param name="Row3_Col1">The value in the third row and first column of the matrix</param>
					/// <param name="Row3_Col2">The value in the third row and second column of the matrix</param>
					Matrix3x2F(System::Single Row1_Col1, System::Single Row1_Col2, System::Single Row2_Col1, System::Single Row2_Col2, System::Single Row3_Col1, System::Single Row3_Col2);

				internal:
					/// <summary>Unmanaged constructor</summary>
					/// <param name="srcMatrix">D2D1_MATRIX_3X2_F source structure</param>
					Matrix3x2F(D2D1_MATRIX_3X2_F srcMatrix);

					/// <summary>Unmanaged constructor</summary>
					/// <param name="srcMatrix">D2D1::Matrix3x2F source class pointer</param>
					Matrix3x2F(D2D1::Matrix3x2F* srcMatrix);
				#pragma endregion

				#pragma region Methods
				internal:
					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					D2D1_MATRIX_3X2_F ToUnmanaged();

					/// <summary>Converts the managed class reference to an unmanaged equivalent class</summary>
					/// <returns>An unmanaged Direct2D class equivalent</returns>
					D2D1::Matrix3x2F* ToUnmanagedClass();

					/// <summary>Converts the managed class reference to an unmanaged equivalent structure</summary>
					/// <returns>An unmanaged Direct2D struct equivalent</returns>
					void FromUnmanagedClass(D2D1::Matrix3x2F* matrix);

				public:
					/// <summary>Populates the matrix with the specified values</summary>
					/// <param name="Row1_Col1">The value in the first row and first column of the matrix</param>
					/// <param name="Row1_Col2">The value in the first row and second column of the matrix</param>
					/// <param name="Row2_Col1">The value in the second row and first column of the matrix</param>
					/// <param name="Row2_Col2">The value in the second row and second column of the matrix</param>
					/// <param name="Row3_Col1">The value in the third row and first column of the matrix</param>
					/// <param name="Row3_Col2">The value in the third row and second column of the matrix</param>
					void Populate(System::Single Row1_Col1, System::Single Row1_Col2, System::Single Row2_Col1, System::Single Row2_Col2, System::Single Row3_Col1, System::Single Row3_Col2);

					/// <summary>Detects if both objects are equal Matrix3x2F instances</summary>
					/// <returns>true if both are Matrix3x2F and all matrix elements are even, false otherwise.</returns>
					virtual System::Boolean Equals(System::Object^ instance) override;
										
					/// <summary>Multiplies this matrix with the specified matrix and returns the result.</summary>
					/// <param name="left">The matrix to multiply against the right.</param>
					/// <param name="right">The matrix to multiply against the left.</param>
					/// <returns>The result of multiplying this matrix and the specified matrix.</returns>
					static Matrix3x2F^ operator*(Matrix3x2F^ left, Matrix3x2F^ right);

				#pragma region Direct2D Matrix3x2F methods
					/// <summary>Calculates the determinant of the matrix.</summary>
					/// <returns>The determinant of this matrix.</returns>
					System::Single Determinant();

					/// <summary>Creates an identity matrix.</summary>
					/// <returns>An identity matrix.</returns>
					static Matrix3x2F^ Identity();

					/// <summary>Inverts the matrix, if it is invertible.</summary>
					/// <returns>true if the matrix was inverted; otherwise, false.</returns>
					System::Boolean Invert();

					/// <summary>Indicates whether this matrix is the identity matrix.</summary>
					/// <returns>true if the matrix is an identity matrix; otherwise, false.</returns>
					System::Boolean IsIdentity();

					/// <summary>Indicates whether the matrix is invertible.</summary>
					/// <returns>true if the matrix is invertible; otherwise, false.</returns>
					System::Boolean IsInvertible();

					/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
					/// <param name="angle">The rotation angle in degrees. A positive angle creates a clockwise rotation, and a negative angle creates a counterclockwise rotation.</param>
					/// <param name="center">The point about which the rotation is performed.</param>
					/// <returns>A rotated 3x2 Matrix.</returns>
					static Matrix3x2F^ Rotation(System::Single angle, Point2dF^ center);

					/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
					/// <param name="angle">The rotation angle in degrees. A positive angle creates a clockwise rotation, and a negative angle creates a counterclockwise rotation.</param>
					/// <returns>A rotated 3x2 Matrix.</returns>
					static Matrix3x2F^ Rotation(System::Single angle);

					/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
					/// <param name="size">The x-axis and y-axis scale factors of the scale transformation.</param>
					/// <param name="center">The point about which the scale is performed.</param>
					/// <returns>A rotated 3x2 Matrix.</returns>
					static Matrix3x2F^ Scale(SizeF^ size, Point2dF^ center);

					/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
					/// <param name="x">The x-axis scale factor of the scale transformation.</param>
					/// <param name="y">The y-axis scale factor of the scale transformation.</param>
					/// <param name="center">The point about which the scale is performed.</param>
					/// <returns>A rotated 3x2 Matrix.</returns>
					static Matrix3x2F^ Scale(System::Single x, System::Single y, Point2dF^ center);
					
					/// <summary>Creates a rotation transformation that has the specified angle and center point.</summary>
					/// <param name="a">The first matrix to multiply.</param>
					/// <param name="b">The second matrix to multiply.</param>
					void SetProduct(Matrix3x2F^ a, Matrix3x2F^ b);

					/// <summary>Creates a skew transformation that has the specified x-axis and y-axis values and center point.</summary>
					/// <param name="angleX">The x-axis skew angle, which is measured in degrees counterclockwise from the y-axis.</param>
					/// <param name="angleY">The y-axis skew angle, which is measured in degrees clockwise from the x-axis.</param>
					/// <param name="center">The point about which the skew is performed.</param>
					/// <returns>The new skew transformation.</returns>
					static Matrix3x2F^ Skew(System::Single angleX, System::Single angleY, Point2dF^ center);

					/// <summary>Creates a skew transformation that has the specified x-axis and y-axis values and center point.</summary>
					/// <param name="angleX">The x-axis skew angle, which is measured in degrees counterclockwise from the y-axis.</param>
					/// <param name="angleY">The y-axis skew angle, which is measured in degrees clockwise from the x-axis.</param>
					/// <returns>The new skew transformation.</returns>
					static Matrix3x2F^ Skew(System::Single angleX, System::Single angleY);

					/// <summary>Uses this matrix to transform the specified point and returns the result.</summary>
					/// <param name="point">The point to transform.</param>
					/// <returns>The transformed point.</returns>
					Point2dF^ TransformPoint(Point2dF^ point);

					/// <summary>Creates a translation transformation that has the specified x and y displacements.</summary>
					/// <param name="size">The distance to translate along the x-axis and the y-axis.</param>
					/// <returns>A transformation matrix that translates an object the specified horizontal and vertical distance.</returns>
					static Matrix3x2F^ Translation(SizeF^ size);

					/// <summary>Creates a translation transformation that has the specified x and y displacements.</summary>
					/// <param name="x">The distance to translate along the x-axis.</param>
					/// <param name="y">The distance to translate along the y-axis.</param>
					/// <returns>A transformation matrix that translates an object the specified horizontal and vertical distance.</returns>
					static Matrix3x2F^ Translation(System::Single x, System::Single y);
				#pragma endregion
				#pragma endregion
				};
			}
		}
	}
}


#endif
