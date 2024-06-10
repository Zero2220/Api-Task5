using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Services.Exceptions;
using Services.Services.Interfaces;
using UniversityApi.Data;
using UniversityApi.Dtos;
using Microsoft.Extensions.Hosting;
using Services.Services.Extensions.UniversityApp.Service.Extentions;
using Microsoft.AspNetCore.Hosting;

namespace Services.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly UniDatabase _uniDatabase;
        private readonly IWebHostEnvironment _environment;


        public StudentService(UniDatabase uniDatabase, IWebHostEnvironment environment)
        {
            _uniDatabase = uniDatabase ;
            _environment = environment;
        }

        public int Create(CreateStudentDto createDto)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            if (!_uniDatabase.Groups.Any(g => g.Id == createDto.GroupId))
                throw new NullReferenceException();
             
            var student = new Student
            {
                GroupId = createDto.GroupId,
                FullName = createDto.FullName,
                Email = createDto.Email,
                BirthDate = createDto.BirthDate,
                ImageName = createDto.FormFile.Save(_environment.WebRootPath, "/uploads/students")
            };

            _uniDatabase.Students.Add(student);
            _uniDatabase.SaveChanges();

            return student.Id;
        }

        public int Delete(int id)
        {
            var student = _uniDatabase.Students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                throw new NullReferenceException($"Student with ID {id} not found.");

            _uniDatabase.Students.Remove(student);
            _uniDatabase.SaveChanges();

            return student.Id;
        }

        public int Edit(int id, EditStudentDto editDto)
        {
            var student = _uniDatabase.Students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                throw new NullReferenceException();

            if (!_uniDatabase.Groups.Any(g => g.Id == editDto.GroupId))
                throw new NullReferenceException();

            student.GroupId = editDto.GroupId;
            student.FullName = editDto.FullName;
            student.Email = editDto.Email;
            student.BirthDate = editDto.BirthDate;

            _uniDatabase.SaveChanges();

            return student.Id;
        }

        public List<GetStudentDto> GetAll()
        {
            return _uniDatabase.Students
                .Select(s => new GetStudentDto
                {
                    FullName = s.FullName,
                    Email = s.Email,
                    BirthDate = s.BirthDate
                })
                .ToList();
        }

        public GetStudentDto GetById(int id)
        {
            var student = _uniDatabase.Students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                throw new NullReferenceException();

            return new GetStudentDto
            {
                FullName = student.FullName,
                Email = student.Email,
                BirthDate = student.BirthDate
            };
        }
    }
}
