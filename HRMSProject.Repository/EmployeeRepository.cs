﻿using HRMSProject.Repository.Interfaces;
using HRMSProject.Repository.Models;
using HRMSProject.Repository.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSProject.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly NextTurnDBContext _myContext;
        public EmployeeRepository(NextTurnDBContext myContext)
        {
            _myContext = myContext;
        }

        public async  Task<List<Employee>> GetAllEmployees()
        {
            return await _myContext.Employees.ToListAsync();
        }
        public async Task<Employee> InsertEmployee(Employee emp)
        {
            var result = await _myContext.Employees.AddAsync(emp);
            await _myContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<List<Employee>> GetEmpDetails(int? DeptId)
        {
            var result = await _myContext.Employees.FromSqlRaw("exec GetEmployeesByDept {0}", DeptId).ToListAsync();
            return result;
        }

        public async Task<Employee> DeleteEmployee(int? EmpId)
        {

            var result = await _myContext.Employees.Where(x => x.EmpId == EmpId).SingleOrDefaultAsync();
            var result2 = _myContext.Employees.Remove(result);
            await _myContext.SaveChangesAsync();
            return result2.Entity;
        }

        public async Task<Employee> UpdateEmployee(Employee emp)
        {

            var result = await _myContext.Employees.Where(k => k.EmpId == emp.EmpId).SingleOrDefaultAsync();
            _myContext.Employees.Remove(result);
            _myContext.Employees.Update(emp);
            _myContext.SaveChanges();
            return null;
        }
    }
}
