global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Reflection;
global using System.Threading;
global using System.Threading.Tasks;

global using Asp.Versioning;

global using Dapper;

global using FluentValidation;
global using FluentValidation.AspNetCore;

global using HealthChecks.UI.Client;

global using MediatR;

global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.IdentityModel.Logging;

global using RecipeApp.CoreApi.Features.HealthChecks;
global using RecipeApp.CoreApi.Features.Ingredient.Cqrs;
global using RecipeApp.CoreApi.Features.Ingredient.V1_0;
global using RecipeApp.CoreApi.Features.Instruction.V1_0;
global using RecipeApp.CoreApi.Features.Introduction.V1_0;
global using RecipeApp.Database.Ef.RecipeDb;
global using RecipeApp.Shared.Features.Ingredient;
global using RecipeApp.Shared.Features.Instruction;
global using RecipeApp.Shared.Features.Introduction;
global using RecipeApp.Shared.Handlers;

global using Tbd.Shared.ApiLog;
global using Tbd.Shared.ApiResult;
global using Tbd.Shared.Extensions;
global using Tbd.Shared.Pagination;
global using Tbd.WebApi.Shared.ApiLogging;
global using Tbd.WebApi.Shared.Controllers;
global using Tbd.WebApi.Shared.CorrelationId;
global using Tbd.WebApi.Shared.Extensions;
global using Tbd.WebApi.Shared.Filters;
global using Tbd.WebApi.Shared.Repositories;
global using Tbd.WebApi.Shared.Services;
global using Tbd.WebApi.Shared.Swagger;
