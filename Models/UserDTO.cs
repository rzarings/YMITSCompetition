using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMITSDeployedWebsite.Models
{
    public class UserDTO
    {
        [Required(ErrorMessage = "An email address is required")]
        [StringLength(160)]
        [DisplayName("Genre")]
        public String Email { get; set; }

        //id of the Azure subscription
        public String SubscriptionId { get; set; }
        //are you a team leader
        public bool TeamLead { get; set; }

        [Required(ErrorMessage = "Please put your team name in")]
        [StringLength(160)]
        //the team name
        public string TeamName { get; set; }
        //subscription date of the user
       
    }

    public class UserViewModel : UserDTO
    {
        public List<UserDTO> TeamMembers { get; set; }

        public bool Lauched = false;

    }


    }

