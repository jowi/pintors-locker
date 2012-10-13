using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using itmm.Models;

namespace itmm.Models
{
    public  class itmmAdminRoom
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid Room Name")]
        [Remote("IsRoomNameTaken", "AdminBold")]
        [Display(Name="Room")]
        public string room { get; set; }
        public int roomid { get; set; }
    }

    public class itmmLaboratory
    {
        public int labid { get; set; }

        [Required(ErrorMessage = "Required")]
        [Remote("IsLabNameTaken", "AdminBold")] // there is problem during editing with this
        [Display(Name = "Laboratory Name")]
        public string labname { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Laboratory Description")]
        public string labdesc { get; set; }
        
        [Required(ErrorMessage = "Required")]
        //[StringLength(50, MinimumLength = 7, ErrorMessage="Must be at least 7 digits")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter digits.")]
        [Display(Name = "Laboratory Contact")]
        public string labcontact { get; set; }

    }

    public class itmmAdminHead
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid First Name")]
        [Display(Name="First Name")]
        public string fname { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid Last Name")]
        //[Remote("CheckLastName", "Head")]
        [Display(Name = "Last Name")]
        public string lname { get; set; }

        [Required(ErrorMessage = "Required")]
        //[StringLength(50, MinimumLength = 7, ErrorMessage = "Must be at least 7 digits")]
        //[RegularExpression(@"^\d+$", ErrorMessage = "Please enter digits.")]
        [Display(Name = "Contact Number")]
        public int cnum { get; set; }

        [Required(ErrorMessage = "Required")]
        //[DataType(DataType.EmailAddress, ErrorMessage="Invalid Email")]
        [RegularExpression(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", ErrorMessage = "Invalid Email")]
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
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string c_password { get; set; }

    }

    public class itmmAdminStaff
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid First Name")]
        [Display(Name = "First Name")]
        public string fname { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid Last Name")]
        [Display(Name = "Last Name")]
        public string lname { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter digits.")]
        [Display(Name = "ID Number")]
        public string cnum { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Invalid Education Attained")]
        [Display(Name = "Education Attained")]
        public string course { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", ErrorMessage = "Invalid Email")]
       // [DataType(DataType.EmailAddress)]
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
        [Display(Name = "Confirm Password")]
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

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Location")]
        public string location { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid Serial")]
        [Remote("IsSerialExist", "Head")]  
        [Display(Name = "Serial")]
        public string serial { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid Barcode")]
        [Remote("IsBarcodeExist", "Head")]
        [Display(Name = "Barcode")]
        public string barcode { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Status")]
        public string status { get; set; }

 
        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Life Expectancy")]
        public string life { get; set; }
    }

    public class itmmNotification 
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Special characters not allowed")]
        [Display(Name = "What")]
        public string what { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9/]+$", ErrorMessage = "Special characters not allowed")]
        [Display(Name = "When")]
        public string whin { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Special characters not allowed")]
        [Display(Name = "Where")]
        public string whire { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Special characters not allowed")]
        [Display(Name = "Who")]
        public string who { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9-:\s]+$", ErrorMessage = "Special characters not allowed")]
        [Display(Name = "Time")]
        public string time { get; set; }
    }

    public class itmmSchedule {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Group No.")]
        public int GroupNo { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Assign Table")]
        public int AvailableTable { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid Course Code")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Course Description")]
        public string CourseDesc { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Day")]
        public string Day { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Schedule")]
        public string Schedule { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid Instrcutor Name")]
        [Display(Name = "Instructor")]
        public string Instructor { get; set; }
    }

    public class itmmMaterial
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid Material Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Remote("IsMaterialDescriptionUnique", "Head")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
     
    }

    public class itmmLiability
    {
        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Family Name")]
        //public string FamilyName { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "FirstName")]
        //public string FirstName { get; set; }
        string _status = "Unsettled";

        [Required(ErrorMessage = "Required")]
        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }


        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Course & Year")]
        //public string Course { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Equipment")]
        public string Equipment { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Fine")]
        public string Fine { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Status")]
        public string Status { 
            get {
                return _status;
            } 
            set{
                _status = value;
            } 
                
        }


    }

    public class itmmIncome
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid Family Name")]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Required")]
        [Display(Name = "ID Number")]
        public int IdNumber { get; set; }


        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Invalid Course and Year")]
        [Display(Name = "Course and Year")]
        public string Course { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid Transaction Input")]
        [Display(Name = "Transaction")]
        public string Transaction { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Cost")]
        public decimal Cost { get; set; }


    }

    public class itmmStudentsToTables {

        [Required(ErrorMessage = "Required")]
        [Display(Name = "ID Numbers (format : ID Number 1, ID Number2)")]
        public string IdNumbers { get; set; }
    }

    public class itmmExpenses
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Invalid Transaction Input")]
        [Display(Name = "Transaction")]
        public string Transaction { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Detail")]
        public string Detail { get; set; }


        [Required(ErrorMessage = "Required")]
        [Display(Name = "Cost")]
        public decimal Cost { get; set; }


    }

    


}