using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using fwv.Common;
using fwv.Exceptions;

namespace fwv.Models
{
    /// <summary>
    /// singleton class to wrap git.exe.
    /// to get an instance, call GetInstance() method.
    /// </summary>
    internal class GitManager
    {
        /// <summary>
        /// run a git command.
        /// if you would like to run "git clone xxxxx yyyyy",
        /// you only have to call RunGitCommand("clone xxxxx yyyyy").
        /// </summary>
        /// <param name="gitArguments">git sub command and options</param>
        /// <returns>standard output and error from git.exe</returns>
        internal GitResult RunGitCommand(string gitArguments = "")
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

            _logManager.AppendLog(output);
            _logManager.AppendErrorLog(error);

            return new GitResult { StandardOutput = output, StandardError = error };
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
                GitResult result = RunGitCommand();
                System.Windows.MessageBox.Show(result.StandardOutput);
                System.Windows.MessageBox.Show(result.StandardError);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("error occured !!!");
                System.Windows.MessageBox.Show(ex.Message);
            }
            _logManager.AppendLog("fin.");
        }

        internal GitResult Clone(string url, string directoryPath)
        {
            return RunGitCommand($"clone {url} {directoryPath}");
        }

        #region Constructor

        /// <summary>
        /// static method for singleton pattern.
        /// </summary>
        /// <returns>GitManager instance</returns>
        internal static GitManager GetInstance()
        {
            return _gitManager;
        }

        private GitManager()
        {

        }

        #endregion

        private static GitManager _gitManager = new GitManager();
        private Process _proc = null;
        private string _exe = "git";
        private LogManager _logManager = LogManager.GetInstance();
    }
}
