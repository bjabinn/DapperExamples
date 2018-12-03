using System;
using System.Collections.Generic;

namespace EjemploBestDay.Models
{
    class TiposRelated
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public List<Pet> Animales { get; set; }        
    }
}
