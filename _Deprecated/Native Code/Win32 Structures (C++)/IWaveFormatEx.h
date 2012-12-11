

#ifndef Bardez_Projects_Win32_Audio_IWaveFormatEx
#define Bardez_Projects_Win32_Audio_IWaveFormatEx

#include "Wave Format Ex.h"

namespace Bardez
{
	namespace Projects
	{
		namespace Win32
		{
			namespace Audio
			{
				/// <summary>Interface that declares a method to return a WaveFormatEx reference</summary>
				public interface class IWaveFormatEx
				{
					/// <summary>Returns a WaveFormatEx instance from this header data</summary>
					/// <returns>A WaveFormatEx instance to submit to API calls</returns>
					WaveFormatEx^ GetWaveFormat();
				};
			}
		}
	}
}

#endif

