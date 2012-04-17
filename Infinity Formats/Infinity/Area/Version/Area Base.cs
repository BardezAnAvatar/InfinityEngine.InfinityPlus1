using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Bardez.Projects.InfinityPlus1.FileFormats.Basic;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Component;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Base;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Common.ItmSpl;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Area.Version
{
    /// <summary>Base area class</summary>
    public abstract class AreaBase : IInfinityFormat
    {
        #region Fields
        /* header */

        /// <summary>Collection of actors in this area</summary>
        public List<Actor> Actors { get; set; }

        /// <summary>Collection of regions in this area</summary>
        public List<Region> Regions { get; set; }

        /* Spawn points */

        /// <summary>Collection of entrances within this area</summary>
        public List<Entrance> Entrances { get; set; }

        /// <summary>Collection of containers in this area</summary>
        public List<Container> Containers { get; set; }

        /// <summary>Collection of items in this area</summary>
        public List<ItemInstance> Items { get; set; }

        /// <summary>Collection of vertex Points for polygons in this area</summary>
        public List<Point> Vertices { get; set; }

        /// <summary>Collection of ambient sounds in this area</summary>
        public List<Ambient> AmbientSounds { get; set; }

        /// <summary>Collection of variables in this area's scope</summary>
        public List<Variable> Variables { get; set; }

        /// <summary>Bitmat for this area's explored bitmask</summary>
        public ExploredBitmask ExploredMask { get; set; }

        /// <summary>Collection of doors in this area</summary>
        public List<Door> Doors { get; set; }

        /// <summary>Collection of animations in this area</summary>
        public List<Animation> Animations { get; set; }

        /* Map Notes */

        /// <summary>Collection of set projectile traps in this area</summary>
        public List<ProjectileTrap> ProjectileTraps { get; set; }

        /// <summary>Object wrapping the song references for this area</summary>
        public Song Songs { get; set; }

        /// <summary>Object wrapping the interruptions of rest in this area</summary>
        public RestInterruption SleepInterruptions { get; set; }

        /* Creatures */
        #endregion


        #region Construction
        /// <summary>Instantiates reference types</summary>
        public virtual void Initialize()
        {
            this.Actors = new List<Actor>();
            this.Regions = new List<Region>();
            this.Entrances = new List<Entrance>();
            this.Containers = new List<Container>();
            this.Items = new List<ItemInstance>();
            this.Vertices = new List<Point>();
            this.AmbientSounds = new List<Ambient>();
            this.Variables = new List<Variable>();
            this.ExploredMask = new ExploredBitmask();
            this.Doors = new List<Door>();
            this.Animations = new List<Animation>();
            this.ProjectileTraps = new List<ProjectileTrap>();
            this.Songs = new Song();
            this.SleepInterruptions = new RestInterruption();
        }
        #endregion


        #region IO method implemetations
        #region Read
        /// <summary>This public method reads file format from the input stream. Reads the whole structure.</summary>
        /// <param name="input">Input stream to read from</param>
        public void Read(Stream input)
        {
            this.ReadBody(input);
        }

        /// <summary>This public method reads file format data structure from the input stream.</summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="fullRead">Boolean indicating whether to read the full stream or just everything after the identifying signature (and possibly version)</param>
        public abstract void Read(Stream input, Boolean fullRead);

        /// <summary>This public method reads file format data structure from the input stream, after the signature has already been read.</summary>
        /// <param name="input">Stream to read from</param>
        public abstract void ReadBody(Stream input);

        /// <summary>Reads the header from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        protected abstract void ReadHeader(Stream input);

        /// <summary>Reads actors from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of actors to read</param>
        protected void ReadActors(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Actor actor = new Actor();
                    actor.Read(input);
                    this.Actors.Add(actor);
                }
            }
        }

        /// <summary>Reads regions from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of regions to read</param>
        protected void ReadRegions(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Region region = new Region();
                    region.Read(input);
                    this.Regions.Add(region);
                }
            }
        }

        /// <summary>Reads spawn points from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of spawn points to read</param>
        protected abstract void ReadSpawnPoints(Stream input, Int32 offset, Int32 count);

        /// <summary>Reads entrances from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of entrances to read</param>
        protected void ReadEntrances(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Entrance entrance = new Entrance();
                    entrance.Read(input);
                    this.Entrances.Add(entrance);
                }
            }
        }

        /// <summary>Reads containers from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of containers to read</param>
        protected void ReadContainers(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Container container = new Container();
                    container.Read(input);
                    this.Containers.Add(container);
                }
            }
        }

        /// <summary>Reads items from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of items to read</param>
        protected void ReadItems(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    ItemInstance item = new ItemInstance();
                    item.Read(input);
                    this.Items.Add(item);
                }
            }
        }

        /// <summary>Reads vertices from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of vertices to read</param>
        protected void ReadVertices(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Point vertex = new Point();

                    Byte[] buffer = ReusableIO.BinaryRead(input, 4);
                    vertex.X = ReusableIO.ReadUInt16FromArray(buffer, 0);
                    vertex.Y = ReusableIO.ReadUInt16FromArray(buffer, 2);

                    this.Vertices.Add(vertex);
                }
            }
        }

        /// <summary>Reads ambients from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of ambient sounds to read</param>
        protected void ReadAmbientSounds(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Ambient ambient = new Ambient();
                    ambient.Read(input);
                    this.AmbientSounds.Add(ambient);
                }
            }
        }

        /// <summary>Reads variables from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of variables to read</param>
        protected void ReadVariables(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Variable variable = new Variable();
                    variable.Read(input);
                    this.Variables.Add(variable);
                }
            }
        }

        /// <summary>Reads explored mask from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        protected void ReadExploredMask(Stream input, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);
                this.ExploredMask.Read(input);
            }
        }
        
        /// <summary>Reads doors from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of doors to read</param>
        protected void ReadDoors(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Door door = new Door();
                    door.Read(input);
                    this.Doors.Add(door);
                }
            }
        }

        /// <summary>Reads animations from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of animations to read</param>
        protected void ReadAnimations(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    Animation animations = new Animation();
                    animations.Read(input);
                    this.Animations.Add(animations);
                }
            }
        }

        /// <summary>Reads map notes from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of actors to read</param>
        protected abstract void ReadMapNotes(Stream input, Int32 offset, Int32 count);

        /// <summary>Reads projectile traps from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        /// <param name="count">Count of projectile traps to read</param>
        protected void ReadProjectileTraps(Stream input, Int32 offset, Int32 count)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);

                for (Int32 counter = 0; counter < count; ++counter)
                {
                    ProjectileTrap trap = new ProjectileTrap();
                    trap.Read(input);
                    this.ProjectileTraps.Add(trap);
                }
            }
        }

        /// <summary>Reads songs from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        protected void ReadSongs(Stream input, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);
                this.Songs.Read(input);
            }
        }

        /// <summary>Reads rest interruptions from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset to seek to before read</param>
        protected void ReadSleepInterruptions(Stream input, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(input, offset);
                this.SleepInterruptions.Read(input);
            }
        }

        /// <summary>Reads embedded creatures from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        protected abstract void ReadEmbeddedCreatures(Stream input);
        #endregion


        #region Write
        /// <summary>This public method writes the file format to the output stream.</summary>
        /// <param name="output">Stream to write to</param>
        public abstract void Write(Stream output);

        /// <summary>Writes actors to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteActors(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Actor actor in this.Actors)
                    actor.Write(output);
            }
        }

        /// <summary>Writes regions to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteRegions(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Region region in this.Regions)
                    region.Write(output);
            }
        }

        /// <summary>Writes spawn points to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected abstract void WriteSpawnPoints(Stream output, Int32 offset);

        /// <summary>Writes entrances to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteEntrances(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Entrance entrance in this.Entrances)
                    entrance.Write(output);
            }
        }

        /// <summary>Writes containers to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteContainers(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Container container in this.Containers)
                    container.Write(output);
            }
        }

        /// <summary>Writes items to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteItems(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (ItemInstance item in this.Items)
                    item.Write(output);
            }
        }

        /// <summary>Writes vertices to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteVertices(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Point vertex in this.Vertices)
                {
                    ReusableIO.WriteUInt16ToStream(vertex.X, output);
                    ReusableIO.WriteUInt16ToStream(vertex.Y, output);
                }
            }
        }

        /// <summary>Writes ambient sounds to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteAmbientSounds(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Ambient ambient in this.AmbientSounds)
                    ambient.Write(output);
            }
        }

        /// <summary>Writes variables to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteVariables(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Variable variable in this.Variables)
                    variable.Write(output);
            }
        }

        /// <summary>Writes the explored area bitmask to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteExploredMask(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);
                this.ExploredMask.Write(output);
            }
        }

        /// <summary>Writes doors to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteDoors(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Door door in this.Doors)
                    door.Write(output);
            }
        }

        /// <summary>Writes animations to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteAnimations(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (Animation animation in this.Animations)
                    animation.Write(output);
            }
        }

        /// <summary>Writes map notes to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected abstract void WriteMapNotes(Stream output, Int32 offset);

        /// <summary>Writes projectile traps to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteProjectileTraps(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);

                foreach (ProjectileTrap trap in this.ProjectileTraps)
                    trap.Write(output);
            }
        }

        /// <summary>Writes songs to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteSongs(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);
                this.Songs.Write(output);
            }
        }

        /// <summary>Writes rest interruptions to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        /// <param name="offset">Offset to start writing in the Stream at</param>
        protected void WriteSleepInterruptions(Stream output, Int32 offset)
        {
            if (offset > 0)
            {
                ReusableIO.SeekIfAble(output, offset);
                this.SleepInterruptions.Write(output);
            }
        }

        /// <summary>Writes embedded creatures to the output stream</summary>
        /// <param name="output">Stream to write to</param>
        protected abstract void WriteEmbeddedCreatures(Stream output);
        #endregion
        #endregion


        #region Data integrity
        /// <summary>Maintains mimimal data integrity by not lying to the output data file.</summary>
        /// <remarks>GAM files many data pointers to balance. Copying from the IWD2 creature model.</remarks>
        protected void MaintainMinimalDataIntegrity()
        {
            List<Tuple<Int64, Int64>> offsets = this.CollectOffsets();
            if (this.OffsetsOverlap(offsets))
                this.ResetOffsets();
        }

        /// <summary>Collects a set of offsets and their lengths to be examined</summary>
        /// <returns>A List of Tuples (Offset and length)</returns>
        protected abstract List<Tuple<Int64, Int64>> CollectOffsets();

        /// <summary>Traverses a collection of Tuples (offset and length) and determines if any overlap</summary>
        /// <param name="offsets">Collection of offset tuples</param>
        /// <returns>True if any overlap; false if not</returns>
        protected Boolean OffsetsOverlap(List<Tuple<Int64, Int64>> offsets)
        {
            Boolean overlaps = false;

            for (Int32 i = 0; i < offsets.Count; ++i)
                for (Int32 j = 0; j < offsets.Count; ++j)
                {
                    if (i == j)
                        continue;

                    if (IntExtension.Between(offsets[i].Item1, offsets[i].Item2, offsets[j].Item1, offsets[j].Item1 + offsets[j].Item2))
                    {
                        overlaps = true;
                        goto ExitPoint; //short-circuit
                    }
                }

        //short-circuit destination
        ExitPoint:

            return overlaps;
        }

        /// <summary>Resets the offsets within the file stream</summary>
        protected abstract void ResetOffsets();
        #endregion


        #region ToString methods
        /// <summary>This method overrides the default ToString() method, printing the member data line by line</summary>
        /// <returns>A string containing the values and descriptions of all values in this class</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(this.GenerateTypeDescriptionString());
            builder.AppendLine(this.GenerateHeaderString());
            builder.AppendLine(this.GenerateActorString());
            builder.AppendLine(this.GenerateRegionString());
            builder.AppendLine(this.GenerateSpawnPointString());
            builder.AppendLine(this.GenerateEntranceString());
            builder.AppendLine(this.GenerateContainersString());
            builder.AppendLine(this.GenerateItemsString());
            builder.AppendLine(this.GenerateVerticesString());
            builder.AppendLine(this.GenerateAmbientsString());
            builder.AppendLine(this.GenerateVariableString());
            builder.AppendLine(this.GenerateExploredBitmaskString());
            builder.AppendLine(this.GenerateDoorsString());
            builder.AppendLine(this.GenerateAnimationsString());
            builder.AppendLine(this.GenerateMapNotesString());
            builder.AppendLine(this.GenerateProjectileTrapsString());
            builder.AppendLine(this.GenerateSongsString());
            builder.AppendLine(this.GenerateRestInterruptionString());
            builder.AppendLine(this.GenerateEmbeddedCreaturesString());

            return builder.ToString();
        }

        /// <summary>Generates a description String of this Type</summary>
        /// <returns>A description String of this Type</returns>
        protected abstract String GenerateTypeDescriptionString();

        /// <summary>Generates a description String of the header</summary>
        /// <returns>A description String of the header</returns>
        protected abstract String GenerateHeaderString();

        /// <summary>Generates a human-readable console output describing the actors in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateActorString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Actors.Count; ++i)
                sb.Append(this.Actors[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the regions in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateRegionString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Regions.Count; ++i)
                sb.Append(this.Regions[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the spawn points in this area</summary>
        /// <returns>A multi-line String</returns>
        protected abstract String GenerateSpawnPointString();

        /// <summary>Generates a human-readable console output describing the entrances to this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateEntranceString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Entrances.Count; ++i)
                sb.Append(this.Entrances[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the containers in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateContainersString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Containers.Count; ++i)
                sb.Append(this.Containers[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the items in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateItemsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Items.Count; ++i)
                sb.Append(this.Items[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the vertices in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateVerticesString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Vertices.Count; ++i)
                sb.Append(this.Vertices[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the ambient sounds in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateAmbientsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.AmbientSounds.Count; ++i)
                sb.Append(this.AmbientSounds[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the variables in local scope of this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateVariableString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Variables.Count; ++i)
                sb.Append(this.Variables[i].ToString());

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the explored bitmask of this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateExploredBitmaskString()
        {
            return this.ExploredMask.ToString();
        }

        /// <summary>Generates a human-readable console output describing the doors in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateDoorsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Doors.Count; ++i)
                sb.Append(this.Doors[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the animations in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateAnimationsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.Animations.Count; ++i)
                sb.Append(this.Animations[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the map notes in this area</summary>
        /// <returns>A multi-line String</returns>
        protected abstract String GenerateMapNotesString();

        /// <summary>Generates a human-readable console output describing the projectile traps in this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateProjectileTrapsString()
        {
            StringBuilder sb = new StringBuilder();

            for (Int32 i = 0; i < this.ProjectileTraps.Count; ++i)
                sb.Append(this.ProjectileTraps[i].ToString(i + 1));

            return sb.ToString();
        }

        /// <summary>Generates a human-readable console output describing the songs of this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateSongsString()
        {
            return this.Songs.ToString();
        }

        /// <summary>Generates a human-readable console output describing the rest interruptions of this area</summary>
        /// <returns>A multi-line String</returns>
        protected String GenerateRestInterruptionString()
        {
            return this.SleepInterruptions.ToString();
        }

        /// <summary>Generates a human-readable console output describing the embedded creatures in this area</summary>
        /// <returns>A multi-line String</returns>
        protected abstract String GenerateEmbeddedCreaturesString();
        #endregion
    }
}