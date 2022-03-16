using System;
using System.Collections.Generic;
using System.Text;

namespace fwv.Models
{
    public enum Operation
    {
        None,
        Create,
        Modify,
        Rename,
        Move,
        Delete
    }

    public class ModificationSummary
    {
        public string TargetFile { get; set; } = null;
        public Operation Operation { get; set; } = Operation.None;
    }
}
