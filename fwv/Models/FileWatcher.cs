using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using fwv.Common;

namespace fwv.Models
{
    public class FileWatcher : IDisposable
    {
        public event ModifiedEventHandler Modified;

        public FileWatcher()
        {
        }

        ~FileWatcher()
        {
            Dispose();
        }

        public void AddDirectory(string hash, string directoryPath)
        {
            if (string.IsNullOrEmpty(hash)) return;
            if (string.IsNullOrEmpty(directoryPath)) return;

            IdentifiedFileSystemWatcher watcher = new IdentifiedFileSystemWatcher(directoryPath);
            watcher.Hash = hash;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            _watchers.Add(hash, watcher);
        }

        public void RemoveDirectory(string hash)
        {
            if (string.IsNullOrEmpty(hash)) return;
            if (_watchers.Count == 0)
            {
                _logManager.AppendErrorLog("removing a directory watcher failed because there is no watcher.");
                _logManager.AppendLog($"hash: {hash}");
                return;
            }

            bool getResult = _watchers.TryGetValue(hash, out IdentifiedFileSystemWatcher watcher);
            if (getResult) watcher?.Dispose();

            bool removeResult = _watchers.Remove(hash);
            if (removeResult)
            {
                _logManager.AppendLog("a directory watcher was removed.");
                _logManager.AppendLog($"hash: {hash}");
            }
            else
            {
                _logManager.AppendErrorLog("removing a directory watcher failed.");
                _logManager.AppendErrorLog($"hash: {hash}");
            }
        }

        private void OnModified(object sender, string directory, string name)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }

            IdentifiedFileSystemWatcher watcher = sender as IdentifiedFileSystemWatcher;
            Modified.Invoke(sender, new ModifiedEventArgs(directory, name, watcher.Hash));

            // TODO: debugging code.
            System.Windows.MessageBox.Show("file is modified !!!");
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            OnModified(sender, Path.GetDirectoryName(e.FullPath), e.Name);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            OnModified(sender, Path.GetDirectoryName(e.FullPath), e.Name);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            OnModified(sender, Path.GetDirectoryName(e.FullPath), e.Name);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            OnModified(sender, Path.GetDirectoryName(e.FullPath), e.Name);
        }

        public void Dispose()
        {
            foreach (KeyValuePair<string, IdentifiedFileSystemWatcher> pair in _watchers)
            {
                IdentifiedFileSystemWatcher watcher = pair.Value;
                watcher?.Dispose();
            }
        }

        private Dictionary<string, IdentifiedFileSystemWatcher> _watchers = new Dictionary<string, IdentifiedFileSystemWatcher>();
        private LogManager _logManager = LogManager.GetInstance();
    }
}
