using System;
using System.Collections.Generic;

namespace EjemploBestDay.Models
{
    class PetMapping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int OwnerId { get; set; }
        public List<Type> Tipos { get; set; }


}
}
