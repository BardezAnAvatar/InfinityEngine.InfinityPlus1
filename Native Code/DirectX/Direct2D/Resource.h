
#ifndef Bardez_Projects_DirectX_Direct2D_Resource
#define Bardez_Projects_DirectX_Direct2D_Resource

using namespace Bardez::Projects::Win32;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Represents a base Direct2D resource class</summary>
				public ref class Resource abstract
				{
				#pragma region Interface Methods
				public:
					/// <summary>Begins drawing on the specified surface</summary>
					virtual void GetFactory();
				#pragma endregion
				};
			}
		}
	}
}

#endif
