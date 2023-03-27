using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaAPI.Models;
using VillaAPI.Models.DTO;
using VillaAPI.Repository;

namespace VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        protected ResponseObject response;
        private readonly IVillaNumberRepository villaNumberRepository;
        private readonly IMapper mapper;
        private readonly IVillaRepository villaRepository;

        public VillaNumberController(IVillaNumberRepository villaNumberRepository, IMapper mapper,IVillaRepository villaRepository)
        {
            this.villaNumberRepository = villaNumberRepository;
            this.mapper = mapper;
            this.villaRepository = villaRepository;
            this.response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseObject>> GetAllVillaNumbers()
        {
            try
            {
                var villaNumbers = await villaNumberRepository.GetAllAsync();
                response.Result = mapper.Map<List<VillaNumberDTO>>(villaNumbers);
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
        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ResponseObject>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {

                    response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                var villaNumber = await villaNumberRepository.GetAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                {
                    response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                response.Result = mapper.Map<VillaNumberDTO>(villaNumber);
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseObject>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCraeteDTO)
        {
           
            try
            {
                if (await villaNumberRepository.GetAsync(x => x.VillaNo == villaNumberCraeteDTO.VillaNo)!=null)
                {
                    ModelState.AddModelError("CustomError", "Villa Number Already exist in the table");
                    return BadRequest(ModelState);
                }
                if (villaNumberCraeteDTO == null)
                {

                    return BadRequest(villaNumberCraeteDTO);
                }
                if(await villaRepository.GetAsync(x=>x.Id== villaNumberCraeteDTO.VillaId)==null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }
                VillaNumber model = mapper.Map<VillaNumber>(villaNumberCraeteDTO);

                await villaNumberRepository.CreateAsync(model);
                response.Result = model;
                response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { id = model.VillaNo }, response);
            }
            catch (Exception ex)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseObject>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var villaNumber = await villaNumberRepository.GetAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                    return NotFound();

                await villaNumberRepository.DeleteAsync(villaNumber);
                response.IsSuccess = true;
                response.statusCode = HttpStatusCode.NoContent;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;


        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ResponseObject>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO villNumberaUpdateDTO)
        {
            try
            {
                if (villNumberaUpdateDTO.VillaNo != id || villNumberaUpdateDTO == null)
                {
                    return BadRequest();
                }
                if (await villaNumberRepository.GetAsync(x => x.VillaNo == villNumberaUpdateDTO.VillaNo) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa Number Not exist in the table");
                    return BadRequest(ModelState);
                }
                if (await villaRepository.GetAsync(x => x.Id == villNumberaUpdateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }
                VillaNumber model = mapper.Map<VillaNumber>(villNumberaUpdateDTO);
                await villaNumberRepository.UpdateVillaAsync(model);
                response.IsSuccess = true;
                response.statusCode = HttpStatusCode.NoContent;
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }
    }
}
