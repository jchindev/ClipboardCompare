using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace ClipboardCompare.Core
{
    public class ComparisonManager
    {
        private readonly string _comparisonToolPath = ConfigurationManager.AppSettings["CompareToolPath"];
        private readonly string _comparisonToolArguments = ConfigurationManager.AppSettings["CompareToolArgs"];

        private string _clip1Text;
        private string _clip2Text;

        public string Clip1Text
        {
            get { return _clip1Text; }
            set { _clip1Text = value; }
        }

        public string Clip2Text
        {
            get { return _clip2Text; }
            set { _clip2Text = value; }
        }

        public string[] CreateTempFiles()
        {
            string workDirPath = GetWorkingDirectoryPath();

            DirectoryInfo di = new DirectoryInfo(workDirPath);
            if (!di.Exists)
            {
                di.Create();
            }

            var selectionFile = workDirPath + Path.DirectorySeparatorChar + "clip_" + Guid.NewGuid() + ".txt";
            File.WriteAllText(selectionFile, _clip1Text);

            var clipboardFile = workDirPath + Path.DirectorySeparatorChar + "clip_" + Guid.NewGuid() + ".txt";
            File.WriteAllText(clipboardFile, _clip2Text);

            return new[] { selectionFile, clipboardFile };
        }

        public void ExecuteCleanup()
        {
            string workDirPath = GetWorkingDirectoryPath();

            DirectoryInfo di = new DirectoryInfo(workDirPath);
            if (di.Exists)
            {
                var fileInfos = di.GetFiles("clip_*").ToList();
                fileInfos.ForEach(fi =>
                {
                    fi.Delete();
                });
            }
        }

        private string GetWorkingDirectoryPath()
        {
            string workDirPath = "../../Temp";
            return workDirPath;
        }

        public void OpenComparisonProcess()
        {
            try
            {
                ExecuteCleanup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            string[] tempFiles = null;
            try
            {
                tempFiles = CreateTempFiles();

                System.Diagnostics.Process proc = new System.Diagnostics.Process
                {
                    EnableRaisingEvents = false,
                    StartInfo =
                    {
                        FileName = _comparisonToolPath,
                        Arguments = $"{_comparisonToolArguments} {tempFiles[0]} {tempFiles[1]}"
                    }
                };

                if (File.Exists(tempFiles[0]) && File.Exists(tempFiles[1]) ||
                    Directory.Exists(tempFiles[0]) && Directory.Exists(tempFiles[1]))
                {
                    proc.Start();
                }
                else
                {
                    MessageBox.Show("Could not find files to compare!");
                }             
            }
            catch (Exception e)
            {
                if (tempFiles != null)
                {
                    File.Delete(tempFiles[0]);
                    File.Delete(tempFiles[1]);
                }

                MessageBox.Show(e.Message);
            }
        }
    }
}