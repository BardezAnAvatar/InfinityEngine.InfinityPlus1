using namespace InfinityPlus1::Files::ImageFormats
{
	public struct BlackIsleAnimationFrameEntry
	{
	public:
		short frameWidth;
		short frameHeight;
		short frameCenterCoordinateX;
		short frameCenterCoordinateY;
		int frameDataOffsetAndCompressionFlag;
	}
}