using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace fwv.Models
{
    public class ModifiedEventArgs : EventArgs
    {
        public string FullPath { get; set; }
        public string Name { get; set; }

        public ModifiedEventArgs(string directory, string name)
        {
            FullPath = $"{directory}{name}";
            Name = name;
        }
    }

    public delegate void ModifiedEventHandler(object sender, ModifiedEventArgs args);
}
