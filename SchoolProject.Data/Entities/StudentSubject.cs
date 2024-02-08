using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

    public class StudentSubject
    {
        [Key]
        public int StudSubID { get; set; }
        public int StudID { get; set; }
        public int SubID { get; set; }


        //NAVIGATION PROPERTY
        [ForeignKey("StudID")]
        public virtual Student Student { get; set; }

        [ForeignKey("SubID")]
        public virtual Subjects Subject { get; set; }

    }
