using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace fwv.Models
{
    public class FileWatcher : IDisposable
    {
        public event ModifiedEventHandler Modified;

        public FileWatcher(string directoryPath)
        {
            _watcher = new FileSystemWatcher(directoryPath);
            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;
        }

        ~FileWatcher()
        {
            Dispose();
        }

        private void OnModified(object sender, string directory, string name)
        {
            Modified.Invoke(sender, new ModifiedEventArgs(directory, name));
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
            _watcher?.Dispose();
        }

        private FileSystemWatcher _watcher = null;
    }
}
