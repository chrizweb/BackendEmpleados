using System;
using AutoMapper;
using BackendEmpleados.DTOs;
using BackendEmpleados.Models;
using BackendEmpleados.Services.Contrato;
using BackendEmpleados.Services.Implementacion;
using BackendEmpleados.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

/*Agregando AutoMapper*/
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

/*Cors*/
builder.Services.AddCors(options => {
  options.AddPolicy("NuevaPolitica", app => {
    app.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod();
  });
});

/*END CODIGO TRABAJADO*/
  
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

#region API_REST
app.MapGet("/departamento/list", async (IDepartamentoService departamentoService, IMapper mapper) => {

  List<Departamento> listaDepartamento = await departamentoService.GetList();
  List<DepartamentoDTO> listaDepartamentoDTO = mapper.Map<List<DepartamentoDTO>>(listaDepartamento);

  if (listaDepartamentoDTO.Count > 0) {
    return Results.Ok(listaDepartamentoDTO);
  }
  else {
    return Results.NotFound();
  }
});

app.MapGet("/empleado/list", async (IEmpleadoService empleadoService, IMapper mapper) => {

  List<Empleado> listaEmpleado = await empleadoService.GetList();
  List<EmpleadoDTO> listaEmpleadoDTO = mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

  if(listaEmpleadoDTO.Count > 0) {
    return Results.Ok(listaEmpleadoDTO);
  }
  else {
    return Results.NotFound();
  }
});

app.MapPost("/empleado/save", async (EmpleadoDTO empleado_dto, IEmpleadoService empleadoService, IMapper mapper) => {

  var empleado = mapper.Map<Empleado>(empleado_dto);
  var empleado_saved = await empleadoService.Add(empleado);

  if(empleado_saved.IdEmpleado != 0) {
    return Results.Ok(mapper.Map<EmpleadoDTO>(empleado_saved));
  }
  else {
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
  }
});

app.MapPut("/empleado/update/{id_empleado}", async (int id_empleado, EmpleadoDTO empleado_dto, IEmpleadoService empleadoService, IMapper mapper) => {

  var empleado_encontrado = await empleadoService.Get(id_empleado);

  if (empleado_encontrado == null) {
    return Results.NotFound();
  }

  var empleado_req = mapper.Map<Empleado>(empleado_dto);
  empleado_encontrado.NombreCompleto = empleado_req.NombreCompleto;
  empleado_encontrado.IdDepartamento = empleado_req.IdDepartamento;
  empleado_encontrado.Sueldo = empleado_req.Sueldo;
  empleado_encontrado.FechaContrato = empleado_req.FechaContrato;

  var empleado_updated = await empleadoService.Update(empleado_encontrado);

  if (empleado_updated) {
    return Results.Ok(mapper.Map<EmpleadoDTO>(empleado_encontrado));
  }
  else {
    return Results.StatusCode(StatusCodes.Status500InternalServerError); 
  }

});

app.MapDelete("/empleado/delete/{id_empleado}", async (int id_empleado, IEmpleadoService empleadoService) => {

  var empleado_encontrado = await empleadoService.Get(id_empleado);
  if (empleado_encontrado is null)
    return Results.NotFound();

  var empleado_deleted = await empleadoService.Delete(empleado_encontrado);
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

