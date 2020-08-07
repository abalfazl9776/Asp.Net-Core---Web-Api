using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public static class ModelConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
//            modelBuilder.Entity<ProductFabric>().HasKey(pf => new { pf.ProductId, pf.FabricId });
        }
    }
}
