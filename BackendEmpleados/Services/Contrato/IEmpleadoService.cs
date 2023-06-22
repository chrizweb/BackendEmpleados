using BackendEmpleados.Models;

namespace BackendEmpleados.Services.Contrato {
  public interface IEmpleadoService {
    Task<List<Empleado>> GetList();
    Task<Empleado> Get(int id_empleado);
    Task<Empleado> Add(Empleado empleado);
    Task<bool> Update(Empleado empleado);
    Task<bool> Delete(Empleado empleado);
  }
}
