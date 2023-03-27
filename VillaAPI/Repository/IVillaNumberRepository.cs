using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VillaAPI.Models;

namespace VillaAPI.Repository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateVillaAsync(VillaNumber villaNumber);

    }
}
