using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using fev.Common;

namespace fev.Models
{
    internal class GitManager
    {
        internal GitManager()
        {
            TestRun();
        }

        /// <summary>
        /// run a git command.
        /// if you would like to run "git clone xxxxx yyyyy",
        /// you only have to put "clone xxxxx yyyyy" to the argument "gitArguments".
        /// </summary>
        /// <param name="gitArguments">git sub command and options</param>
        /// <returns>standard output and error from git.exe</returns>
        internal GitOutput RunGitCommand(string gitArguments = "")
        {
            _proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _exe,
                    Arguments = gitArguments,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            _proc.Start();

            string output = "";
            string error = "";
            while (!_proc.StandardOutput.EndOfStream)
            {
                string line = _proc.StandardOutput.ReadLine();
                output += line + "\n";
            }
            while (!_proc.StandardError.EndOfStream)
            {
                string line = _proc.StandardError.ReadLine();
                error += line + "\n";
            }

            return new GitOutput
            {
                Output = output,
                Error = error
            };
        }

        /// <summary>
        /// test run for git.exe.
        /// this function run "git" command without any subcommands and options.
        /// the standard output and error are shown on messageboxes.
        /// if an error occured, an error message is shown on a messagebox.
        /// </summary>
        internal void TestRun()
        {
            _logManager.AppendLog("begin.");
            try
            {
                GitOutput result = RunGitCommand();
                System.Windows.MessageBox.Show(result.Output);
                System.Windows.MessageBox.Show(result.Error);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("error occured !!!");
                System.Windows.MessageBox.Show(ex.Message);
            }
            _logManager.AppendLog("fin.");
        }

        internal void Checkout(string url)
        {

        }

        private Process _proc = null;
        private string _exe = "Resources\\PortableGit\\cmd\\git.exe";
        private LogManager _logManager = LogManager.GetInstance();
    }
}
