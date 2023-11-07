using Microsoft.EntityFrameworkCore;
using BackendEmpleados.Models;
using BackendEmpleados.Services.Contrato;

namespace BackendEmpleados.Services.Implementacion {
  public class DepartamentoService : IDepartamentoService{
    private DbempleadosContext db_context;

    public DepartamentoService(DbempleadosContext context) {
      this.db_context = context;
    }

    public async Task<List<Departamento>> GetList() {
      try {
        List<Departamento> list = new List<Departamento>();
        list = await db_context.Departamentos.ToListAsync();
        return list;
      } catch (Exception) {

        throw;
      }
    }
  }
}
