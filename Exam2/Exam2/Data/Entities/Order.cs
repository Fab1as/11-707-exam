using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Data.Entities
{
    public class Order : BaseEntity
    {
        public IEnumerable<Dish> Dishes { get; set; }
    }
}
