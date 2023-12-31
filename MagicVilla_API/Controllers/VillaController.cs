﻿
using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;

        private readonly IVillaRepositorio _villaRepo;

        private readonly IMapper _mapper;

        protected APIResponse _response;
        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //1º public ActionResult<IEnumerable<VillaDto>> GetVillas()
        //Metodo normal

        //2º public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        // Metodo asincrono

        public async Task<ActionResult<APIResponse>> GetVillas()
        // 3º Ahora retorna APIResponse
        {
            try
            // try {} catch{} se usa como filtro para saber si hay errores
            {
                _logger.LogInformation("Obtener las Villas");
                /* return Ok(_db.Villas.ToList());
                Metodo normal */

                IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _response.statusCode = HttpStatusCode.OK;
                // 3º Asi queda con APIResponse
                return Ok(_response);

                // 2º return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
                // Metodo asincrono

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Villa con Id" + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRepo.Obtener(v => v.Id == id);

                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La Villa con ese nombre ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                /*if (villaDto.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }*/

                /*villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                VillaStore.villaList.Add(villaDto);*/

                Villa modelo = _mapper.Map<Villa>(createDto);

                /*Villa modelo = new()
                {
                    Nombre = villaDto.Nombre,
                    Detalle = villaDto.Detalle,
                    ImageUrl = villaDto.ImageUrl,
                    Ocupantes = villaDto.Ocupantes,
                    Tarifa = villaDto.Tarifa,
                    Amenidad = villaDto.Amenidad,
                    MetrosCuadrados = villaDto.MetrosCuadrados,
                };*/
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _villaRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = modelo.Id, _response });

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        // Como IActionResult viene de una interfaz, en este caso no podemos poner <IActionResult<APIResponse>>, hay que quitar APIResponse
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRepo.Obtener(v => v.Id == id);
                if (villa == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                await _villaRepo.Remover(villa);

                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa modelo = _mapper.Map<Villa>(updateDto);

            /*Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImageUrl = villaDto.ImageUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad,
            };*/

            await _villaRepo.Actualizar(modelo);

            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);

        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        // JsonPatchDocument<> al mencionarlo, hace referencia a la libreria de Json
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            /*var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
                Metodo anterior:
                AsNoTracking() es un método que se utiliza para indicar a Entity Framework que no realice seguimiento de las entidades recuperadas en el contexto*/

            var villa = await _villaRepo.Obtener(v => v.Id == id, tracked: false);
            // Nuevo metodo:
            // Para mantener AsNoTracking que servia para que no hiciera seguimiento de las entidades, se pone tracked: false

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            /*VillaUpdateDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImageUrl = villa.ImageUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad,
            };*/

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa modelo = _mapper.Map<Villa>(villaDto);

            /*Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImageUrl = villaDto.ImageUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad,
            };*/

            await _villaRepo.Actualizar(modelo);

            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);

        }
    }
}
