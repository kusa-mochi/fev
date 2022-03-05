using System;
using System.Collections.Generic;
using System.Text;

using fwv.Common;

namespace fwv.Common
{
    public class GitAddCommanItem : GitCommandItemBase
    {
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public GitAddCommanItem(string workingDirectory)
        {
            Command = GitCommand.Add;
            WorkingDirectory = workingDirectory;
        }
    }
}
