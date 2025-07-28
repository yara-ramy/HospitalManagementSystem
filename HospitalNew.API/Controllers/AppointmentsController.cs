﻿using AutoMapper;
using HospitalNew.BLL.Dtos;
using HospitalNew.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalNew.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsService _appointmentsService;
        private readonly IMapper _mapper;
        private readonly IPatientsService _patientsService;
        private readonly IDoctorGenericService _service;
        public AppointmentsController(IAppointmentsService appointmentsService, IMapper mapper, IPatientsService patientsService, IDoctorGenericService service)
        {
            _appointmentsService = appointmentsService;
            _mapper = mapper;
            _patientsService = patientsService;
            _service = service;
        }
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentsService.GetAppointmentsDetails();
            var app = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(appointments);
            return Ok(app);
        }
        [Authorize(Roles = "Admin,Receptionist,Patient")]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm]AppointmentDto dto)
        {
            var result = await _appointmentsService.AddAppointment(dto);
            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Receptionist,Patient")]

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] AppointmentDto dto)
        {
            var result = await _appointmentsService.UpdateAppointment(id, dto);
            if (result == null)
                return NotFound($"No appointment exists with the ID: {id}");
            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Receptionist")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _appointmentsService.DeleteAppointment(id);
            if (result == null)
                return NotFound($"No appointment exists with the ID: {id}");
            return Ok(result);
        }

    }
}
