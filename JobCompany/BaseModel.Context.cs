﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobCompany
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CoalCompanyEntities : DbContext
    {
        //Создается контекст базы данных
        private static CoalCompanyEntities _context;

        public CoalCompanyEntities()
            : base("name=CoalCompanyEntities")
        {
        }

        //Метод возращающий контекст
        public static CoalCompanyEntities GetContext()
        {
            //Если такого элемента в бд нет, то создается новый
            if (_context == null)
                _context = new CoalCompanyEntities();

            return _context;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<HistoryChange> HistoryChange { get; set; }
        public virtual DbSet<Picket> Picket { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
