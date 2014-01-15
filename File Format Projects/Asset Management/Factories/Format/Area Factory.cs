using System;
using System.IO;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Version;
using Bardez.Projects.InfinityPlus1.Information;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Factories
{
    /// <summary>Creates and returns an Area instance based on an input stream</summary>
    public static class AreaFactory
    {
        /// <summary>Builds an area from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="engine">Game engine, defining interpretation of an area</param>
        /// <returns>The built area</returns>
        public static Object BuildArea(Stream input, GameEngine engine)
        {
            Object area = null;

            try
            {
                lock (input)
                {
                    //validation
                    if (!input.CanRead)
                        throw new InvalidOperationException("The input Stream cannot be read from.");
                    else if (!input.CanSeek)
                        throw new InvalidOperationException("The input Stream cannot seek, which is required.");

                    Int64 position = input.Position;
                    Int64 availableSize = input.Length - input.Position;
                    Int32 peekLen = 8;

                    if (availableSize < peekLen)
                        throw new InvalidDataException("The input Stream does not have sufficient data to read.");

                    //read 8 bytes to get the type and version
                    Byte[] peek = ReusableIO.BinaryRead(input, peekLen);
                    ReusableIO.SeekIfAble(input, position);

                    String type = ReusableIO.ReadStringFromByteArray(peek, 0, CultureConstants.CultureCodeEnglish, 4);
                    String version = ReusableIO.ReadStringFromByteArray(peek, 4, CultureConstants.CultureCodeEnglish, 4);

                    if (type != "AREA")
                        throw new InvalidDataException(String.Format("The first four bytes of the data ([{0}, {1}, {2}, {3}]) did not indicate an area.", peek[0], peek[1], peek[2], peek[3]));

                    //now, build the actual object
                    if (version == "V1.0")
                    {
                        switch (engine)
                        {
                            case GameEngine.IcewindDale:
                                IcewindArea iwdArea = new IcewindArea();
                                iwdArea.Initialize();
                                iwdArea.Read(input, true);
                                area = iwdArea;
                                break;

                            case GameEngine.PlanescapeTorment:
                                TormentArea pstArea = new TormentArea();
                                pstArea.Initialize();
                                pstArea.Read(input, true);
                                area = pstArea;
                                break;

                            case GameEngine.BaldursGate:
                            case GameEngine.BaldursGate2:
                            case GameEngine.BaldursGateEnhancedEdition:
                            case GameEngine.BaldursGate2EnhancedEdition:
                            default:
                                BaldurArea bgArea = new BaldurArea();
                                bgArea.Initialize();
                                bgArea.Read(input, true);
                                area = bgArea;
                                break;
                        }
                    }
                    else if (version == "V9.1")
                    {
                        Icewind2Area iwd2Area = new Icewind2Area();
                        iwd2Area.Initialize();
                        iwd2Area.Read(input, true);
                        area = iwd2Area;
                    }
                    else
                        throw new InvalidDataException(String.Format("The second four bytes of the data ([{0}, {1}, {2}, {3}]) did not indicate a known area type (V1.0 or V9.1).", peek[4], peek[5], peek[6], peek[7]));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Encountered an error while trying to build an area from the input stream.", ex);
            }

            return area;
        }
    }
}