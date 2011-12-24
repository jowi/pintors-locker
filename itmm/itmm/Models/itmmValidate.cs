using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace itmm.Models
{
    public class itmmAdmin
    {
        [Required(ErrorMessage = "*Required")]
        public string LaboratoryName { get; set; }
        public string LaboratoryDesc { get; set; }
        public string LabRoom { get; set; }
    }
}