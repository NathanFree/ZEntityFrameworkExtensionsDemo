using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZEntityFrameworkExtensionsDemo.Models
{
    public class Chicken
    {

        [Obsolete("For EF/tests only.", error: true)]
        protected Chicken() { }

        public Chicken(
            string name,
            bool isAdoptable,
            int chickenBreedId,
            ChickenBreed chickenBreed,
            int chickenCoopId,
            ChickenCoop chickenCoop,
            int? ownerId,
            Owner owner)
        {
            Name = name;
            IsAdoptable = isAdoptable;
            ChickenBreedId = chickenBreedId;
            ChickenBreed = chickenBreed;
            ChickenCoopId = chickenCoopId;
            ChickenCoop = chickenCoop;
            OwnerId = ownerId;
            Owner = owner;
        }

        [Key]
        public int ChickenId { get; set; }
        public string Name { get; set; }
        public bool IsAdoptable { get; set; }
        public int ChickenBreedId { get; set; }
        [ForeignKey(nameof(ChickenBreedId))]
        public virtual ChickenBreed ChickenBreed { get; set; }
        public int ChickenCoopId { get; set; }
        [ForeignKey(nameof(ChickenCoopId))]
        public virtual ChickenCoop ChickenCoop { get; set; }
        public int? OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual Owner Owner { get; set; }

    }
}
