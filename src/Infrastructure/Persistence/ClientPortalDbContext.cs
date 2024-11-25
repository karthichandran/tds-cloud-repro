using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ReProServices.Infrastructure.Persistence
{
    public class ClientPortalDbContext : DbContext
    {
        public ClientPortalDbContext(DbContextOptions<ClientPortalDbContext> options) : base(options) { }
    }
}
