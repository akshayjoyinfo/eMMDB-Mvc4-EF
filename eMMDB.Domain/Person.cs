using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace eMMDB.Domain
{
    public class Person
    {
        [Key]
        public virtual int PersonID { get; set; }
        
        [Display(Name="Actor Name")]
        [Required(ErrorMessage= "Actor Name can not be Blank")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage="Sex can not be blank")]
        public virtual string Sex { get; set; }

        [Display(Name="Date of Birth")]
        public virtual DateTime? Dob { get; set; }

        [Display(Name="Biography")]
        public virtual string Bio { get; set; }
    }
}
