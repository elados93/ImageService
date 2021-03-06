﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    public class Employee
    {
        public Employee(string firstName, string lastName, int iD)
        {
            FirstName = firstName;
            LastName = lastName;
            ID = iD;
        }

        public void copy(Employee emp)
        {
            FirstName = emp.FirstName;
            ID = emp.ID;
            LastName = emp.LastName;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public int ID { get; set; }

    }
}
