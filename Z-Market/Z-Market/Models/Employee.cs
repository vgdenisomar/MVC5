using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Z_Market.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Display(Name = "Primer Nombre")]
        [Required(ErrorMessage = "Usted debe ingresar {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {1} y {2} caracteres", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Display(Name = "Segundo Nombre")]
        [Required(ErrorMessage = "Usted debe ingresar {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {1} y {2} caracteres", MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "Salario")]
        [Required(ErrorMessage = "Usted debe ingresar {0}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public Decimal Salary { get; set; }
        [Display(Name = "Porcentaje de bonus")]
        [AllowHtml]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public float? BonusPercent { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "Usted debe ingresar {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Hora inicial")]
        [Required(ErrorMessage = "Usted debe ingresar {0}")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }
        [Display(Name = "Correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Url)]
        public string URL { get; set; }

        [Required(ErrorMessage = "Usted debe ingresar {0}")]
        public int DocumentTypeID { get; set; }

        [NotMapped]
        public int Age { get { return DateTime.Now.Year - DateOfBirth.Year; } }

        public virtual DocumentType DocumentType { get; set; }

    }
}