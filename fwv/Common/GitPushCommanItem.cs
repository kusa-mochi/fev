using System;
using System.Collections.Generic;
using System.Text;

using fwv.Common;

namespace fwv.Common
{
    public class GitCPushCommandItem : GitCommandItemBase
    {
        public string Branch = "master";

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public GitCPushCommandItem(string workingDirectory, string branch = "master")
        {
            Command = GitCommand.Push;
            WorkingDirectory = workingDirectory;
            Branch = branch;
        }
    }
}
