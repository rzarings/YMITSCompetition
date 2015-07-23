using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMITSDeployedWebsite.Models.JsonItems;

namespace YMITSDeployedWebsite.Models
{
    static class State
    {

        //status of the page, 0 is login, 1 is logged, 2 is submitted
        public static PageStatus status = new PageStatus();

    }
}
