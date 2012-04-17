using System;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Enum;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Globals;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Game.Component
{
    /// <summary>Represents the party history of the character; time in party, XP, kills, favorite ___, etc.</summary>
    public class CharacterInfo : IInfinityFormat
    {
        #region Constants
        /// <summary>Represents the size of the structure on disk</summary>
        private const Int32 StructSize = 116;
        #endregion


        #region Fields
        /// <summary>StringReference Index to the name of this character's most powerful enemy vanquished</summary>
        public StringReference MostPowerfulVanquishedName { get; set; }

        /// <summary>XP value of this character's most powerful enemy vanquished</summary>
        public UInt32 MostPowerfulVanquishedXP { get; set; }

        /// <summary>How long the character has NOT been in the party (ticks increment 1/15 of a second)</summary>
        /// <remarks>
        ///     0 seems to indicate "I've been in the party since tick 0"
        ///     Seems more like a "how long I've *not* been in the party. Changing.
        /// </remarks>
        public UInt32 TimeAwayFromParty { get; set; }

        /// <summary>When the character joined the party (ticks increment 1/15 of a second)</summary>
        public UInt32 TimeJoinedParty { get; set; }

        /// <summary>Value indicating whether the character is a party member or not</summary>
        /// <value>0 = false; 1 = true</value>
        protected UInt16 isPartyMember { get; set; }

        /// <summary>Unknown value prior to the asterisk</summary>
        /// <remarks>
        ///     IESDP says it is the first letter, then the unknown. However, all viewed data indicates
        ///     a 0x00 then a 0x2A (which is the asterisk)
        ///     
        ///     Perhaps in-memory the value is matched and some sort of hack to not re-load a CRE
        ///     if they cannot find it?
        /// </remarks>
        public Byte Unknown { get; set; }

        /// <summary>Byte of the character with which the first character of the CRE file ResRef was replaced with</summary>
        /// <value>0 (null) if not performed or unnecessary.</value>
        public Byte ReplacedLetterByte { get; set; }

        /// <summary>Represents the total count of XP gaind from this character killing things during this chapter</summary>
        public UInt32 TotalChapterKillsXP { get; set; }

        /// <summary>Represents the total count of kills for this character during this chapter</summary>
        public UInt32 TotalChapterKills { get; set; }

        /// <summary>Represents the total count of XP gaind from this character killing things</summary>
        public UInt32 TotalKillsXP { get; set; }

        /// <summary>Represents the total count of kills for this character</summary>
        public UInt32 TotalKills { get; set; }

        /// <summary>Represents the four most favorite spells of the character</summary>
        /// <remarks>Size is 4.</remarks>
        public ResourceReference[] FavoriteSpells { get; set; }

        /// <summary>Represents the count of times a favorite spell has been cast, matching indexes between this and <see cref="FavoriteSpells" />.</summary>
        /// <remarks>
        ///     Size is 4.
        ///     How does the engine notice if, say, you cast Sleep, Magic Missile, Chromatic Orb, and Grease a lot
        ///     but discover the joy of cloudkill 10 levels later?
        ///     For conversion, I will need to make these two a struct for all spells cast,
        ///     and also ignore any cross-class differences for the same spell.
        /// </remarks>
        public UInt16[] FavoriteSpellCounts { get; set; }

        /// <summary>Represents the four most favorite weapons of the character</summary>
        /// <remarks>Size is 4.</remarks>
        public ResourceReference[] FavoriteWeapons { get; set; }

        /// <summary>Represents the length of time a favorite weapon has been equipped in combat, matching indexes between this and <see cref="FavoriteWeapons" />.</summary>
        /// <remarks>
        ///     Size is 4.
        ///     IESDP indicates combat time with ticks in increments of 1/15 of a second.
        ///     How does the engine notice if, say, you hold four weapons (bow, xbow, sword, dagger) for a year
        ///     but discover the joy of a +3 dagger later on?
        ///     For conversion, I will need to make these two a struct for all weapons equipped.
        /// </remarks>
        public UInt16[] FavoriteWeaponTime { get; set; }
        #endregion


        #region Properties
        /// <summary>Value indicating whether the character is a party member or not</summary>
        /// <value>0 = false; 1 = true</value>
        public Boolean IsPartyMember
        {
            get { return Convert.ToBoolean(this.isPartyMember); }
            set { this.isPartyMember = Convert.ToUInt16(value ? 1 : 0); }
        }

        /// <summary>Character with which the first character of the CRE file ResRef was replaced with</summary>
        /// <value>0 (null) if not performed or unnecessary.</value>
        public Char FirstLetterOfCreFileResRef
        {
            get
            {
                String character = ASCIIEncoding.ASCII.GetString(new Byte[] { this.ReplacedLetterByte });
                return character[0];
            }
            set
            {
                Byte[] characters = ASCIIEncoding.ASCII.GetBytes(new Char[] { value });
                this.ReplacedLetterByte = characters[0];
            }
        }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.MostPowerfulVanquishedName = new StringReference();
            this.FavoriteSpells = new ResourceReference[4];
            this.FavoriteSpellCounts = new UInt16[4];
            this.FavoriteWeapons = new ResourceReference[4];
            this.FavoriteWeaponTime = new UInt16[4];

            //two birds, one loop
            for (Int32 i = 0; i < 4; ++i)
            {
                this.FavoriteSpells[i] = new ResourceReference();
                this.FavoriteWeapons[i] = new ResourceReference();
            }
        }
        #endregion


        #region IInfinityFormat IO Methods
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public virtual void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format from the input stream.</summary>
        /// <param name="input">Input stream to read from</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public virtual void Read(Stream input, Boolean fullRead)
        {
            this.Read(input);
        }

        /// <summary>This public method reads file format from the output stream, after the header has already been read.</summary>
        public virtual void ReadBody(Stream input)
        {
            this.Initialize();

            Byte[] buffer = ReusableIO.BinaryRead(input, CharacterInfo.StructSize);

            this.MostPowerfulVanquishedName.StringReferenceIndex = ReusableIO.ReadInt32FromArray(buffer, 0);
            this.MostPowerfulVanquishedXP = ReusableIO.ReadUInt32FromArray(buffer, 4);
            this.TimeAwayFromParty = ReusableIO.ReadUInt32FromArray(buffer, 8);
            this.TimeJoinedParty = ReusableIO.ReadUInt32FromArray(buffer, 12);
            this.isPartyMember = ReusableIO.ReadUInt16FromArray(buffer, 16);
            this.Unknown = buffer[18];
            this.ReplacedLetterByte = buffer[19];
            this.TotalChapterKillsXP = ReusableIO.ReadUInt32FromArray(buffer, 20);
            this.TotalChapterKills = ReusableIO.ReadUInt32FromArray(buffer, 24);
            this.TotalKillsXP = ReusableIO.ReadUInt32FromArray(buffer, 28);
            this.TotalKills = ReusableIO.ReadUInt32FromArray(buffer, 32);

            for (Int32 i = 0; i < 4; ++i)
                this.FavoriteSpells[i].ResRef = ReusableIO.ReadStringFromByteArray(buffer, (36 + (8 * i)), CultureConstants.CultureCodeEnglish);
            
            for (Int32 i = 0; i < 4; ++i)
                this.FavoriteSpellCounts[i] = ReusableIO.ReadUInt16FromArray(buffer, (68 + (2 * i)));

            for (Int32 i = 0; i < 4; ++i)
                this.FavoriteWeapons[i].ResRef = ReusableIO.ReadStringFromByteArray(buffer, (76 + (8 * i)), CultureConstants.CultureCodeEnglish);

            for (Int32 i = 0; i < 4; ++i)
                this.FavoriteWeaponTime[i] = ReusableIO.ReadUInt16FromArray(buffer, (108 + (2 * i)));
        }

        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Output stream to write to</param>
        public virtual void Write(Stream output)
        {
            ReusableIO.WriteInt32ToStream(this.MostPowerfulVanquishedName.StringReferenceIndex, output);
            ReusableIO.WriteUInt32ToStream(this.MostPowerfulVanquishedXP, output);
            ReusableIO.WriteUInt32ToStream(this.TimeAwayFromParty, output);
            ReusableIO.WriteUInt32ToStream(this.TimeJoinedParty, output);
            ReusableIO.WriteUInt16ToStream(this.isPartyMember, output);
            output.WriteByte(this.Unknown);
            output.WriteByte(this.ReplacedLetterByte);
            ReusableIO.WriteUInt32ToStream(this.TotalChapterKillsXP, output);
            ReusableIO.WriteUInt32ToStream(this.TotalChapterKills, output);
            ReusableIO.WriteUInt32ToStream(this.TotalKillsXP, output);
            ReusableIO.WriteUInt32ToStream(this.TotalKills, output);

            for (Int32 i = 0; i < 4; ++i)
                 ReusableIO.WriteStringToStream(this.FavoriteSpells[i].ResRef, output, CultureConstants.CultureCodeEnglish);

            for (Int32 i = 0; i < 4; ++i)
                ReusableIO.WriteUInt16ToStream(this.FavoriteSpellCounts[i], output);

            for (Int32 i = 0; i < 4; ++i)
                ReusableIO.WriteStringToStream(this.FavoriteWeapons[i].ResRef, output, CultureConstants.CultureCodeEnglish);

            for (Int32 i = 0; i < 4; ++i)
                ReusableIO.WriteUInt16ToStream(this.FavoriteWeaponTime[i], output);
        }
        #endregion


        #region ToString methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(StringFormat.ToStringAlignment("StrRef to Most Powerful Foe Vanquished"));
            builder.Append(this.MostPowerfulVanquishedName.StringReferenceIndex);
            builder.Append(StringFormat.ToStringAlignment("XP award for Most Powerful Foe Vanquished"));
            builder.Append(this.MostPowerfulVanquishedXP);
            builder.Append(StringFormat.ToStringAlignment("Time away from party"));
            builder.Append(this.TimeAwayFromParty);
            builder.Append(StringFormat.ToStringAlignment("Time joined party"));
            builder.Append(this.TimeJoinedParty);
            builder.Append(StringFormat.ToStringAlignment("Is party member? (value)"));
            builder.Append(this.isPartyMember);
            builder.Append(StringFormat.ToStringAlignment("Is party member? (Boolean)"));
            builder.Append(this.IsPartyMember);
            builder.Append(StringFormat.ToStringAlignment("Unknown"));
            builder.Append(this.Unknown);
            builder.Append(StringFormat.ToStringAlignment("First Letter of Creature resource"));
            if (this.FirstLetterOfCreFileResRef != Char.MinValue) //null
                builder.Append(this.FirstLetterOfCreFileResRef);
            builder.Append(StringFormat.ToStringAlignment("XP value of Total Kills (chapter)"));
            builder.Append(this.TotalChapterKillsXP);
            builder.Append(StringFormat.ToStringAlignment("Total Kills (chapter)"));
            builder.Append(this.TotalChapterKills);
            builder.Append(StringFormat.ToStringAlignment("XP value of Total Kills"));
            builder.Append(this.TotalKillsXP);
            builder.Append(StringFormat.ToStringAlignment("Total Kills"));
            builder.Append(this.TotalKills);

            for (Int32 index = 0; index < 4; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Favorite spell {0}", index)));
                builder.Append(String.Format("'{0}'", this.FavoriteSpells[index].ZResRef));
            }

            for (Int32 index = 0; index < 4; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Favorite spell count {0}", index)));
                builder.Append(this.FavoriteSpellCounts[index]);
            }

            for (Int32 index = 0; index < 4; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Favorite weapon {0}", index)));
                builder.Append(String.Format("'{0}'", this.FavoriteWeapons[index].ZResRef));
            }

            for (Int32 index = 0; index < 4; ++index)
            {
                builder.Append(StringFormat.ToStringAlignment(String.Format("Favorite weapon time {0}", index)));
                builder.Append(this.FavoriteWeaponTime[index]);
            }

            return builder.ToString();
        }
        #endregion
    }
}