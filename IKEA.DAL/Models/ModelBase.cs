using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Models
{
    public class ModelBase
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifictionBy {  get; set; }
        public DateTime LastModifictionOn { get;set; }
        public bool IsDeleted { get; set; }
    }
}
