using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaAPI.Data;
using VillaAPI.Models;
using VillaAPI.Repository;

namespace VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }


        public async Task<VillaNumber> UpdateVillaAsync(VillaNumber villaNumber)
        {
            villaNumber.CreatedDate = DateTime.Now;
            db.villaNumbers.Update(villaNumber);
            await db.SaveChangesAsync();
            return villaNumber;
        }
    }
}
