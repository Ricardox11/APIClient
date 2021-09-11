using System;
using System.Collections.Generic;


// clase para master detail 

namespace AppClient.Models
{
    public class Branch
    {

        public Guid BranchId { get; set; }
        public String  BranchName { get; set; }
        public String BranchCity { get; set; }


        public virtual List<ProjectBranch> ProjectBranch { get; set; }


        public Branch()
        {
        }
    }
}
