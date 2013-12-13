using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.External.Interplay.MVE.Enum;

namespace Bardez.Projects.InfinityPlus1.UnitTesting.Control_Objects
{
    public class MveVideoCoder8SuppliedLogic
    {
        #region Fields
        protected Byte[] BackBuf;
        public Byte[] PaletteData { get; set; }
        public Int32 g_width { get; set; }
        public Int32 g_height { get; set; }
        #endregion


        #region Construction
        /// <summary>Default constructor</summary>
        public MveVideoCoder8SuppliedLogic(Int32 height, Int32 width)
        {
            this.BackBuf = new Byte[this.g_height * this.g_width * 2];
            this.g_height = height;
            this.g_width = width;
        }

        /// <summary>Default constructor</summary>
        public MveVideoCoder8SuppliedLogic(Int32 height, Int32 width, Byte[] backBuffer)
        {
            this.g_height = height;
            this.g_width = width;
            this.BackBuf = new Byte[this.g_height * this.g_width * 2];
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
                Byte* pFrame = g_vBackBuf1 + (curYb * this.g_width) + curXb;
                Byte* pData = PData;
                Byte* pOutput = Output + (curYb * this.g_width) + curXb;

                Byte[] p = new Byte[4];
                Byte[] pat = new Byte[16];
                int i, j, k;
                int x, y;
                
                switch(codeType)
                {
                    case 0x0:
                              copyFrame(pOutput, pFrame + (g_vBackBuf2 - g_vBackBuf1));
                              pOutput += 8;
                              break;

                    case 0x1:
                              pOutput += 8;
                              break;

                    case 0x2:
                              relFar(*(pData)++, 1, out x, out y);
                              copyFrame(pOutput, pOutput + x + y * g_width);
                              pOutput += 8;
                              --pDataRemain;
                              break;

                    case 0x3:
                              relFar(*(pData)++, -1, out x, out y);
                              copyFrame(pOutput, pOutput + x + y * g_width);
                              pOutput += 8;
                              --pDataRemain;
                              break;

                    case 0x4:
                              relClose(*(pData)++, out x, out y);
                              copyFrame(pOutput, pFrame + (g_vBackBuf2 - g_vBackBuf1) + x + y * g_width);
                              pOutput += 8;
                              --pDataRemain;
                              break;

                    case 0x5:
                              x = (SByte)(*(pData)++);
                              y = (SByte)(*(pData)++);
                              copyFrame(pOutput, pFrame + (g_vBackBuf2 - g_vBackBuf1) + x + y * g_width);
                              pOutput += 8;
                              pDataRemain -= 2;
                              break;

                    case 0x6:
                              for (i=0; i<2; i++)
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
                              p[0] = *(pData)++;
                              p[1] = *(pData)++;
                              if (p[0] <= p[1])
                              {
                                  for (i=0; i<8; i++)
                                  {
                                      patternRow2Pixels(pOutput, *(pData)++, p);
                                      pOutput += g_width;
                                  }
                              }
                              else
                              {
                                  for (i=0; i<2; i++)
                                  {
                                      patternRow2Pixels2(pOutput, (Byte)(*(pData) & 0xf), p);
                                      pOutput += 2 * g_width;
                                      patternRow2Pixels2(pOutput, (Byte)(*(pData)++ >> 4), p);
                                      pOutput += 2 * g_width;
                                  }
                              }
                              pOutput -= (8 * g_width - 8);
                              break;

                    case 0x8:
                              if ( (pData)[0] <= (pData)[1])
                              {
                                  for (i=0; i<4; i++)
                                  {
                                      p[0] = *(pData)++;
                                      p[1] = *(pData)++;
                                      pat[0] = *(pData)++;
                                      pat[1] = *(pData)++;
                                      patternQuadrant2Pixels(pOutput, pat[0], pat[1], p);

                                      if ((i & 1) > 0)
                                          pOutput -= (4 * g_width - 4);
                                      else
                                          pOutput += 4 * g_width;
                                  }
                              }
                              else if ( (pData)[6] <= (pData)[7])
                              {
                                  for (i=0; i<4; i++)
                                  {
                                      if ((i & 1) == 0)
                                      {
                                          p[0] = *(pData)++;
                                          p[1] = *(pData)++;
                                      }
                                      pat[0] = *(pData)++;
                                      pat[1] = *(pData)++;
                                      patternQuadrant2Pixels(pOutput, pat[0], pat[1], p);

                                      if ((i & 1) > 0)
                                          pOutput -= (4 * g_width - 4);
                                      else
                                          pOutput += 4 * g_width;
                                  }
                              }
                              else
                              {
                                  for (i=0; i<8; i++)
                                  {
                                      if ((i & 3) == 0)
                                      {
                                          p[0] = *(pData)++;
                                          p[1] = *(pData)++;
                                      }
                                      patternRow2Pixels(pOutput, *(pData)++, p);
                                      pOutput += g_width;
                                  }
                                  pOutput -= (8 * g_width - 8);
                              }
                              break;

                    case 0x9:
                              if ( (pData)[0] <= (pData)[1])
                              {
                                  if ( (pData)[2] <= (pData)[3])
                                  {
                                      p[0] = *(pData)++;
                                      p[1] = *(pData)++;
                                      p[2] = *(pData)++;
                                      p[3] = *(pData)++;

                                      for (i=0; i<8; i++)
                                      {
                                          pat[0] = *(pData)++;
                                          pat[1] = *(pData)++;
                                          patternRow4Pixels(pOutput, pat[0], pat[1], p);
                                          pOutput += g_width;
                                      }

                                      pOutput -= (8 * g_width - 8);
                                  }
                                  else
                                  {
                                      p[0] = *(pData)++;
                                      p[1] = *(pData)++;
                                      p[2] = *(pData)++;
                                      p[3] = *(pData)++;

                                      patternRow4Pixels2(pOutput, *(pData)++, p);
                                      pOutput += 2 * g_width;
                                      patternRow4Pixels2(pOutput, *(pData)++, p);
                                      pOutput += 2 * g_width;
                                      patternRow4Pixels2(pOutput, *(pData)++, p);
                                      pOutput += 2 * g_width;
                                      patternRow4Pixels2(pOutput, *(pData)++, p);
                                      pOutput -= (6 * g_width - 8);
                                  }
                              }
                              else
                              {
                                  if ( (pData)[2] <= (pData)[3])
                                  {
                                      p[0] = *(pData)++;
                                      p[1] = *(pData)++;
                                      p[2] = *(pData)++;
                                      p[3] = *(pData)++;

                                      for (i=0; i<8; i++)
                                      {
                                          pat[0] = *(pData)++;
                                          patternRow4Pixels2x1(pOutput, pat[0], p);
                                          pOutput += g_width;
                                      }

                                      pOutput -= (8 * g_width - 8);
                                  }
                                  else
                                  {
                                      p[0] = *(pData)++;
                                      p[1] = *(pData)++;
                                      p[2] = *(pData)++;
                                      p[3] = *(pData)++;

                                      for (i=0; i<4; i++)
                                      {
                                          pat[0] = *(pData)++;
                                          pat[1] = *(pData)++;
                                          patternRow4Pixels(pOutput, pat[0], pat[1], p);
                                          pOutput += g_width;
                                          patternRow4Pixels(pOutput, pat[0], pat[1], p);
                                          pOutput += g_width;
                                      }

                                      pOutput -= (8 * g_width - 8);
                                  }
                              }
                              break;

                    case 0xa:
                              if ( (pData)[0] <= (pData)[1])
                              {
                                  for (i=0; i<4; i++)
                                  {
                                      p[0] = *(pData)++;
                                      p[1] = *(pData)++;
                                      p[2] = *(pData)++;
                                      p[3] = *(pData)++;
                                      pat[0] = *(pData)++;
                                      pat[1] = *(pData)++;
                                      pat[2] = *(pData)++;
                                      pat[3] = *(pData)++;

                                      patternQuadrant4Pixels(pOutput, pat[0], pat[1], pat[2], pat[3], p);

                                      if ((i & 1) != 0)
                                          pOutput -= (4 * g_width - 4);
                                      else
                                          pOutput += 4 * g_width;
                                  }
                              }
                              else
                              {
                                  if ( (pData)[12] <= (pData)[13])
                                  {
                                      for (i=0; i<4; i++)
                                      {
                                          if ((i&1) == 0)
                                          {
                                              p[0] = *(pData)++;
                                              p[1] = *(pData)++;
                                              p[2] = *(pData)++;
                                              p[3] = *(pData)++;
                                          }

                                          pat[0] = *(pData)++;
                                          pat[1] = *(pData)++;
                                          pat[2] = *(pData)++;
                                          pat[3] = *(pData)++;

                                          patternQuadrant4Pixels(pOutput, pat[0], pat[1], pat[2], pat[3], p);

                                          if ((i & 1) != 0)
                                              pOutput -= (4 * g_width - 4);
                                          else
                                              pOutput += 4 * g_width;
                                      }
                                  }
                                  else
                                  {
                                      for (i=0; i<8; i++)
                                      {
                                          if ((i&3) == 0)
                                          {
                                              p[0] = *(pData)++;
                                              p[1] = *(pData)++;
                                              p[2] = *(pData)++;
                                              p[3] = *(pData)++;
                                          }

                                          pat[0] = *(pData)++;
                                          pat[1] = *(pData)++;
                                          patternRow4Pixels(pOutput, pat[0], pat[1], p);
                                          pOutput += g_width;
                                      }

                                      pOutput -= (8 * g_width - 8);
                                  }
                              }
                              break;

                    case 0xb:
                              for (i=0; i<8; i++)
                              {
                                  memcpy(pOutput, pData, 8);
                                  pOutput += g_width;
                                  pData += 8;
                                  pDataRemain -= 8;
                              }
                              pOutput -= (8 * g_width - 8);
                              break;

                    case 0xc:
                        //  this code de-allocates or moves or marks as collectible the data array. consistently. for no obvious reason. swapping with GemRB 8 bit code.
                        /*
                              for (i=0; i<4; i++)
                              {
                                  for (j=0; j<2; j++)
                                  {
                                      for (k=0; k<4; k++)
                                      {
                                          (pOutput)[j + 2 * k] = (pData)[k];
                                          (pOutput)[g_width + j + 2 * k] = (pData)[k];
                                      }
                                      pOutput += g_width;
                                  }
                                  pData += 4;
                                  pDataRemain -= 4;
                              }
                              pOutput -= (8 * g_width - 8);
                              break;
                         * */

                        /* GemRB code */
                              for (y = 0; y < 8; y += 2)
                              {
                                  for (x = 0; x < 8; x += 2)
                                  {
                                      byte pix = *(pData)++;
                                      *(pOutput + x) = pix;
                                      *(pOutput + x + 1) = pix;
                                      *(pOutput + g_width + x) = pix;
                                      *(pOutput + g_width + x + 1) = pix;
                                  }
                                  pOutput += g_width * 2;
                              }
                              break;

                    case 0xd:
                              for (i=0; i<2; i++)
                              {
                                  for (j=0; j<4; j++)
                                  {
                                      for (k=0; k<4; k++)
                                      {
                                          (pOutput)[k * g_width + j] = (pData)[0];
                                          (pOutput)[k * g_width + j + 4] = (pData)[1];
                                      }
                                  }
                                  pOutput += 4 * g_width;
                                  pData += 2;
                                  pDataRemain -= 2;
                              }
                              pOutput -= (8 * g_width - 8);
                              break;

                    case 0xe:
                              for (i=0; i<8; i++)
                              {
                                  memset(pOutput, *pData, 8);
                                  pOutput += g_width;
                              }
                              ++pData;
                              --pDataRemain;
                              pOutput -= (8 * g_width - 8);
                              break;

                    case 0xf:
                              for (i=0; i<8; i++)
                              {
                                  for (j=0; j<8; j++)
                                  {
                                      (pOutput)[j] = (pData)[(i + j) & 1];
                                  }
                                  pOutput += g_width;
                              }
                              pData += 2;
                              pDataRemain -= 2;
                              pOutput -= (8 * g_width - 8);
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

        protected virtual unsafe void copyFrame(Byte* pDest, Byte* pSrc)
        {
            int i;

            for (i=0; i<8; i++)
            {
                memcpy(pDest, pSrc, 8);
                pDest += g_width;
                pSrc += g_width;
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
    }
}