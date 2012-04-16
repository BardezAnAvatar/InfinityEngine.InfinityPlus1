using namespace InfinityPlus1::Files::ImageFormats
{
	public struct BlackIsleAnimationHeader
	{
	public:
		char[4] signature;
		char[4] version;
		short frameCount;
		unsigned char cycleCount;
		unsigned char paletteTransparencyIndex;
		int frameEntriesOffset;
		int paletteOffset;
		int frameLookupTableOffset;
	}
}