using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZEntityFrameworkExtensionsDemo.Models
{
    public class ChickenCoop
    {
        public ChickenCoop(
            string name,
            int ownerId,
            Owner owner,
            ICollection<Chicken> housedChickens)
        {
            Name = name;
            OwnerId = ownerId;
            Owner = owner;
            HousedChickens = housedChickens;
        }

        public ChickenCoop(
            int chickenCoopId,
            string name,
            int ownerId,
            Owner owner,
            ICollection<Chicken> housedChickens)
        {
            ChickenCoopId = chickenCoopId;
            Name = name;
            OwnerId = ownerId;
            Owner = owner;
            HousedChickens = housedChickens;
        }

        [Obsolete("For EF/tests only.", error: true)]
        protected ChickenCoop() { }

        [Key]
        public int ChickenCoopId { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual Owner Owner { get; set; }
        public virtual ICollection<Chicken> HousedChickens { get; set; }
    }
}
