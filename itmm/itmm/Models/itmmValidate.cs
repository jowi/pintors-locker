using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using itmm.Models;

namespace itmm.Models
{
    public class itmmAdminHead
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name="Firstname")]
        public string fname { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Lastname")]
        public string lname { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Contact Number")]
        public string cnum { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string eadd { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Section")]
        //public IEnumerable<Laboratory> section { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Username")]
        public string uname { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Confrim Password")]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string c_password { get; set; }

    }
    public class itmmAdminStaff
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Firstname")]
        public string fname { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Lastname")]
        public string lname { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Id Number")]
        public string cnum { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Course&Year")]
        public string course { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string eadd { get; set; }


        [Required(ErrorMessage = "Required")]
        [Display(Name = "Username")]
        public string uname { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Confrim Password")]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string c_password { get; set; }

    }
    public class itmmAdminEquipment
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Make")]
        public string make { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Location")]
        public string location { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Serial Number")]
        public string serial { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Barcode")]
        public string barcode { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Status")]
        public string status { get; set; }

 
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Life (e.g. 4 years)")]
        public string life { get; set; }
    }
    public class itmmNotification 
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "What")]
        public string what { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "When")]
        public string whin { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Where")]
        public string whire { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Who")]
        public string who { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "time")]
        public string time { get; set; }
    }
}