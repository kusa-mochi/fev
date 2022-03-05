using System;
using System.Collections.Generic;
using System.Text;

using fwv.Common;

namespace fwv.Common
{
    public class GitCPushCommandItem : GitCommandItemBase
    {
        public string Branch = "main";

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public GitCPushCommandItem(string workingDirectory, string branch = "main")
        {
            Command = GitCommand.Push;
            WorkingDirectory = workingDirectory;
            Branch = branch;
        }
    }
}
