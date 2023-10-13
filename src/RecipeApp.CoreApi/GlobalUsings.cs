global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Reflection;

global using Dapper;

global using FluentValidation;
global using FluentValidation.AspNetCore;

global using HealthChecks.UI.Client;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.IdentityModel.Logging;

global using RecipeApp.CoreApi.Features.HealthChecks;
global using RecipeApp.CoreApi.Features.Ingredient.V1_0;
global using RecipeApp.CoreApi.Features.Instruction.V1_0;
global using RecipeApp.CoreApi.Features.Introduction.V1_0;
global using RecipeApp.Database.Ef.RecipeDb;
global using RecipeApp.Shared.Handlers;

global using Tbd.Shared.ApiLog;
global using Tbd.Shared.ApiResult;
global using Tbd.WebApi.Shared.ApiLogging;
global using Tbd.WebApi.Shared.CorrelationId;
global using Tbd.WebApi.Shared.Extensions;
global using Tbd.WebApi.Shared.Filters;
global using Tbd.WebApi.Shared.Repositories;
global using Tbd.WebApi.Shared.Swagger;
