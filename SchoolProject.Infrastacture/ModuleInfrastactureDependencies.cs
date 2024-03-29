﻿using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Infrastacture.Abstracts;
using SchoolProject.Infrastacture.InfrastructureBases;
using SchoolProject.Infrastacture.Repositories;
using System;

namespace SchoolProject.Infrastacture
{
    public static class ModuleInfrastactureDependencies
    {

        public static IServiceCollection AddInfrastactureDependencies (this IServiceCollection services)
        {
            services.AddTransient<IStudentRepository, StudentRepository>();

            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            return services;

        }
        
    }
}
