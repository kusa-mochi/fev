using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

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
        #region Properties

        private bool _CanRunGitCommand = true;
        internal bool CanRunGitCommand
        {
            get
            {
                return _CanRunGitCommand;
            }
            set
            {
                _logManager.AppendLog($"set CanRunGitCommand {value}");
                _CanRunGitCommand = value;
            }
        }

        internal string WorkingDirectory { get; set; } = null;

        #endregion

        #region Internal Methods

        internal void EnqueueCommand(GitCommandItemBase command)
        {
            _gitCommandQueue.Enqueue(command);
            _logManager.AppendLog($"a git \"{command.Command.ToString()}\" command was enqueued.");
        }


        internal CommandOutput RunWindowsCommand(string command)
        {
            return RunCommand("cmd.exe", command);
        }

        /// <summary>
        /// run a git command.
        /// if you would like to run "git clone xxxxx yyyyy",
        /// you only have to call RunGitCommand("clone xxxxx yyyyy").
        /// </summary>
        /// <param name="gitArguments">git sub command and options</param>
        /// <returns>standard output and error from git.exe</returns>
        internal CommandOutput RunGitCommand(string gitArguments = "")
        {
            return RunCommand(_exe, gitArguments);
        }

        /// <summary>
        /// test run for git.exe.
        /// this function run "git" command without any subcommands and options.
        /// the standard output and error are shown on messageboxes.
        /// if an error occured, an error message is shown on a messagebox.
        /// </summary>
        internal CommandOutput TestRun()
        {
            if (!CanRunGitCommand)
            {
                _logManager.AppendErrorLog("GitManager instance is busy now.");
                return new CommandOutput { StandardOutput = "", StandardError = "" };
            }

            _logManager.AppendLog("begin.");
            CommandOutput result;
            try
            {
                result = RunGitCommand();
                System.Windows.MessageBox.Show(result.StandardOutput);
                System.Windows.MessageBox.Show(result.StandardError);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("error occured !!!");
                System.Windows.MessageBox.Show(ex.Message);
                result = new CommandOutput { StandardError = ex.Message, StandardOutput = "" };
            }
            _logManager.AppendLog("fin.");
            return result;
        }

        internal CommandOutput Clone(string url, string directoryPath)
        {
            return RunGitCommand($"clone {url} \"{directoryPath}\"");
        }

        internal CommandOutput Add(string filter = "*")
        {
            return RunGitCommand($"add {filter}");
        }

        internal CommandOutput Commit(string message = "files were modified.")
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "files were modified.";
            }
            return RunGitCommand($"commit -m \"{message}\"");
        }

        internal CommandOutput Push(string branch = "main")
        {
            return RunGitCommand($"push origin {branch}");
        }

        internal CommandOutput Init(bool isBare = false, string initialBranch = "main")
        {
            string args = isBare ? $" --bare --shared --initial-branch={initialBranch}" : "";
            return RunGitCommand($"init{args}");
        }

        #endregion

        #region Private Methods

        private CommandOutput RunCommand(string fileName, string args)
        {
            _logManager.AppendLog($"runnig command.. \"{fileName} {args}\"");
            if (!CanRunGitCommand)
            {
                string logMessage = $"git is busy now. command \"{fileName} {args}\" was not executed.";
                _logManager.AppendErrorLog(logMessage);
                return new CommandOutput { StandardOutput = "", StandardError = logMessage };
            }

            CanRunGitCommand = false;
            if (WorkingDirectory == null)
            {
                CanRunGitCommand = true;
                _logManager.AppendErrorLog("running command failed because the WorkingDirectory property is not set.");
                throw new InvalidOperationException("WorkingDirectory property must be set before running commands.");
            }

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = args,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = WorkingDirectory
                }
            };

            proc.EnableRaisingEvents = true;
            proc.Exited += RunNextCommand;

            bool ret = proc.Start();
            string output = "";
            string error = "";
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                output += line + "\n";
            }
            while (!proc.StandardError.EndOfStream)
            {
                string line = proc.StandardError.ReadLine();
                error += line + "\n";
            }

            _logManager.AppendLog(output);
            _logManager.AppendErrorLog(error);

            return new CommandOutput { StandardOutput = output, StandardError = error };
        }

        private void RunNextCommand(object sender, EventArgs e)
        {
            _logManager.AppendLog("running next command..");
            CanRunGitCommand = true;
            RunCommandQueue(sender, null);
        }

        private void RunCommandQueue(object sender, ElapsedEventArgs e)
        {
            if (_gitCommandQueue.Count == 0)
            {
                _logManager.AppendLog("there is no command to execute in the queue.");
                return;
            }
            if (!CanRunGitCommand)
            {
                _logManager.AppendErrorLog("the git is busy now. Dequeue() was not called.");
                return;
            }

            GitCommandItemBase queueItem = _gitCommandQueue.Dequeue();
            _logManager.AppendLog($"a git \"{queueItem.Command.ToString()}\" command was dequeued.");
            WorkingDirectory = queueItem.WorkingDirectory;
            switch (queueItem.Command)
            {
                case GitCommand.Init:
                    {
                        GitInitCommandItem item = queueItem as GitInitCommandItem;
                        Init(item.IsBare);
                    }
                    break;
                case GitCommand.Clone:
                    {
                        GitCloneCommandItem item = queueItem as GitCloneCommandItem;
                        Clone(item.RemoteUrl, item.WorkingDirectoryPath);
                    }
                    break;
                case GitCommand.Add:
                    {
                        GitAddCommandItem item = queueItem as GitAddCommandItem;
                        Add();
                    }
                    break;
                case GitCommand.Commit:
                    {
                        GitCommitCommandItem item = queueItem as GitCommitCommandItem;
                        Commit();
                    }
                    break;
                case GitCommand.Push:
                    {
                        GitPushCommandItem item = queueItem as GitPushCommandItem;
                        Push();
                    }
                    break;
                case GitCommand.Pull:
                    throw new NotImplementedException();
                default:
                    {
                        string errorMessage = "invalid git command in queue.";
                        _logManager.AppendErrorLog(errorMessage);
                        throw new InvalidOperationException(errorMessage);
                    }
            }
        }

        #endregion

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
            // start a timer to check git command queue at the intervals.
            Timer timer = new Timer
            {
                Interval = Properties.Settings.Default.WatchInterval,
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += RunCommandQueue;
            timer.Start();
        }

        #endregion

        #region Fields

        private static GitManager _gitManager = new GitManager();
        private string _exe = "git";
        private LogManager _logManager = LogManager.GetInstance();
        private Queue<GitCommandItemBase> _gitCommandQueue = new Queue<GitCommandItemBase>();

        #endregion
    }
}
