using System;
using System.Collections.Generic;
using System.Text;

using fwv.Common;

namespace fwv.Common
{
    public class GitCommitCommanItem : GitCommandItemBase
    {
        public string Message = null;

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public GitCommitCommanItem(string workingDirectory, string message = null)
        {
            Command = GitCommand.Commit;
            WorkingDirectory = workingDirectory;
            Message = message;
        }
    }
}
