using System;
using System.Collections.Generic;
// clase para master detail


namespace AppClient.Models
{
    public class Project
    {

        public Guid ProjectId { get; set; }
        public String ProjectName { get; set; }
        public String ProjectType { get; set; }


        public virtual List<ProjectBranch> ProjectBranch { get; set; }



        public Project()
        {
        }
    }
}
