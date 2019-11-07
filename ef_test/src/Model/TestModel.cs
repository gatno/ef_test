using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ef_test.Model
{
    [Table("Test")]
    public class TestModel
    {
        [Key] public int Id { get; set; }

        public string TestColumn { get; set; }

    }
}
