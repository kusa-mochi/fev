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
            TestRun();
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
                    UseShellExecute = false
                }
            };
            _proc.Start();
            while(!_proc.StandardOutput.EndOfStream)
            {
                string line = _proc.StandardOutput.ReadLine();
                Trace.WriteLine(line);
            }
        }

        private Process _proc = null;
        private string _exe = "git.exe";
    }
}
