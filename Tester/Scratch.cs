using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Bardez.Projects.DirectX.Direct2D;
using Direct2D = Bardez.Projects.DirectX.Direct2D;
using ExternalPixelEnums = Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Pixels.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.External.Image.JPEG;
using Bardez.Projects.InfinityPlus1.FileFormats.Infinity.Save;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Enums;
using Bardez.Projects.InfinityPlus1.FileFormats.MediaBase.Video.Resize;
using Bardez.Projects.InfinityPlus1.Output.Visual;

namespace Bardez.Projects.InfinityPlus1.Tester
{
    /// <summary>Scratch Windows form to quickly test code</summary>
    public partial class Scratch : Form
    {
        #region Construction
        /// <summary>Default constructor</summary>
        public Scratch()
        {
            InitializeComponent();
        }
        #endregion


        /// <summary>Form Load event handler</summary>
        /// <param name="sender">Sending Object</param>
        /// <param name="e">Event parameters</param>
        private void Scratch_Load(Object sender, EventArgs e)
        {
            this.DoDirectoryStuff();
        }


        #region Copy Save files
        private void DoDirectoryStuff()
        {
            this.ListFiles();
        }

        private void CopyFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(@"N:\Test Data\Infinity Engine\Example files\Saves\PST");

            //get directory list
            DirectoryInfo[] subdirs = dir.GetDirectories();

            foreach (DirectoryInfo subDir in subdirs)
                this.MoveGamFiles(subDir);
        }

        private void ListFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(@"N:\Test Data\Infinity Engine\Example files\CBF");
            
            StringBuilder builder = new StringBuilder();

            //get directory list
            //DirectoryInfo[] subdirs = dir.GetDirectories();

            //foreach (DirectoryInfo subDir in subdirs)
                //this.ListGamFiles(subDir, builder);

            //alternate single directory approach
            this.ListGamFiles(dir, builder);

            
            //post-processing
            builder.Replace("N:", "<add key=\"Test.CBF.Path\" value=\"");
            builder.Replace("\r\n", "\" />\r\n");

            using (TextWriter writer = new StreamWriter(@"\cbf.txt"))
                writer.Write(builder.ToString());
        }

        private void ExtractFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(@"N:\Test Data\Infinity Engine\Example files\Saves\IWD2");

            //get directory list
            DirectoryInfo[] subdirs = dir.GetDirectories();

            foreach (DirectoryInfo subDir in subdirs)
                this.Extract_T_O_WILDCARD_Files(subDir);
        }

        private void MoveGamFiles(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                if (file.Extension.ToLower() == ".sav")
                    file.CopyTo(@"N:\Test Data\Infinity Engine\Example files\SAV\PST\" + file.Directory.Name + ".SAV", true);
            }
        }

        private void Extract_T_O_WILDCARD_Files(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            String prefixDir = @"N:\Test Data\Infinity Engine\Example files\";
            String prefixGameDir = @"\IWD2\";

            foreach (FileInfo file in files)
            {
                //save file
                if (file.Extension.ToLower() == ".sav")
                {
                    SaveFile save = new SaveFile();
                    using (FileStream stream = file.OpenRead())
                        save.Read(stream);

                    SaveManager manager = new SaveManager(save);

                    foreach (SavedResource resource in manager.SavedResources.Resources)
                    {
                        String resName = resource.ResourceName.Trim().ToLower();
                        if (resName.EndsWith(".toh") || resName.EndsWith(".tot"))
                        {
                            String extension = null;
                            if (resName.EndsWith(".toh"))
                                extension = "TOH";
                            else
                                extension = "TOT";

                            String newFileName = prefixDir + extension + prefixGameDir + file.Directory.Name + ".to";
                            newFileName += resName[resName.Length - 1]; //apend trailing character

                            using (MemoryStream memStream = manager.ExtractResource(resource.ResourceName))
                            using (FileStream output = File.Create(newFileName))
                                memStream.CopyTo(output);
                        }
                    }
                }
            }
        }

        private void ListGamFiles(DirectoryInfo dir, StringBuilder builder)
        {
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                if (file.Extension.ToLower() == ".cbf")
                    builder.AppendLine(file.FullName);
            }

            //get directory list
            DirectoryInfo[] subdirs = dir.GetDirectories();
            foreach (DirectoryInfo subdir in subdirs)
                this.ListGamFiles(subdir, builder);            
        }
        #endregion
    }
}