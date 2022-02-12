using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace fev.Models
{
    internal class GitManager
    {
        public GitManager()
        {
            try
            {
                TestRun();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("error occured !!!");
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void TestRun()
        {
            _proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _exe,
                    Arguments = "",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            _proc.Start();
            string output = "";
            string error = "";
            while(!_proc.StandardOutput.EndOfStream)
            {
                string line = _proc.StandardOutput.ReadLine();
                output += line + "\n";
            }
            while(!_proc.StandardError.EndOfStream)
            {
                string line = _proc.StandardError.ReadLine();
                error += line + "\n";
            }
            System.Windows.MessageBox.Show(output);
            System.Windows.MessageBox.Show(error);
        }

        private Process _proc = null;
        private string _exe = "Resources\\PortableGit\\cmd\\git.exe";
    }
}
