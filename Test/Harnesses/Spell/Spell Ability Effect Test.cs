using System;
using System.IO;

using Bardez.Projects.Configuration;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Effect.Effect1;
using Bardez.Projects.InfinityPlus1.Files.Infinity.Spell.Spell1;
using Bardez.Projects.InfinityPlus1.Test;
using Bardez.Projects.ReusableCode;

namespace Bardez.Projects.InfinityPlus1.Test.Harnesses.Spell
{
    /// <summary>This class tests the usable methods in the Bardez.Projects.InfinityPlus1.Files.Infinity.Common.ItmSpl.ItemSpellAbilityEffect class.</summary>
    public class SpellAbilityEffectTest : FileTesterBase
    {
        #region Fields
        /// <summary>Constant key to look up in app.config</summary>
        protected const String configKey = "Test.Spell.Spell1Path";

        /// <summary>Instance supporting data read</summary>
        protected Spell1Header Header { get; set; }

        /// <summary>Format instance to test</summary>
        protected Effect1 Effect { get; set; }
        #endregion

        #region Construction
        /// <summary>Default constructor</summary>
        public SpellAbilityEffectTest()
        {
            this.InitializeInstance();
        }
        #endregion

        /// <summary>Initializes the test class data</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="e">Specific initialization event parameters</param>
        protected override void InitializeTestData(Object sender, EventArgs e)
        {
            this.FilePaths = ConfigurationHandlerMulti.GetSettingValues(SpellAbilityEffectTest.configKey);
        }

        /// <summary>Event to raise for testing instance(s)</summary>
        /// <param name="sender">Object sending/raising the request</param>
        /// <param name="testArgs">Arguments containing the item to test (usually a file path)</param>
        protected override void TestCase(Object sender, TestEventArgs testArgs)
        {
            String effect = "No abilities";
            using (FileStream stream = new FileStream(testArgs.Path, FileMode.Open, FileAccess.Read))
            {
                this.Header = new Spell1Header();
                this.Header.Read(stream);

                if (this.Header.CountAbilities > 0)
                {
                    ReusableIO.SeekIfAble(stream, this.Header.OffsetAbilityEffects);
                    this.Effect = new Effect1();
                    this.Effect.Read(stream);
                    effect = this.Effect.ToString();
                }
            }

            this.DoPostMessage(new MessageEventArgs(effect, "Output", testArgs.Path));

            if (this.Header.CountAbilities > 0)
                //not sure how I want to handle this...
                using (FileStream dest = new FileStream(testArgs.Path + ".abilityeffect.rewrite", FileMode.Create, FileAccess.Write))
                    this.TestWrite(dest);
        }

        /// <summary>Writes the data structure back out to a destination stream</summary>
        /// <param name="destination">Stream to write output to</param>
        protected virtual void TestWrite(Stream destination)
        {
            this.Effect.Write(destination);
        }
    }
}