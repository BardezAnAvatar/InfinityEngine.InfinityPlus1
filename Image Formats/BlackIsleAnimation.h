namespace InfinityPlus
{
	namespace Files
	{
		namespace ImageFormats
		{
			public ref struct RGBQUAD
			{
			public:
				unsigned char blue;
				unsigned char green;
				unsigned char red;
				unsigned char alpha;	//should always be zero for Infinity Engine
			};
		}
	}
}