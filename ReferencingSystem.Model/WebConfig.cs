using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencingSystem.Model
{

    public class WebConfig
    {
        [Key]
        public int Id { get; set; }

        // record info

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class Navigation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string ValueType { get; set; }

        [Required]
        public int Order { get; set; }

        // record info

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class WebAssets
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Extension { get; set; }

        public int Size { get; set; }

        [Required]
        public byte[] File { get; set; }

        // record info

        public int CreatedUTC { get; set; }

        public string CreatedBy { get; set; }

        public int ModifiedUTC { get; set; }

        public string ModifiedBy { get; set; }
    }
}
