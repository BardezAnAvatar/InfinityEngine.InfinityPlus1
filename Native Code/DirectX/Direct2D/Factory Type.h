

#ifndef Bardez_Projects_DirectX_Direct2D_FactoryType
#define Bardez_Projects_DirectX_Direct2D_FactoryType

using namespace System::ComponentModel;

namespace Bardez
{
	namespace Projects
	{
		namespace DirectX
		{
			namespace Direct2D
			{
				/// <summary>Specifies whether Direct2D provides synchronization for an ID2D1Factory and the resources it creates, so that they may be safely accessed from multiple threads.</summary>
				/// <remarks>See http://msdn.microsoft.com/en-us/library/windows/desktop/dd368104%28v=VS.85%29.aspx for more detail</remarks>
				public enum struct FactoryType : System::UInt32
				{
					/// <summary>No synchronization is provided for accessing or writing to the factory or the objects it creates. If the factory or the objects are called from multiple threads, it is up to the application to provide access locking.</summary>
					[DescriptionAttribute("No synchronization is provided for accessing or writing to the factory or the objects it creates. If the factory or the objects are called from multiple threads, it is up to the application to provide access locking.")]
					SingleThreaded		= 0,

					/// <summary>Direct2D provides synchronization for accessing and writing to the factory and the objects it creates, enabling safe access from multiple threads.</summary>
					[DescriptionAttribute("Direct2D provides synchronization for accessing and writing to the factory and the objects it creates, enabling safe access from multiple threads.")]
					MultiThreaded		= 1,
				};
			}
		}
	}
}


#endif
