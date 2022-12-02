using AutoMapper;
using DBAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using SD;
using System.Threading;

namespace RfToDoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MetasController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public MetasController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMetas()
        {
            try
            {
                IEnumerable<MetaModel> MetasList = _mapper.Map<IEnumerable<Meta>, IEnumerable<MetaModel>>(_db.Metas);

                foreach (var meta in MetasList)
                {
                    List<Tarea> Tareas = await _db.Tareas.Where(t => t.Id_Meta == meta.Id_Meta).ToListAsync();

                    meta.TotalTareas = Tareas.Count;
                    meta.TareasCompletadas = Tareas.Where(x => x.Estado == SD.SD.Estatus_Completada).ToList().Count;
                    if (meta.TotalTareas != 0)
                    {
                        double v1 = meta.TareasCompletadas;
                        double v2 = meta.TotalTareas;
                        var result = v1 / v2;

                        meta.Porcentaje = result.ToString("P2");
                    }
                    else
                    {
                        meta.Porcentaje = (0.00).ToString("P2");
                    }
                }

                return Ok(MetasList);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{Id_Meta:int}")]
        public async Task<IActionResult> GetTareas(int Id_Meta)
        {
            try
            {
                IEnumerable<TareaModel> result = _mapper.Map<IEnumerable<Tarea>, IEnumerable<TareaModel>>(await _db.Tareas.Where(x => x.Id_Meta == Id_Meta).ToListAsync());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> NewMeta([FromBody] MetaModel meta)
        {
            if (await IsMetaUnique(meta.Nombre, meta.Id_Meta))
            {
                Meta newMeta = _mapper.Map<MetaModel, Meta>(meta);
                newMeta.Fecha_Creacion = DateTime.Now;

                var addedMeta = await _db.Metas.AddAsync(newMeta);
                await _db.SaveChangesAsync();

                return Ok(_mapper.Map<Meta, MetaModel>(addedMeta.Entity));
            }
            else
            { 
                return BadRequest("El nombre de la Meta no puede repetirse.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> NewTarea([FromBody] TareaModel tarea)
        {
            if (await IsTareaUnique(tarea.Nombre, tarea.Id_Tarea, tarea.Id_Meta))
            {
                Tarea newTarea = _mapper.Map<TareaModel, Tarea>(tarea);
                newTarea.Fecha_Creacion = DateTime.Now;

                var adddedTarea = await _db.Tareas.AddAsync(newTarea);
                await _db.SaveChangesAsync();

                return Ok(_mapper.Map<Tarea, TareaModel>(adddedTarea.Entity));
            }
            else
            {
                return BadRequest("El nombre de la Tarea no puede repetirse.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMeta([FromBody] MetaModel meta)
        {
            try
            {
                if (await IsMetaUnique(meta.Nombre, meta.Id_Meta))
                {
                    Meta myMetaDetails = await _db.Metas.FindAsync(meta.Id_Meta);
                    myMetaDetails.Nombre = meta.Nombre;

                    var updatedMeta = _db.Metas.Update(myMetaDetails);
                    await _db.SaveChangesAsync();

                    return Ok(_mapper.Map<Meta, MetaModel>(updatedMeta.Entity));
                }
                else
                {
                    return BadRequest("El nombre de la Meta no puede repetirse.");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTarea([FromBody] TareaModel tarea)
        {
            try
            {
                if (await IsTareaUnique(tarea.Nombre, tarea.Id_Tarea, tarea.Id_Meta))
                {
                    Tarea myTareaDetails = await _db.Tareas.FindAsync(tarea.Id_Tarea);
                    myTareaDetails.Nombre = tarea.Nombre;

                    var updatedTarea = _db.Tareas.Update(myTareaDetails);
                    await _db.SaveChangesAsync();

                    return Ok(_mapper.Map<Tarea, TareaModel>(updatedTarea.Entity));
                }
                else
                {
                    return BadRequest("El nombre de la Tarea no puede repetirse.");
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateImportancia([FromBody] TareaModel tarea)
        {
            try
            {
                Tarea myTareaDetails = await _db.Tareas.FindAsync(tarea.Id_Tarea);
                myTareaDetails.Importante = tarea.Importante;

                var updatedTarea = _db.Tareas.Update(myTareaDetails);
                await _db.SaveChangesAsync();

                return Ok(_mapper.Map<Tarea, TareaModel>(updatedTarea.Entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{Id:int}")]
        public async Task<IActionResult> CompletarTarea(int Id)
        {
            try
            {
                Tarea myTarea = await _db.Tareas.FirstOrDefaultAsync(t => t.Id_Tarea == Id);
                myTarea.Estado = SD.SD.Estatus_Completada;

                var updatedTarea = _db.Tareas.Update(myTarea);
                await _db.SaveChangesAsync();

                return Ok(_mapper.Map<Tarea, TareaModel>(updatedTarea.Entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompletarTarea([FromBody] List<int> tareas)
        {
            try
            {
                List<Tarea> myTareas = await _db.Tareas.Where(t => tareas.Contains(t.Id_Tarea)).ToListAsync();

                myTareas.ForEach(tarea => { tarea.Estado = SD.SD.Estatus_Completada;});

                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteMeta(int Id)
        {
            try
            {
                List<Tarea> myTareas = await _db.Tareas.Where(t => t.Id_Meta == Id).ToListAsync();
                _db.Tareas.RemoveRange(myTareas);

                await _db.SaveChangesAsync();

                var myMeta = await _db.Metas.FindAsync(Id);
                _db.Metas.Remove(myMeta);

                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteTarea(int Id)
        {
            try
            {
                var myTarea = await _db.Tareas.FindAsync(Id);
                _db.Tareas.Remove(myTarea);
                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTarea([FromBody] List<int> tareas)
        {
            try
            {
                List<Tarea> myTareas = await _db.Tareas.Where(t => tareas.Contains(t.Id_Tarea)).ToListAsync();
                _db.Tareas.RemoveRange(myTareas);

                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<bool> IsMetaUnique(string name, int Id_meta)
        {
            try
            {
                MetaModel meta = null;
                
                meta = _mapper.Map<Meta, MetaModel>(await _db.Metas.FirstOrDefaultAsync(m => m.Nombre.ToLower() == name.ToLower()));
                
                if (meta != null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception)
            {
                return true;
            }
        }

        private async Task<bool> IsTareaUnique(string name, int Id_Tarea, int Id_Meta)
        {
            try
            {
                TareaModel tarea = null;

                if (Id_Tarea == 0)
                {
                    tarea = _mapper.Map<Tarea, TareaModel>(await _db.Tareas.FirstOrDefaultAsync(m => m.Nombre.ToLower() == name.ToLower() 
                                                                                                && m.Id_Meta == Id_Meta && m.Estado == SD.SD.Estatus_Abierta));
                }
                else
                {
                    tarea = _mapper.Map<Tarea, TareaModel>(await _db.Tareas.FirstOrDefaultAsync(m => m.Nombre.ToLower() == name.ToLower() && m.Id_Meta == Id_Meta));
                }

                if (tarea != null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
