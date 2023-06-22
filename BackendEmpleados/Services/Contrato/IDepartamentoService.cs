using BackendEmpleados.Models;

namespace BackendEmpleados.Services.Contrato {
  public interface IDepartamentoService {
    Task<List<Departamento>> GetList();
  }
}
