using System;

namespace Storyteller.Portal.Db.Model
{
    public class Run 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RunDateTime { get; set; }
        public virtual App App { get; set; }
    }
}