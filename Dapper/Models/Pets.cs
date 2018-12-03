using System;

namespace EjemploBestDay.Models
{
    class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int TypeId { get; set; }
        public int OwnerId { get; set; }


    }
}
