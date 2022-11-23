using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CogniSmiles.Models;

namespace CogniSmiles.Data
{
    public class CogniSmilesContext : DbContext
    {
        public CogniSmilesContext (DbContextOptions<CogniSmilesContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patient { get; set; } = default!;
        public DbSet<Doctor> Doctor { get; set; } = default!;
        public DbSet<Login> Login { get; set; } = default!;
        public DbSet<PatientFile> PatientFile { get; set; } = default!; 

    }
}
