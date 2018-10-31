using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB_And_Selenium.Persistence
{
    class GoogleBusinessContext : DbContext
    {
        public virtual DbSet<Models.ProjectData> Projects { get; set; }
    }
}
