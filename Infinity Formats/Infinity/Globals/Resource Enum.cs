using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals
{
    /// <summary>This is an enumeration of known Infinity Engine resource codes</summary>
    public enum ResourceType : short /* Int16 */
    {
        ImageBitmap         	= 0x0001,
        Movie               	= 0x0002,
        Targa               	= 0x0003,   //TARGA image (NWN)
        SoundWave           	= 0x0004,
        Wfx                 	= 0x0005,
        PackedLayeredTexture    = 0x0006,

        //NWN says INI is 7
        //NWN says TXT is 10

        Bam                 	= 0x03E8,
        Wed                 	= 0x03E9,
        ChunkedUserInterface    = 0x03EA,
        Tileset             	= 0x03EB,
        Mosaic                 	= 0x03EC,
        Item                	= 0x03ED,
        Spell               	= 0x03EE,
        ScriptGame          	= 0x03EF,
        Identifiers         	= 0x03F0,
        Creature            	= 0x03F1,
        Area                	= 0x03F2,
        Dialog              	= 0x03F3,
        TwoDimensionalArray 	= 0x03F4,
        Game                	= 0x03F5,
        Store               	= 0x03F6,
        Worldmap            	= 0x03F7,
        Effect              	= 0x03F8,
        ScriptCharacter     	= 0x03F9,
        Character           	= 0x03FA,   //also = 0x03F8?
        VisualCastingEffects    = 0x03FB,
        UnknownVef          	= 0x03FC,
        Projectile          	= 0x03FD,
        Biography           	= 0x03FE,
        UnknownBa           	= 0x044C,
        Ini                 	= 0x0802,
        Src                 	= 0x0803,

        //BG:EE
        Font                    = 0x0400,
        GUI                     = 0x0402,
        SQL                     = 0x0403,
        UnknownPVR              = 0x0404,
        Unknown405              = 0x0405,

        //BG2:EE
        Unknown3FF              = 0x03FF,   //Is this a movie format? OGG? Totorial naming implies the new tutorial movies.

        //Invalid types
        Save                    = -50,      //arbitrary negative number
        Biff,
        CBif,
        Talk,
        ACM,
        Mus,
        Res,
        BAF,
        TOH,
        TOT,
        Variables,
        Key,
        Invalid             	= -1,       //junk; I am borrowing from the NWN key file spec with this
    }

    /// <summary>This class is an extender that performs additional operations on the ResourceType class</summary>
    public static class ResourceTypeExtender
    {
        /// <summary>Generates the file extension of the known resource type</summary>
        /// <param name="resourceType">Code of the resource type</param>
        /// <returns>The file extension of the resource type</returns>
        public static String ToFileExtension(this ResourceType resourceType)
        {
            String extension;

            switch (resourceType)
            {
                case ResourceType.Area:
                    extension = "are";
                    break;
                case ResourceType.Bam:
                    extension = "bam";
                    break;
                case ResourceType.Biography:
                    extension = "bio";
                    break;
                case ResourceType.Character:
                    extension = "chr";
                    break;
                case ResourceType.ChunkedUserInterface:
                    extension = "chu";
                    break;
                case ResourceType.Creature:
                    extension = "cre";
                    break;
                case ResourceType.Dialog:
                    extension = "dlg";
                    break;
                case ResourceType.Effect:
                    extension = "eff";
                    break;
                case ResourceType.Game:
                    extension = "gam";
                    break;
                case ResourceType.Identifiers:
                    extension = "ids";
                    break;
                case ResourceType.ImageBitmap:
                    extension = "bmp";
                    break;
                case ResourceType.Ini:
                    extension = "ini";
                    break;
                case ResourceType.Item:
                    extension = "itm";
                    break;
                case ResourceType.Mosaic:
                    extension = "mos";
                    break;
                case ResourceType.Movie:
                    extension = "mve";
                    break;
                case ResourceType.PackedLayeredTexture:
                    extension = "plt";
                    break;
                case ResourceType.Projectile:
                    extension = "pro";
                    break;
                case ResourceType.Save:
                    extension = "sav";
                    break;
                case ResourceType.ScriptCharacter:
                    extension = "bs";
                    break;
                case ResourceType.ScriptGame:
                    extension = "bcs";
                    break;
                case ResourceType.SoundWave:
                    extension = "wav";
                    break;
                case ResourceType.Spell:
                    extension = "spl";
                    break;
                case ResourceType.Src:
                    extension = "src";
                    break;
                case ResourceType.Store:
                    extension = "sto";
                    break;
                case ResourceType.Tileset:
                    extension = "tis";
                    break;
                case ResourceType.TwoDimensionalArray:
                    extension = "2da";
                    break;
                case ResourceType.VisualCastingEffects:
                    extension = "vvc";
                    break;
                case ResourceType.Wed:
                    extension = "wed";
                    break;
                case ResourceType.Wfx:
                    extension = "wfx";
                    break;
                case ResourceType.Worldmap:
                    extension = "wmp";
                    break;
                case ResourceType.GUI:
                    extension = "gui";
                    break;
                case ResourceType.SQL:
                    extension = "sql";
                    break;
                default:
                    extension = null;
                    break;
            }

            return extension;
        }

        /// <summary>Reverse mapping of a file extension to its resource type</summary>
        /// <param name="extension">File extension to translate</param>
        /// <returns>The mapped resource type</returns>
        public static ResourceType FromExtension(String extension)
        {
            ResourceType type = ResourceType.Invalid;

            switch (extension.ToUpper())
            {
                case "BMP":
                    type = ResourceType.ImageBitmap;
                    break;
                case "MVE":
                    type = ResourceType.Movie;
                    break;
                case "TGA":
                    type = ResourceType.Targa;
                    break;
                case "WAV":
                    type = ResourceType.SoundWave;
                    break;
                case "WFX":
                    type = ResourceType.Wfx;
                    break;
                case "PLT":
                    type = ResourceType.PackedLayeredTexture;
                    break;
                case "BAM":
                    type = ResourceType.Bam;
                    break;
                case "WED":
                    type = ResourceType.Wed;
                    break;
                case "CHU":
                    type = ResourceType.ChunkedUserInterface;
                    break;
                case "TIS":
                    type = ResourceType.Tileset;
                    break;
                case "MOS":
                    type = ResourceType.Mosaic;
                    break;
                case "ITM":
                    type = ResourceType.Item;
                    break;
                case "SPL":
                    type = ResourceType.Spell;
                    break;
                case "BCS":
                    type = ResourceType.ScriptGame;
                    break;
                case "IDS":
                    type = ResourceType.Identifiers;
                    break;
                case "CRE":
                    type = ResourceType.Creature;
                    break;
                case "ARE":
                    type = ResourceType.Area;
                    break;
                case "DLG":
                    type = ResourceType.Dialog;
                    break;
                case "2DA":
                    type = ResourceType.TwoDimensionalArray;
                    break;
                case "GAM":
                    type = ResourceType.Game;
                    break;
                case "STO":
                    type = ResourceType.Store;
                    break;
                case "WMP":
                    type = ResourceType.Worldmap;
                    break;
                case "EFF":
                    type = ResourceType.Effect;
                    break;
                case "BS":
                    type = ResourceType.ScriptCharacter;
                    break;
                case "CHR":
                    type = ResourceType.Character;
                    break;
                case "VVC":
                    type = ResourceType.VisualCastingEffects;
                    break;
                case "VEF":
                    type = ResourceType.UnknownVef;
                    break;
                case "PRO":
                    type = ResourceType.Projectile;
                    break;
                case "BIO":
                    type = ResourceType.Biography;
                    break;
                case "INI":
                    type = ResourceType.Ini;
                    break;
                case "SRC":
                    type = ResourceType.Src;
                    break;
                case "CBF":
                    type = ResourceType.CBif;
                    break;
                case "SAV":
                    type = ResourceType.Save;
                    break;
                case "BIF":
                    type = ResourceType.Biff;
                    break;
                case "TOH":
                    type = ResourceType.TOH;
                    break;
                case "TOT":
                    type = ResourceType.TOT;
                    break;
                case "TLK":
                    type = ResourceType.Talk;
                    break;
                case "ACM":
                    type = ResourceType.ACM;
                    break;
                case "MUS":
                    type = ResourceType.Mus;
                    break;
                case "RES":
                    type = ResourceType.Res;
                    break;
                case "BAF":
                    type = ResourceType.BAF;
                    break;
                case "VAR":
                    type = ResourceType.Variables;
                    break;
                case "KEY":
                    type = ResourceType.Key;
                    break;


                //not 100% certain, yet
                case "BA":
                    type = ResourceType.UnknownBa;
                    break;
                case "FNT":
                    type = ResourceType.Font;
                    break;
                case "GUI":
                    type = ResourceType.GUI;
                    break;
                case "SQL":
                    type = ResourceType.SQL;
                    break;
                case "PVR":
                    type = ResourceType.UnknownPVR;
                    break;
                case "405":     //I have no clue whatsoever
                    type = ResourceType.Unknown405;
                    break;
                case "WBM":
                    type = ResourceType.Unknown3FF;
                    break;
            }

            return type;
        }
    }
}