using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using VillaAPI.Data;
using VillaAPI.Logging;
using VillaAPI.Models;
using VillaAPI.Models.DTO;
using VillaAPI.Repository;

namespace VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        protected ResponseObject response;
        private readonly IVillaRepository villaRepository;
        private readonly IMapper mapper;

        //private readonly ILogging logger;

        //public VillaController(ILogging logger)
        //{
        //    this.logger = logger;
        //}
        public VillaController(IVillaRepository villaRepository,IMapper mapper)
        {
            this.villaRepository = villaRepository;
            this.mapper = mapper;
            this.response=new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseObject>> GetAllVillas()
        {
            // logger.Log("Getting all informations","");
          
            //var villas = await villaRepository.GetAllAsync();
           // response.Result = mapper.Map<List<VillaDTO>>(villas);
           // response.statusCode = HttpStatusCode.OK;
           // return Ok(response);
          // return response;
            try
            {
                // IEnumerable<Villa> villas= await villaRepository.GetAllAsync();
                var villas = await villaRepository.GetAllAsync();
                response.Result = mapper.Map<List<VillaDTO>>(villas);
                response.statusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;

        }
        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404, Type = typeof(VillaDTO))]
        //[ProducesResponseType(200,Type=typeof(VillaDTO))]
       // public IActionResult<VillaDTO> GetVilla(int id)
        public async Task<ActionResult<ResponseObject>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    // logger.Log("Get Villa with id throws an error"+id,"error") ;
                    response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                var villa = await villaRepository.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                response.Result = mapper.Map<VillaDTO>(villa);
                response.statusCode = HttpStatusCode.OK;
                return Ok(response);

            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages=new List<string>() { ex.ToString()};
            }
            return response;
            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //ActionResult<VillaDTO> or IActionResult
        public async Task<ActionResult<ResponseObject>> CreateVilla([FromBody] VillaCreateDTO villaCraeteDTO)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            try
            {
                if (await villaRepository.GetAsync(x => x.Name.ToLower() == villaCraeteDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Name Already exist in the table");
                    //response.ErrorMessages = new List<string> { "Villa Name Already exist in the table" };
                    // response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }
                if (villaCraeteDTO == null)
                {

                    return BadRequest(villaCraeteDTO);
                }
                Villa model = mapper.Map<Villa>(villaCraeteDTO);

                //Villa model = new Villa()
                //{
                //    Name = villaCraeteDTO.Name,
                //    Rate = villaCraeteDTO.Rate,
                //    Sqft = villaCraeteDTO.Sqft,
                //    Occupancy = villaCraeteDTO.Occupancy,
                //    ImageUrl = villaCraeteDTO.ImageUrl,
                //    Amenity = villaCraeteDTO.Amenity,
                //    Details = villaCraeteDTO.Details,
                //};

                await villaRepository.CreateAsync(model);
                //await villaRepository.Save();
                // response.Result=mapper.Map<VillaDTO>(villaCraeteDTO);
                response.Result = model;
                response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = model.Id }, response);
                // return Ok(model);
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;


        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ResponseObject>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var villa = await villaRepository.GetAsync(x => x.Id == id);
                if (villa == null)
                    return NotFound();

                await villaRepository.DeleteAsync(villa);
                response.IsSuccess = true;
                response.statusCode = HttpStatusCode.NoContent;
                // await villaRepository.Save();
                // return NoContent();
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
            

        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ResponseObject>> UpdateVilla(int id, [FromBody]VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO.Id != id || villaUpdateDTO == null)
                {
                    return BadRequest();
                }
                //var villa = db.Villas.FirstOrDefault(x => x.Id == id);
                //villa.Name = villaDTO.Name;
                //villa.Sqrft= villaDTO.Sqrft;
                //villa.Occupancy = villaDTO.Occupancy;

                Villa model = mapper.Map<Villa>(villaUpdateDTO);
                //Villa model = new Villa()
                //{
                //    Id = villaUpdateDTO.Id,
                //    Name = villaUpdateDTO.Name,
                //    Rate = villaUpdateDTO.Rate,
                //    Sqft = villaUpdateDTO.Sqft,
                //    Occupancy = villaUpdateDTO.Occupancy,
                //    ImageUrl = villaUpdateDTO.ImageUrl,
                //    Amenity = villaUpdateDTO.Amenity,
                //    Details = villaUpdateDTO.Details,
                //};
                await villaRepository.UpdateVillaAsync(model);
                response.IsSuccess = true;
                response.statusCode = HttpStatusCode.NoContent;
                //await villaRepository.Save();
                //return NoContent();
                return Ok(response);

            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages=new List<string>() { ex.ToString() };    
            }
            return response;
            

        }
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id,JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if(patchDTO==null || id==0)
            {
                return BadRequest();
            }
            var villa=await villaRepository.GetAsync(x=>x.Id==id,tracked:false);
            VillaUpdateDTO villaDTO = mapper.Map<VillaUpdateDTO>(villa);
            //VillaUpdateDTO villaDTO = new VillaUpdateDTO()
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft,
            //    Occupancy = villa.Occupancy,
            //    ImageUrl = villa.ImageUrl,
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //};
            if (villa == null)
                return BadRequest();

            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = mapper.Map<Villa>(villaDTO);
            //Villa model = new Villa()
            //{
            //    Id = villaDTO.Id,
            //    Name = villaDTO.Name,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //    Occupancy = villaDTO.Occupancy,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //};
            await villaRepository.UpdateVillaAsync(model);
            //db.Villas.Update(model);
            //await db.SaveChangesAsync();   
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();
        }

    }
}
