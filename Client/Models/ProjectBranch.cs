using System;
namespace AppClient.Models
{
    public class ProjectBranch
    {

        public Guid ProjectId { get; set; }
        public Guid BranchId { get; set; }



        public string ServiceProject { get; set; }
        public string Module { get; set; }

        public virtual Project Project { get; set; }
        public virtual Branch Branch { get; set; }




        public ProjectBranch()
        {
        }
    }
}
