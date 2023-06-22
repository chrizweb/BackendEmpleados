using System;  
using BackendEmpleados.Models;
using Microsoft.EntityFrameworkCore;
using BackendEmpleados.Services.Contrato;
using BackendEmpleados.Services.Implementacion;
using AutoMapper;
using BackendEmpleados.DTOs;
using BackendEmpleados.Utilidades;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*CODIGO TRABAJADO*/

/*Agregando conexion a la base de datos*/
builder.Services.AddDbContext<DbempleadosContext>(option => {
  option.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});
/*Agregando interfaces y clases de implementacion*/
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

/*Agregando (AutoMapper) = Mapeo de Modelos y DTOs*/
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

/*END CODIGO TRABAJADO*/

/*Cors*/
builder.Services.AddCors(options => {
  options.AddPolicy("NuevaPolitica", app => {
    app.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod();
  });
});
  
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

#region API_REST
app.MapGet("/departamento/list", async(IDepartamentoService _departamentoService, IMapper _mapper ) => {
  List<Departamento> listaDepartamento = await _departamentoService.GetList();
  List<DepartamentoDTO> listaDepartamentoDTO = _mapper.Map<List<DepartamentoDTO>>(listaDepartamento);

  if (listaDepartamentoDTO.Count > 0) {
    return Results.Ok(listaDepartamentoDTO);
  }
  else {
    return Results.NotFound();
  }
});

app.MapGet("/empleado/list",async(IEmpleadoService _empleadoService, IMapper _mapper) => {
  List<Empleado> listaEmpleado = await _empleadoService.GetList();
  List<EmpleadoDTO> listaEmpleadoDTO = _mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

  if (listaEmpleadoDTO.Count > 0) {
    return Results.Ok(listaEmpleadoDTO);
  }
  else {
    return Results.NotFound();
  }
});

app.MapPost("/empleado/save", async (EmpleadoDTO empleado_dto, IEmpleadoService _empleadoService, IMapper _mapper) => {

  var empleado =  _mapper.Map<Empleado>(empleado_dto);
  var empleado_saved = await _empleadoService.Add(empleado);

  if (empleado_saved.IdEmpleado != 0) {
    return Results.Ok(_mapper.Map<EmpleadoDTO>(empleado_saved));
  }
  else {
    return Results.StatusCode(StatusCodes.Status500InternalServerError); 
  }

});

app.MapPut("/empleado/update/{id_empleado}", async (int id_empleado, EmpleadoDTO empleado_dto, IEmpleadoService _empleadoService, IMapper _mapper) => {

  var empleado_encontrado = await _empleadoService.Get(id_empleado);

  if(empleado_encontrado == null) {

    return Results.NotFound();
  } 

  var empleado = _mapper.Map<Empleado>(empleado_dto);
  empleado_encontrado.NombreCompleto = empleado.NombreCompleto;
  empleado_encontrado.IdDepartamento = empleado.IdDepartamento;
  empleado_encontrado.Sueldo = empleado.Sueldo;
  empleado_encontrado.FechaContrato = empleado.FechaContrato;

  var empleado_updated = await _empleadoService.Update(empleado_encontrado);
  if (empleado_updated) {

    return Results.Ok(_mapper.Map<EmpleadoDTO>(empleado_encontrado));
  }
  else {
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }

});

app.MapDelete("/empleado/delete/{id_empleado}", async (int id_empleado, IEmpleadoService _empleadoService) =>{

  var empleado_encontrado = await _empleadoService.Get(id_empleado);
  if (empleado_encontrado is null) return Results.NotFound();

  var empleado_deleted = await _empleadoService.Delete(empleado_encontrado);
  if (empleado_deleted) {
    return Results.Ok();
  }
  else {

    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
    
});
#endregion

app.UseCors("NuevaPolitica");
app.Run();

