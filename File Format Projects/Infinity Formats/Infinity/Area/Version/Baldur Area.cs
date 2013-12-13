using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Creature.Creature1;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Version
{
    /// <summary>Represents a single area in BG, BG2</summary>
    public class BaldurArea : AreaBase
    {
        #region Fields
        /// <summary>Header for this area</summary>
        public CommonAreaHeader Header { get; set; }

        /// <summary>Collection of spawn points in this area</summary>
        public List<CommonSpawnPoint> SpawnPoints { get; set; }

        /// <summary>Collection of map notes in this area</summary>
        public List<CommonMapNote> MapNotes { get; set; }

        /// <summary>Represents the collection of creatures, keyed with the offset as the unique key</summary>
        public Dictionary<Int32, Creature1> Creatures { get; set; }
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public override void Initialize()
        {
            base.Initialize();

            this.Header = new CommonAreaHeader();
            this.SpawnPoints = new List<CommonSpawnPoint>();
            this.MapNotes = new List<CommonMapNote>();
            this.Creatures = new Dictionary<Int32, Creature1>();
        }
        #endregion


        #region IO method implemetations
        #region Read
        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public override void Read(Stream input, Boolean fullRead)
        {
            if (fullRead)
                this.Read(input);
            else
            {
                this.Header = new CommonAreaHeader();
                this.Header.Read(input, false);
            }
        }

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public override void ReadBody(Stream input)
        {
            this.Initialize();

            this.ReadHeader(input);
            this.ReadActors(input, this.Header.OffsetActors, this.Header.CountActors);
            this.ReadRegions(input, this.Header.OffsetRegions, this.Header.CountRegions);
            this.ReadSpawnPoints(input, this.Header.OffsetSpawnPoints, this.Header.CountSpawnPoints);
            this.ReadEntrances(input, this.Header.OffsetEntrances, this.Header.CountEntrances);
            this.ReadContainers(input, this.Header.OffsetContainers, this.Header.CountContainers);
            this.ReadItems(input, this.Header.OffsetItems, this.Header.CountItems);
            this.ReadVertices(input, this.Header.OffsetVertices, this.Header.CountVertices);
            this.ReadAmbientSounds(input, this.Header.OffsetAmbients, this.Header.CountAmbients);
            this.ReadVariables(input, this.Header.OffsetVariables, this.Header.CountVariables);
            this.ReadExploredMask(input, this.Header.OffsetVertices);
            this.ReadDoors(input, this.Header.OffsetDoors, this.Header.CountDoors);
            this.ReadAnimations(input, this.Header.OffsetAnimations, this.Header.CountAnimations);
            this.ReadMapNotes(input, this.Header.OffsetMapNote, this.Header.CountMapNote);
            this.ReadProjectileTraps(input, this.Header.OffsetProjectileTrap, this.Header.CountProjectileTrap);
            this.ReadSongs(input, this.Header.OffsetSongEntries);
            this.ReadSleepInterruptions(input, this.Header.OffsetRestInterruptions);
            this.ReadEmbeddedCreatures(input);
        }

        /// <summary>Reads the header from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        protected override void ReadHeader(Stream input)
        {
            this.Header.Read(input);
        }

        /// <summary>Reads spawn points from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of spawn points to read</param>
        protected override void ReadSpawnPoints(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    CommonSpawnPoint spawnPoint = new CommonSpawnPoint();
                    spawnPoint.Read(input);
                    this.SpawnPoints.Add(spawnPoint);
                }
            }
        }

        /// <summary>Reads map notes from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of actors to read</param>
        protected override void ReadMapNotes(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    CommonMapNote mapNote = new CommonMapNote();
                    mapNote.Read(input);
                    this.MapNotes.Add(mapNote);
                }
            }
        }

        /// <summary>Reads embedded creatures from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        protected override void ReadEmbeddedCreatures(Stream input)
        {
            foreach (Actor actor in this.Actors)
            {
                if (actor.OffsetCreStruct > 0)
                {
                    ReusableIO.SeekIfAble(input, actor.OffsetCreStruct);
                    Byte[] creatureBuffer = ReusableIO.BinaryRead(input, actor.SizeCreStruct);
                    using (MemoryStream subStream = new MemoryStream(creatureBuffer))
                    {
                        Creature1 creature = new Creature1();
                        creature.Read(subStream);
                        this.Creatures.Add(actor.OffsetCreStruct, creature);
                    }
                }
            }
        }
        #endregion


        #region Write
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public override void Write(Stream output)
        {
            //data integrity
            this.MaintainMinimalDataIntegrity();

            this.Header.Write(output);
            this.WriteActors(output, this.Header.OffsetActors);
            this.WriteRegions(output, this.Header.OffsetRegions);
            this.WriteSpawnPoints(output, this.Header.OffsetSpawnPoints);
            this.WriteEntrances(output, this.Header.OffsetEntrances);
            this.WriteContainers(output, this.Header.OffsetContainers);
            this.WriteItems(output, this.Header.OffsetItems);
            this.WriteVertices(output, this.Header.OffsetVertices);
            this.WriteAmbientSounds(output, this.Header.OffsetAmbients);
            this.WriteVariables(output, this.Header.OffsetVariables);
            this.WriteExploredMask(output, this.Header.OffsetVertices);
            this.WriteDoors(output, this.Header.OffsetDoors);
            this.WriteAnimations(output, this.Header.OffsetAnimations);
            this.WriteMapNotes(output, this.Header.OffsetMapNote);
            this.WriteProjectileTraps(output, this.Header.OffsetProjectileTrap);
            this.WriteSongs(output, this.Header.OffsetSongEntries);
            this.WriteSleepInterruptions(output, this.Header.OffsetRestInterruptions);
            this.WriteEmbeddedCreatures(output);
        }

        /// <summary>Writes spawn points to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected override void WriteSpawnPoints(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (CommonSpawnPoint spawnPoint in this.SpawnPoints)
                    spawnPoint.Write(output);
            }
        }

        /// <summary>Writes map notes to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected override void WriteMapNotes(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (CommonMapNote mapNote in this.MapNotes)
                    mapNote.Write(output);
            }
        }

        /// <summary>Writes embedded creatures to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected override void WriteEmbeddedCreatures(Stream output)
        {
            foreach (Int32 key in this.Creatures.Keys)
            {
                Byte[] buffer;
                Int64 bufferLen;

                using (MemoryStream subStream = new MemoryStream())
                {
                    this.Creatures[key].Write(subStream);
                    buffer = subStream.GetBuffer();
                    bufferLen = subStream.Length;
                }

                ReusableIO.SeekIfAble(output, key);
                output.Write(buffer, 0, Convert.ToInt32(bufferLen));
            }
        }
        #endregion
        #endregion


        #region Data integrity
        /// <summary>Collects a set of offsets and their lengths to be examined</summary>
        /// <returns>A List of Tuples (Offset and length)</returns>
        protected override List<Tuple<Int64, Int64>> CollectOffsets()
        {
            List<Tuple<Int64, Int64>> offsetCollection = new List<Tuple<Int64, Int64>>();

            //header
            offsetCollection.Add(new Tuple<Int64, Int64>(0L, CommonAreaHeader.StructSize));

            //actors
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetActors, this.Header.CountActors * Actor.StructSize));

            //regions
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetRegions, this.Header.CountRegions * Region.StructSize));

            //spawn points
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetSpawnPoints, this.Header.CountSpawnPoints * CommonSpawnPoint.StructSize));

            //entrances
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetEntrances, this.Header.CountEntrances * Entrance.StructSize));

            //containers
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetContainers, this.Header.CountContainers * Container.StructSize));

            //items
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetItems, this.Header.CountItems * ItemInstance.StructSize));

            //vertices
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetVertices, this.Header.CountVertices * Point.StructSize));

            //ambients
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetAmbients, this.Header.CountAmbients * Ambient.StructSize));

            //variables
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetVariables, this.Header.CountVariables * Variable.StructSize));

            //explored cell bitmask
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetExploredBitmask, this.ExploredMask.Size));

            //doors
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetDoors, this.Header.CountDoors * Door.StructSize));

            //animations
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetAnimations, this.Header.CountAnimations * Animation.StructSize));

            //map notes
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetMapNote, this.Header.OffsetMapNote * CommonMapNote.StructSize));

            //projectile traps
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetProjectileTrap, this.Header.CountProjectileTrap * ProjectileTrap.StructSize));

            //songs
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetSongEntries, Song.StructSize));

            //rest interruptions
            offsetCollection.Add(new Tuple<Int64, Int64>(this.Header.OffsetRestInterruptions, RestInterruption.StructSize));

            //embedded creatures
            foreach (Actor actor in this.Actors)
                if (actor.OffsetCreStruct > 0)
                    offsetCollection.Add(new Tuple<Int64, Int64>(actor.OffsetCreStruct, actor.SizeCreStruct));

            return offsetCollection;
        }

        /// <summary>Resets the offsets within the file stream</summary>
        protected override void ResetOffsets()
        {
            Int32 offset = 0;

            //skip header
            offset += CommonAreaHeader.StructSize;

            //actors
            this.Header.OffsetActors = offset;
            offset += (this.Header.CountActors * Actor.StructSize);

            //regions
            this.Header.OffsetRegions = offset;
            offset += (this.Header.CountRegions * Region.StructSize);

            //spawn points
            this.Header.OffsetSpawnPoints = offset;
            offset += (this.Header.CountSpawnPoints * CommonSpawnPoint.StructSize);

            //entrances
            this.Header.OffsetEntrances = offset;
            offset += (this.Header.CountEntrances * Entrance.StructSize);

            //containers
            this.Header.OffsetContainers = offset;
            offset += (this.Header.CountContainers * Container.StructSize);

            //items
            this.Header.OffsetItems = offset;
            offset += (this.Header.CountItems * ItemInstance.StructSize);

            //vertices
            this.Header.OffsetVertices = offset;
            offset += (this.Header.CountVertices * Point.StructSize);

            //amibents
            this.Header.OffsetAmbients = offset;
            offset += (this.Header.CountAmbients * Ambient.StructSize);

            //variables
            this.Header.OffsetVariables = offset;
            offset += (this.Header.CountVariables * Variable.StructSize);

            //explored bitmask
            this.Header.OffsetExploredBitmask = offset;
            offset += this.ExploredMask.Size;

            //doors
            this.Header.OffsetDoors = offset;
            offset += (this.Header.CountDoors * Door.StructSize);

            //animations
            this.Header.OffsetAnimations = offset;
            offset += (this.Header.CountAnimations * Animation.StructSize);

            //map notes
            this.Header.OffsetMapNote = offset;
            offset += (this.Header.CountMapNote * CommonMapNote.StructSize);

            //projectile trap
            this.Header.OffsetProjectileTrap = offset;
            offset += (this.Header.CountProjectileTrap * ProjectileTrap.StructSize);

            //song entries
            this.Header.OffsetSongEntries = offset;
            offset += Song.StructSize;

            //rest interruptions
            this.Header.OffsetRestInterruptions = offset;
            offset += RestInterruption.StructSize;

            //creature offsets
            foreach (Actor actor in this.Actors)
                if (actor.OffsetCreStruct > 0)
                {
                    Creature1 cre = this.Creatures[actor.OffsetCreStruct];
                    this.Creatures.Remove(actor.OffsetCreStruct);
                    this.Creatures.Add(offset, cre);

                    actor.OffsetCreStruct = offset;

                    offset += actor.SizeCreStruct;
                }
        }
        #endregion


        #region ToString methods
        /// <summary>Generates a description String of this Type</summary>
        /// <returns>A description String of this Type</returns>
        protected override String GenerateTypeDescriptionString()
        {
            return "Baldur's Gate area";
        }

        /// <summary>Generates a description String of the header</summary>
        /// <returns>A description String of the header</returns>
        protected override String GenerateHeaderString()
        {
            return this.Header.ToString();
        }

        /// <summary>Generates a human-readable console output describing the spawn points in this area</summary>
        /// <returns>A multi-line String</returns>
        protected override String GenerateSpawnPointString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.SpawnPoints.Count; ++i)
                sb.Append(this.SpawnPoints[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the map notes in this area</summary>
        /// <returns>A multi-line String</returns>
        protected override String GenerateMapNotesString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.MapNotes.Count; ++i)
                sb.Append(this.MapNotes[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the embedded creatures in this area</summary>
        /// <returns>A multi-line String</returns>
        protected override String GenerateEmbeddedCreaturesString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Int32 offset in this.Creatures.Keys)
            {
                sb.Append(StringFormat.ToStringAlignment(String.Format("Creature at offset {0:X}", offset)));
                sb.Append(this.Creatures[offset].ToString(false));
            }
         
            return sb.ToString();
        }
        #endregion
    }
}