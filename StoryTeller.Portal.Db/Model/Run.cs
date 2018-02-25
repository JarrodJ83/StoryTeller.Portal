using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storyteller.Portal.Db.Model
{
    [Table(nameof(Run))]
    public class Run 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RunDateTime { get; set; }
        public virtual App App { get; set; }
    }
}