﻿using PersonalFinanceManagement.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManagement.Domain.DTOModels
{
    public class WalletDTO : IEntity
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        public int UserId { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as WalletDTO;
            return other?.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Name is null ? string.Empty : Name;
    }
}
