using System;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals
{
    public enum ResourceType : short /* Int16 */
    {
        Default             	= -1,       //junk
        ImageBitmap         	= 0x0001,
        Movie               	= 0x0002,
        SoundWave           	= 0x0004,
        Wfx                 	= 0x0005,
        Paperdoll           	= 0x0006,
        Bam                 	= 0x03E8,
        Wed                 	= 0x03E9,
        ChUserInterface     	= 0x03EA,
        Tileset             	= 0x03EB,
        Mos                 	= 0x03EC,
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
        Save
    }

    public static class ResourceTypeExtender
    {
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
                case ResourceType.ChUserInterface:
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
                case ResourceType.Mos:
                    extension = "mos";
                    break;
                case ResourceType.Movie:
                    extension = "mve";
                    break;
                case ResourceType.Paperdoll:
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
                default:
                    extension = null;
                    break;
            }

            return extension;
        }
    }
}