using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.Control_Objects
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     Unsafe test case from C/C++ code derived from GemRB and the MVE documentation code.
    ///     Code has been copied and adjusted as little as possible to avoid logic contamination.
    /// </remarks>
    public class MveVideoCoder16SuppliedLogic
    {
        #region Fields
        protected Byte[] BackBuf;
        public Byte[] PaletteData { get; set; }
        public Int32 g_width { get; set; }
        public Int32 g_height { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveVideoCoder16SuppliedLogic(Int32 height, Int32 width)
        {
            this.BackBuf = new Byte[this.g_height * this.g_width * 4];
            this.g_height = height;
            this.g_width = width;
        }

        /// <summary>Default constructor</summary>
        public MveVideoCoder16SuppliedLogic(Int32 height, Int32 width, Byte[] backBuffer)
        {
            this.g_height = height;
            this.g_width = width;
            this.BackBuf = new Byte[this.g_height * this.g_width * 4];
            Array.Copy(backBuffer, 0, this.BackBuf, this.BackBuffer1Index, backBuffer.Length);
        }
        #endregion


        #region Properties
        protected Int32 BackBuffer1Index
        {
            get { return this.BackBuf.Length / 2; }
        }

        protected Int32 BackBuffer2Index
        {
            get { return 0; }
        }
        #endregion


        public unsafe void DecodeBlock(Byte[] output, Byte codeType, Byte[] data, ref Int32 pDataRemain, ref Int32 curXb, ref Int32 curYb)
        {
            fixed (Byte* PData = data, backBuf = BackBuf, Output = output)
            {
                Byte* g_vBackBuf1 = backBuf;
                Byte* g_vBackBuf2 = backBuf + (BackBuf.Length / 2);
                Byte* pFrame = g_vBackBuf1 + (curYb * this.g_width * 2) + (curXb * 2);
                Byte* pData = PData;
                Byte* pOutput = Output + (curYb * this.g_width * 2) + (curXb * 2);

                //Byte[] p = new Byte[4];
                //Byte[] pat = new Byte[16];
                //int i, j, k;
                //int x, y;
                
                switch(codeType)
                {
                    case 0x0:
                              copyFrame(pOutput, pFrame + (g_vBackBuf2 - g_vBackBuf1));
                              break;

                    case 0x1:
                              break;

                    case 0x2:
                              this.ipvideo_decode_0x2((UInt16*)pOutput, &pData);
                              break;

                    case 0x3:
                              this.ipvideo_decode_0x3((UInt16*)pOutput, &pData);
                              break;

                    case 0x4:
                              this.ipvideo_decode_0x4((UInt16*)pOutput, (UInt16*)(pFrame + (g_vBackBuf2 - g_vBackBuf1)), &pData);
                              break;

                    case 0x5:
                              this.ipvideo_decode_0x5((UInt16*)pOutput, (UInt16*)(pFrame + (g_vBackBuf2 - g_vBackBuf1)), &pData);
                              break;

                    case 0x6:
                              for (Int32 i=0; i<2; i++)
                              {
                                  pFrame += 16;
                                  if (++curXb == (g_width >> 3))
                                  {
                                      pOutput += 7 * g_width;
                                      curXb = 0;
                                      if (++curYb == (g_height >> 3))
                                          return;
                                  }
                              }
                              break;

                    case 0x7:
                              this.ipvideo_decode_0x7((UInt16*)pOutput, &pData);
                              break;

                    case 0x8:
                              this.ipvideo_decode_0x8((UInt16*)pOutput, &pData);
                              break;

                    case 0x9:
                              this.ipvideo_decode_0x9((UInt16*)pOutput, &pData);
                              break;

                    case 0xa:
                              this.ipvideo_decode_0xa((UInt16*)pOutput, &pData);
                              break;

                    case 0xb:
                              this.ipvideo_decode_0xb((UInt16*)pOutput, &pData);
                              break;

                    case 0xc:
                              this.ipvideo_decode_0xc((UInt16*)pOutput, &pData);
                              break;

                    case 0xd:
                              this.ipvideo_decode_0xd((UInt16*)pOutput, &pData);
                              break;

                    case 0xe:
                              this.ipvideo_decode_0xe((UInt16*)pOutput, &pData);
                              break;

                    case 0xf:
                              this.ipvideo_decode_0xf((UInt16*)pOutput, &pData);
                              break;

                    default:
                              break;
                }
            }
        }

        protected virtual void relClose(int i, out int x, out int y)
        {
            int ma, mi;

            ma = i >> 4;
            mi = i & 0xf;

            x = mi - 8;
            y = ma - 8;
        }

        protected virtual void relFar(int i, int sign, out int x, out int y)
        {
            if (i < 56)
            {
                x = sign * (8 + (i % 7));
                y = sign *      (i / 7);
            }
            else
            {
                x = sign * (-14 + (i - 56) % 29);
                y = sign *   (8 + (i - 56) / 29);
            }
        }

        protected virtual unsafe void patternRow4Pixels(Byte* pFrame, Byte pat0, Byte pat1, Byte[] p)
        {
            UInt16 mask=0x0003;
            UInt16 shift=0;
            UInt16 pattern = (UInt16)((pat1 << 8) | pat0);

            while (mask != 0)
            {
                *pFrame++ = p[(mask & pattern) >> shift];
                mask <<= 2;
                shift += 2;
            }
        }

        protected virtual unsafe void patternRow4Pixels2(Byte* pFrame, Byte pat0, Byte[] p)
        {
            Byte mask=0x03;
            Byte shift=0;
            Byte pel;
            int skip=1;

            while (mask != 0)
            {
                pel = p[(mask & pat0) >> shift];
                pFrame[0] = pel;
                pFrame[2] = pel;
                pFrame[g_width + 0] = pel;
                pFrame[g_width + 2] = pel;
                pFrame += skip;
                skip = 4 - skip;
                mask <<= 2;
                shift += 2;
            }
        }

        protected virtual unsafe void patternRow4Pixels2x1(Byte* pFrame, Byte pat, Byte[] p)
        {
            Byte mask=0x03;
            Byte shift=0;
            Byte pel;

            while (mask != 0)
            {
                pel = p[(mask & pat) >> shift];
                pFrame[0] = pel;
                pFrame[1] = pel;
                pFrame += 2;
                mask <<= 2;
                shift += 2;
            }
        }

        protected virtual unsafe void patternQuadrant4Pixels(Byte* pFrame, Byte pat0, Byte pat1, Byte pat2, Byte pat3, Byte[] p)
        {
            UInt32 mask = 0x00000003U;
            int shift=0;
            int i;
            UInt32 pat = ((UInt32)(pat3 << 24) | (UInt32)(pat2 << 16) | (UInt32)(pat1 << 8) | pat0);

            for (i=0; i<16; i++)
            {
                pFrame[i&3] = p[(pat & mask) >> shift];

                if ((i&3) == 3)
                    pFrame += g_width;

                mask <<= 2;
                shift += 2;
            }
        }

        protected virtual unsafe void patternRow2Pixels(Byte* pFrame, Byte pat, Byte[] p)
        {
            Byte mask=0x01;

            while (mask != 0)
            {
                *pFrame++ = p[((mask & pat) != 0) ? 1 : 0];
                mask <<= 1;
            }
        }

        protected virtual unsafe void patternRow2Pixels2(Byte* pFrame, Byte pat, Byte[] p)
        {
            Byte pel;
            Byte mask=0x1;
            int skip=1;

            while (mask != 0x10)
            {
                pel = p[((mask & pat) > 0)? 1 : 0];
                pFrame[0] = pel;
                pFrame[2] = pel;
                pFrame[g_width + 0] = pel;
                pFrame[g_width + 2] = pel;
                pFrame += skip;
                skip = 4 - skip;
                mask <<= 1;
            }
        }

        protected virtual unsafe void patternQuadrant2Pixels(Byte* pFrame, Byte pat0, Byte pat1, Byte[] p)
        {
            UInt16 mask = 0x0001;
            int i;
            UInt16 pat = (UInt16)((pat1 << 8) | pat0);

            for (i=0; i<16; i++)
            {
                pFrame[i&3] = p[((pat & mask) != 0) ? 1 : 0];

                if ((i&3) == 3)
                    pFrame += g_width;

                mask <<= 1;
            }
        }

        protected virtual unsafe void memset(Byte* array, Byte datum, Byte count)
        {
            for (Int32 i = 0; i < count; ++i)
                array[i] = datum;
        }

        protected virtual unsafe void memcpy(Byte* dest, Byte* src, Byte count)
        {
            for (Int32 i = 0; i < count; ++i)
                dest[i] = src[i];
        }








        #region 16 bit decoders
        protected virtual unsafe void copyFrame(Byte* pDest, Byte* pSrc)
        {
            int i;

            for (i=0; i<8; i++)
            {
                memcpy(pDest, pSrc, 16);
                pDest += (g_width * 2);
                pSrc += (g_width * 2);
            }
        }

        protected unsafe int ipvideo_decode_0x2(UInt16* frame, Byte **data)
        {
	        Byte B;
	        int x, y;
	        int offset;

	        /* copy block from 2 frames ago using a motion vector */
	        B = *(*data)++;

	        if (B < 56)
            {
		        x = 8 + (B % 7);
		        y = B / 7;
	        }
            else
            {
		        x = -14 + ((B - 56) % 29);
		        y = 8 + ((B - 56) / 29);
	        }
	        offset = y * this.g_width + x;

	        return ipvideo_copy_block (frame, frame + offset);
        }

        protected unsafe int ipvideo_decode_0x3(UInt16* frame, Byte **data)
        {
	        Byte B;
	        int x, y;
	        int offset;

	        /* copy 8x8 block from current frame from an up/left block */
	        B = *(*data)++;

	        if (B < 56) {
		        x = -(8 + (B % 7));
		        y = -(B / 7);
	        } else {
		        x = -(-14 + ((B - 56) % 29));
		        y = -(8 + ((B - 56) / 29));
	        }
	        offset = y * this.g_width + x;

	        return ipvideo_copy_block (frame, frame + offset);
        }

        protected unsafe int ipvideo_decode_0x4(UInt16* frame, UInt16* prev, Byte** data)
        {
	        int x, y;
	        Byte B;
	        int offset;


	        /* copy a block from the previous frame */
	        B = *(*data)++;
	        x = -8 + (B & 0x0F);
	        y = -8 + (B >> 4);
	        offset = y * this.g_width + x;

            return ipvideo_copy_block(frame, prev + offset);
        }

        protected unsafe int ipvideo_decode_0x5(UInt16* frame, UInt16* prev, Byte** data)
        {
	        SByte x, y;
	        int offset;

	        /* copy a block from the previous frame using an expanded range */
	        x = (SByte) (*(*data)++);
	        y = (SByte) (*(*data)++);
	        offset = y * this.g_width + x;

            return ipvideo_copy_block(frame, prev + offset);
        }

        protected unsafe int ipvideo_decode_0x7(UInt16 *frame, Byte **data)
        {
	        int x, y;
	        UInt16 P0, P1;
	        Int32 flags;
	        int bitmask;

	        /* 2-color encoding */
	        P0 = PIXEL (*data);
	        (*data) += 2;
	        P1 = PIXEL (*data);
	        (*data) += 2;

	        if ((P0 & 0x8000) == 0) {

		        /* need 8 more bytes from the stream */
		        for (y = 0; y < 8; ++y) {
			        flags = *(*data)++;
			        for (x = 0x01; x <= 0x80; x <<= 1) {
				        if ((flags & x) != 0)
					        *frame++ = P1;
				        else
					        *frame++ = P0;
			        }
			        frame += this.g_width - 8;
		        }

	        } else {
                unchecked { P0 &= (UInt16)(~0x8000); }

		        /* need 2 more bytes from the stream */

		        flags = ((*data)[1] << 8) | (*data)[0];
		        (*data) += 2;
		        bitmask = 0x0001;
		        for (y = 0; y < 8; y += 2) {
			        for (x = 0; x < 8; x += 2, bitmask <<= 1) {
				        if ((flags & bitmask) != 0)
                        {
					        *(frame + x) = P1;
					        *(frame + x + 1) = P1;
					        *(frame + this.g_width + x) = P1;
					        *(frame + this.g_width + x + 1) = P1;
				        } else {
					        *(frame + x) = P0;
					        *(frame + x + 1) = P0;
					        *(frame + this.g_width + x) = P0;
					        *(frame + this.g_width + x + 1) = P0;
				        }
			        }
			        frame += this.g_width * 2;
		        }
	        }

	        return 0;
        }

        protected unsafe int ipvideo_decode_0x8(UInt16 *frame, Byte **data)
        {
	        int x, y;
	        UInt16[] P = new UInt16[8];
	        Byte[] B = new Byte[8];
	        Int32 flags = 0;
	        Int32 bitmask = 0;
	        UInt16 P0 = 0, P1 = 0;
	        int lower_half = 0;

	        /* 2-color encoding for each 4x4 quadrant, or 2-color encoding on
	         * either top and bottom or left and right halves */

	        P[0] = PIXEL (*data);
	        (*data) += 2;
	        P[1] = PIXEL (*data);
	        (*data) += 2;
	        B[0] = *(*data)++;
	        B[1] = *(*data)++;

	        if ((P[0] & 0x8000) == 0) {

		        /* need 18 more bytes */
		        P[2] = PIXEL (*data);
		        (*data) += 2;
		        P[3] = PIXEL (*data);
		        (*data) += 2;
		        B[2] = *(*data)++;
		        B[3] = *(*data)++;
		        P[4] = PIXEL (*data);
		        (*data) += 2;
		        P[5] = PIXEL (*data);
		        (*data) += 2;
		        B[4] = *(*data)++;
		        B[5] = *(*data)++;
		        P[6] = PIXEL (*data);
		        (*data) += 2;
		        P[7] = PIXEL (*data);
		        (*data) += 2;
		        B[6] = *(*data)++;
		        B[7] = *(*data)++;

		        flags = ((B[0] & 0xF0) << 4) | ((B[4] & 0xF0) << 8) |
			        ((B[0] & 0x0F)) | ((B[4] & 0x0F) << 4) |
			        ((B[1] & 0xF0) << 20) | ((B[5] & 0xF0) << 24) |
			        ((B[1] & 0x0F) << 16) | ((B[5] & 0x0F) << 20);
		        bitmask = 0x00000001;
		        lower_half = 0;             /* still on top half */

		        for (y = 0; y < 8; ++y) {

			        /* time to reload flags? */
			        if (y == 4) {
				        flags = ((B[2] & 0xF0) << 4) | ((B[6] & 0xF0) << 8) |
					        ((B[2] & 0x0F)) | ((B[6] & 0x0F) << 4) |
					        ((B[3] & 0xF0) << 20) | ((B[7] & 0xF0) << 24) |
					        ((B[3] & 0x0F) << 16) | ((B[7] & 0x0F) << 20);
				        bitmask = 0x00000001;
				        lower_half = 2;
			        }

			        /* get the pixel values ready for this quadrant */
			        P0 = P[lower_half + 0];
			        P1 = P[lower_half + 1];

			        for (x = 0; x < 8; ++x, bitmask <<= 1) {
				        if (x == 4) {
					        P0 = P[lower_half + 4];
					        P1 = P[lower_half + 5];
				        }

				        if ((flags & bitmask) != 0)
					        *frame++ = P1;
				        else
					        *frame++ = P0;
			        }
			        frame += this.g_width - 8;
		        }

            }
            else
            {
                unchecked { P[0] &= (UInt16)(~0x8000); }

		        /* need 10 more bytes */
		        B[2] = *(*data)++;
		        B[3] = *(*data)++;
		        P[2] = PIXEL (*data);
		        (*data) += 2;
		        P[3] = PIXEL (*data);
		        (*data) += 2;
		        B[4] = *(*data)++;
		        B[5] = *(*data)++;
		        B[6] = *(*data)++;
		        B[7] = *(*data)++;

		        if ((P[2] & 0x8000) == 0)
                {
			        /* vertical split; left & right halves are 2-color encoded */

			        flags =
					        ((B[0] & 0xF0) << 4) | ((B[4] & 0xF0) << 8) |
					        ((B[0] & 0x0F)) | ((B[4] & 0x0F) << 4) |
					        ((B[1] & 0xF0) << 20) | ((B[5] & 0xF0) << 24) |
					        ((B[1] & 0x0F) << 16) | ((B[5] & 0x0F) << 20);
			        bitmask = 0x00000001;

			        for (y = 0; y < 8; ++y) {

				        /* time to reload flags? */
				        if (y == 4) {
					        flags = ((B[2] & 0xF0) << 4) | ((B[6] & 0xF0) << 8) |
						        ((B[2] & 0x0F)) | ((B[6] & 0x0F) << 4) |
						        ((B[3] & 0xF0) << 20) | ((B[7] & 0xF0) << 24) |
						        ((B[3] & 0x0F) << 16) | ((B[7] & 0x0F) << 20);
					        bitmask = 0x00000001;
				        }

				        /* get the pixel values ready for this half */
				        P0 = P[0];
				        P1 = P[1];

				        for (x = 0; x < 8; ++x, bitmask <<= 1) {
					        if (x == 4) {
						        P0 = P[2];
						        P1 = P[3];
					        }

					        if ((flags & bitmask) != 0)
						        *frame++ = P1;
					        else
						        *frame++ = P0;
				        }
				        frame += this.g_width - 8;
			        }

		        } else {
			        /* horizontal split; top & bottom halves are 2-color encoded */

			        P0 = P[0];
			        P1 = P[1];

			        for (y = 0; y < 8; ++y) {

				        flags = B[y];
				        if (y == 4) {
                            unchecked
                            {
                                P0 = (UInt16)(P[2] & (UInt16)(~0x8000));
                            }
					        P1 = P[3];
				        }

				        for (bitmask = 0x01; bitmask <= 0x80; bitmask <<= 1) {

					        if ((flags & bitmask) != 0)
						        *frame++ = P1;
					        else
						        *frame++ = P0;
				        }
				        frame += this.g_width - 8;
			        }
		        }
	        }

	        return 0;
        }

        protected unsafe int ipvideo_decode_0x9(UInt16 *frame, Byte **data)
        {
	        int x, y;
	        UInt16[] P = new UInt16[4];
	        Byte[] B = new Byte[4];
	        Int32 flags = 0;
	        int shifter = 0;
	        UInt16 pix;

	        /* 4-color encoding */
	        P[0] = PIXEL (*data);
	        (*data) += 2;
	        P[1] = PIXEL (*data);
	        (*data) += 2;
	        P[2] = PIXEL (*data);
	        (*data) += 2;
	        P[3] = PIXEL (*data);
	        (*data) += 2;

	        if ((P[0] & 0x8000) == 0 && (P[2] & 0x8000) == 0) {

		        /* 1 of 4 colors for each pixel, need 16 more bytes */
		        for (y = 0; y < 8; ++y) {
			        /* get the next set of 8 2-bit flags */
			        flags = ((*data)[1] << 8) | (*data)[0];
			        (*data) += 2;
			        for (x = 0, shifter = 0; x < 8; ++x, shifter += 2) {
				        *frame++ = P[(flags >> shifter) & 0x03];
			        }
			        frame += this.g_width - 8;
		        }

            }
            else if ((P[0] & 0x8000) == 0 && (P[2] & 0x8000) != 0)
            {
                unchecked { P[2] &= (UInt16)(~0x8000); }

		        /* 1 of 4 colors for each 2x2 block, need 4 more bytes */

		        B[0] = *(*data)++;
		        B[1] = *(*data)++;
		        B[2] = *(*data)++;
		        B[3] = *(*data)++;
		        flags = (B[3] << 24) | (B[2] << 16) | (B[1] << 8) | B[0];
		        shifter = 0;

		        for (y = 0; y < 8; y += 2) {
			        for (x = 0; x < 8; x += 2, shifter += 2) {
				        pix = P[(flags >> shifter) & 0x03];
				        *(frame + x) = pix;
				        *(frame + x + 1) = pix;
				        *(frame + this.g_width + x) = pix;
				        *(frame + this.g_width + x + 1) = pix;
			        }
			        frame += this.g_width * 2;
		        }

            }
            else if ((P[0] & 0x8000) != 0 && (P[2] & 0x8000) == 0)
            {
                unchecked { P[0] &= (UInt16)(~0x8000); }

		        /* 1 of 4 colors for each 2x1 block, need 8 more bytes */
		        for (y = 0; y < 8; ++y) {
			        /* time to reload flags? */
			        if ((y == 0) || (y == 4)) {
				        B[0] = *(*data)++;
				        B[1] = *(*data)++;
				        B[2] = *(*data)++;
				        B[3] = *(*data)++;
				        flags = (B[3] << 24) | (B[2] << 16) | (B[1] << 8) | B[0];
				        shifter = 0;
			        }
			        for (x = 0; x < 8; x += 2, shifter += 2) {
				        pix = P[(flags >> shifter) & 0x03];
				        *(frame + x) = pix;
				        *(frame + x + 1) = pix;
			        }
			        frame += this.g_width;
		        }

            }
            else
            {
                unchecked { P[0] &= (UInt16)(~0x8000); }
                unchecked { P[2] &= (UInt16)(~0x8000); }

		        /* 1 of 4 colors for each 1x2 block, need 8 more bytes */
		        for (y = 0; y < 8; y += 2) {
			        /* time to reload flags? */
			        if ((y == 0) || (y == 4)) {
				        B[0] = *(*data)++;
				        B[1] = *(*data)++;
				        B[2] = *(*data)++;
				        B[3] = *(*data)++;
				        flags = (B[3] << 24) | (B[2] << 16) | (B[1] << 8) | B[0];
				        shifter = 0;
			        }
			        for (x = 0; x < 8; ++x, shifter += 2) {
				        pix = P[(flags >> shifter) & 0x03];
				        *(frame + x) = pix;
				        *(frame + this.g_width + x) = pix;
			        }
			        frame += this.g_width * 2;
		        }
	        }

	        return 0;
        }

        protected unsafe int ipvideo_decode_0xa(UInt16 *frame, Byte **data)
        {
	        int x, y;
	        UInt16[] P = new UInt16[16];
            Byte[] B = new Byte[16];
	        int flags = 0;
	        int shifter = 0;
	        int index;
	        int split;
	        int lower_half;

	        /* 4-color encoding for each 4x4 quadrant, or 4-color encoding on
	         * either top and bottom or left and right halves */
	        P[0] = PIXEL (*data);
	        (*data) += 2;
	        P[1] = PIXEL (*data);
	        (*data) += 2;
	        P[2] = PIXEL (*data);
	        (*data) += 2;
	        P[3] = PIXEL (*data);
	        (*data) += 2;

	        if ((P[0] & 0x8000) == 0) {

		        /* 4-color encoding for each quadrant; need 40 more bytes */
		        B[0] = *(*data)++;
		        B[1] = *(*data)++;
		        B[2] = *(*data)++;
		        B[3] = *(*data)++;
		        for (y = 4; y < 16; y += 4) {
			        for (x = y; x < y + 4; ++x) {
				        P[x] = PIXEL (*data);
				        (*data) += 2;
			        }
			        for (x = y; x < y + 4; ++x)
				        B[x] = *(*data)++;
		        }

		        for (y = 0; y < 8; ++y) {

			        lower_half = (y >= 4) ? 4 : 0;
			        flags = (B[y + 8] << 8) | B[y];

			        for (x = 0, shifter = 0; x < 8; ++x, shifter += 2) {
				        split = (x >= 4) ? 8 : 0;
				        index = split + lower_half + ((flags >> shifter) & 0x03);
				        *frame++ = P[index];
			        }

			        frame += this.g_width - 8;
		        }

            }
            else
            {
                unchecked { P[0] &= (UInt16)(~0x8000); }

		        /* 4-color encoding for either left and right or top and bottom
		         * halves; need 24 more bytes */

                fixed ( Byte* b = &B[0])
                {
		            memcpy (b, *data, 8);
                }
		        (*data) += 8;
		        P[4] = PIXEL (*data);
		        (*data) += 2;
		        P[5] = PIXEL (*data);
		        (*data) += 2;
		        P[6] = PIXEL (*data);
		        (*data) += 2;
		        P[7] = PIXEL (*data);
		        (*data) += 2;
                fixed (Byte* b = &B[8])
		            memcpy (b, *data, 8);
		        (*data) += 8;

		        if ((P[4] & 0x8000) == 0) {

			        /* block is divided into left and right halves */
			        for (y = 0; y < 8; ++y) {

				        flags = (B[y + 8] << 8) | B[y];
				        split = 0;

				        for (x = 0, shifter = 0; x < 8; ++x, shifter += 2) {
					        if (x == 4)
						        split = 4;
					        *frame++ = P[split + ((flags >> shifter) & 0x03)];
				        }

				        frame += this.g_width - 8;
			        }

                }
                else
                {
                    unchecked { P[4] &= (UInt16)(~0x8000); }

			        /* block is divided into top and bottom halves */
			        split = 0;
			        for (y = 0; y < 8; ++y) {

				        flags = (B[y * 2 + 1] << 8) | B[y * 2];
				        if (y == 4)
					        split = 4;

				        for (x = 0, shifter = 0; x < 8; ++x, shifter += 2)
					        *frame++ = P[split + ((flags >> shifter) & 0x03)];

				        frame += this.g_width - 8;
			        }
		        }
	        }

	        return 0;
        }

        protected unsafe int ipvideo_decode_0xb(UInt16 *frame, Byte **data)
        {
	        int x, y;

	        /* 64-color encoding (each pixel in block is a different color) */
	        for (y = 0; y < 8; ++y) {
		        for (x = 0; x < 8; ++x) {
			        *frame++ = PIXEL (*data);
			        (*data) += 2;
		        }
		        frame += this.g_width - 8;
	        }

	        return 0;
        }

        protected unsafe int ipvideo_decode_0xc(UInt16 *frame, Byte **data)
        {
	        int x, y;
	        UInt16 pix;

	        /* 16-color block encoding: each 2x2 block is a different color */
	        for (y = 0; y < 8; y += 2) {
		        for (x = 0; x < 8; x += 2) {
			        pix = PIXEL (*data);
			        (*data) += 2;
			        *(frame + x) = pix;
			        *(frame + x + 1) = pix;
			        *(frame + this.g_width + x) = pix;
			        *(frame + this.g_width + x + 1) = pix;
		        }
		        frame += this.g_width * 2;
	        }

	        return 0;
        }

        protected unsafe int ipvideo_decode_0xd(UInt16 *frame, Byte **data)
        {
	        int x, y;
	        UInt16[] P= new UInt16[4];
	        Byte index = 0;

	        /* 4-color block encoding: each 4x4 block is a different color */
	        P[0] = PIXEL (*data);
	        (*data) += 2;
	        P[1] = PIXEL (*data);
	        (*data) += 2;
	        P[2] = PIXEL (*data);
	        (*data) += 2;
	        P[3] = PIXEL (*data);
	        (*data) += 2;

	        for (y = 0; y < 8; ++y) {
		        if (y < 4)
			        index = 0;
		        else
			        index = 2;

		        for (x = 0; x < 8; ++x) {
			        if (x == 4)
				        ++index;
			        *frame++ = P[index];
		        }
		        frame += this.g_width - 8;
	        }

	        return 0;
        }

        protected unsafe int ipvideo_decode_0xe(UInt16 *frame, Byte **data)
        {
	        int x, y;
	        UInt16 pix;

	        /* 1-color encoding: the whole block is 1 solid color */
	        pix = PIXEL (*data);
	        (*data) += 2;

	        for (y = 0; y < 8; ++y) {
		        for (x = 0; x < 8; ++x) {
			        *frame++ = pix;
		        }
		        frame += this.g_width - 8;
	        }

	        return 0;
        }

        protected unsafe int ipvideo_decode_0xf(UInt16 *frame, Byte **data)
        {
	        int x, y;
            UInt16[] P = new UInt16[2];

	        /* dithered encoding */
	        P[0] = PIXEL (*data);
	        (*data) += 2;
	        P[1] = PIXEL (*data);
	        (*data) += 2;

	        for (y = 0; y < 8; ++y) {
		        for (x = 0; x < 4; ++x) {
			        *frame++ = P[y & 1];
			        *frame++ = P[(y & 1) ^ 1];
		        }
		        frame += this.g_width - 8;
	        }

	        return 0;
        }

        /* copy an 8x8 block from the stream to the frame buffer */
        protected unsafe int ipvideo_copy_block(UInt16* frame, UInt16* src)
        {
	        int i;

	        for (i = 0; i < 8; ++i)
            {
		        memcpy ((Byte*)frame, (Byte*)src, 16);
		        frame += this.g_width;
		        src += this.g_width;
	        }

	        return 0;
        }

        protected unsafe UInt16 PIXEL(Byte* data)
        {
            UInt16 datum = data[1];
            datum <<= 8;
            datum |= data[0];

            return datum;
        }
        #endregion


        /*
            #define PIXEL(s) GST_READ_UINT16_LE (s)

            #define GST_READ_UINT16_LE(data)        (_GST_GET (data, 1, 16,  8) | \
					             _GST_GET (data, 0, 16,  0))

            #define _GST_GET(__data, __idx, __size, __shift) \
	            (((guint##__size) (((guint8 *) (__data))[__idx])) << __shift)
        */
    }
}