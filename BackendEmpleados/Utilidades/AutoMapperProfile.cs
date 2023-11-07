using AutoMapper;
using BackendEmpleados.DTOs;
using BackendEmpleados.Models;
using System.Globalization;

namespace BackendEmpleados.Utilidades {
  public class AutoMapperProfile : Profile{

    public AutoMapperProfile() {
      #region Departamento
      /*Pasar data de Modelo a DTO*/
      CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
      #endregion

      #region Empleado
      /*Pasar data de Modelo a DTO*/
      CreateMap<Empleado, EmpleadoDTO>()
        .ForMember(destino =>
          destino.NombreDepartamento,
          opt => opt.MapFrom(origen =>
          origen.IdDepartamentoNavigation.Nombre)
        )
        .ForMember(destino =>
          destino.FechaContrato,
          opt => opt.MapFrom(origen =>
          origen.FechaContrato.Value.ToString("dd/MM/yyyy"))
        );
      /*Pasar data de DTO a Modelo */
      CreateMap<EmpleadoDTO, Empleado>()
        .ForMember(destino =>
          destino.IdDepartamentoNavigation,
          opt => opt.Ignore()
        )
        .ForMember(destino =>
          destino.FechaContrato,
          opt => opt.MapFrom(
          origen => DateTime.ParseExact(origen.FechaContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture)
          )
        );

      #endregion


    }
  }
}
