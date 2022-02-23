using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace fwv.Models
{
    public class RepositoryListItem
    {
        public string Hash
        {
            get
            {
                MD5 alg = MD5.Create();
                string inputString = $"{RepositoryUrl}{LocalDirectoryPath}";
                byte[] rawHash = alg.ComputeHash(Encoding.ASCII.GetBytes(inputString));
                return Convert.ToBase64String(rawHash);
            }
        }
        public string RepositoryUrl { get; set; }
        public string LocalDirectoryPath { get; set; }
    }
}
