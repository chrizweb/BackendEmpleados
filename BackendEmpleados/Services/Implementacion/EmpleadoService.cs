using Microsoft.EntityFrameworkCore;
using BackendEmpleados.Models;
using BackendEmpleados.Services.Contrato;

namespace BackendEmpleados.Services.Implementacion {
  public class EmpleadoService : IEmpleadoService {
    private DbempleadosContext db_context;

    public EmpleadoService(DbempleadosContext context) {
      db_context = context;
    }
    public async Task<List<Empleado>> GetList() {
      try {

        List<Empleado> list = new List<Empleado>();
        list = await db_context.Empleados.Include(dpt => dpt.IdDepartamentoNavigation).ToListAsync();

        return list;

       } catch (Exception) {
        
        throw;
      }
    }

    public async Task<Empleado> Get(int id_empleado) {
      try {
        
        Empleado? empleado_encontrado = new Empleado();
        empleado_encontrado = await db_context.Empleados.Include(dpt => dpt.IdDepartamentoNavigation)
          .Where(empleado => empleado.IdEmpleado == id_empleado)
          .FirstOrDefaultAsync();

        return empleado_encontrado;
         
      } catch (Exception) {

        throw;
      }
    }

    public async Task<Empleado> Add(Empleado empleado) {
      try {

        db_context.Empleados.Add(empleado);
        await db_context.SaveChangesAsync();

        return empleado;

      } catch (Exception) {

        throw;
      }
    }

    public async Task<bool> Update(Empleado empleado) {
      try {

        db_context.Empleados.Update(empleado);
        await db_context.SaveChangesAsync();

        return true;

      } catch (Exception) {

        throw;
      }
    }

    public async Task<bool> Delete(Empleado empleado) {
      try {

        db_context.Empleados.Remove(empleado);
        await db_context.SaveChangesAsync();

        return true;

      } catch (Exception) {

        throw;
      }
    }
   
  }
}











