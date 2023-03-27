using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VillaAPI.Models;

namespace VillaAPI.Repository
{
    public interface IVillaRepository:IRepository<Villa>
    {
        Task<Villa> UpdateVillaAsync(Villa villa);

    }
}
