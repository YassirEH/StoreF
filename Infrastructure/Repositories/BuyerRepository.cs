﻿using Core.Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class BuyerRepository : IBuyerRep
    {
        private readonly DataContext _context;

        public BuyerRepository(DataContext context)
        {
            _context = context;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}