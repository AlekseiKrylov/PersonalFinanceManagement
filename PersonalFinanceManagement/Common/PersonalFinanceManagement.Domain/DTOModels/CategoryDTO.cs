﻿using PersonalFinanceManagement.Interfaces.Base.Entities;

namespace PersonalFinanceManagement.Domain.ModelsDTO
{
    public class CategoryDTO : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIncome { get; set; }
        public int WalletId { get; set; }
    }
}
