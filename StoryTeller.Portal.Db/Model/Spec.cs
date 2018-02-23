using System;

namespace Storyteller.Portal.Db.Model
{
    public class Spec 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid StorytellerId { get; set; }
        public virtual App App { get; set; }
    }
}