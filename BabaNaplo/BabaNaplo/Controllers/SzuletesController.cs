﻿using BabaNaplo.Models;
using BabaNaplo.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Cors;
namespace BabaNaplo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class SzuletesController : ControllerBase
    {

        private readonly BabanaploContext _context;  //AMÁ

        public SzuletesController(BabanaploContext context)  //AMÁ
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult Get()
        {
            var context = new BabanaploContext();
            try
            {
                return Ok(_context.Szuletes.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetFull")]
        public IActionResult GetFull()
        {
            try
            {
                var result = _context.Szuletes.Include(x => x.Kedvenceks).Select(f => new { f.Nev, f.Kedvenceks });
                return Ok(result);
            }
            catch (Exception ex)

            { return BadRequest(ex.Message); }
        }
        [HttpGet("GetDTO")]
        public async Task<IActionResult> GetDTO()
        {
            try
            {
                return Ok(await _context.Szuletes.Select(f => new SzuletesDTO() { Id = f.BabaId, Nev = f.Nev, Hely = f.Hely, Idopont = f.Idopont }).ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPost]
        public IActionResult Post(Szuletes szuletes)
        {
            try
            {
                _context.Add(szuletes);
                _context.SaveChanges();
                return Ok(_context.Szuletes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public IActionResult Put(Szuletes szuletes)
        {
            try
            {
                _context.Update(szuletes);
                _context.SaveChanges();
                return Ok(_context.Szuletes);
            }
            catch (Exception ex)

            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Szuletes szuletes = new Szuletes();
                szuletes.BabaId = id;
                _context.Remove(szuletes);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, "Sikeres törlés.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("SearchSzuletesId/{id}")] //AMÁ
        public async Task<ActionResult<Szuletes>> SearchSzuletesId(int id)
        {
            var szuletesIds = await _context.Szuletes.FindAsync(id);

            if (szuletesIds == null)
            {
                return NotFound();
            }

            return szuletesIds;
        }
    }
}
