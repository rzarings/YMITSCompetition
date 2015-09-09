using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMITSDeployedWebsite.Models.JsonItems
{
    class UsersInTeam
    {
        public int NumberUsersInTeam
        {
            get;
            set;
        }

        public int NumberOfUsersValidated
        {
            get;
            set;
        }

        public bool PersonVerified
        {
            get;
            set;
        }

        public bool TeamLead
        {
            get;
            set;
        }

        public string TeamName
        {
            get;
            set;
        }
    }
}
