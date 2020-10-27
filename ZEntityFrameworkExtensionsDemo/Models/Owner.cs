using System;
using System.Collections.Generic;
using System.Text;

namespace ZEntityFrameworkExtensionsDemo.Models
{
    public class Owner
    {
        public Owner(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [Obsolete("For EF/tests only.", error: true)]
        protected Owner() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ChickenCoop> ChickenCoops { get; set; }
        public virtual ICollection<Chicken> Chickens { get; set; }
    }
}
