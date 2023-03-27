using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaAPI.Data;
using VillaAPI.Models;
using VillaAPI.Repository;

namespace VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>,IVillaRepository
    {
        private readonly ApplicationDbContext db;

        public VillaRepository(ApplicationDbContext db):base(db)
        {
            this.db = db;
        }


        public async Task<Villa> UpdateVillaAsync(Villa villa)
        {
            villa.Updated = DateTime.Now;
            db.Update(villa);
            await db.SaveChangesAsync();
            return villa;
        }
    }
}
