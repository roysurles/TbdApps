global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;
global using System.IdentityModel.Tokens.Jwt;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Net.Http;
global using System.Net.Http.Json;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Threading;
global using System.Threading.Tasks;

global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
global using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

global using RecipeApp.Shared.Extensions;
global using RecipeApp.Shared.Features.Ingredient;
global using RecipeApp.Shared.Features.Instruction;
global using RecipeApp.Shared.Features.Introduction;
global using RecipeApp.Shared.Features.Session;
global using RecipeApp.Shared.Models;
global using RecipeApp.Shared.RefitEx;

global using Refit;

global using Tbd.Shared.ApiResult;
global using Tbd.Shared.Extensions;
global using Tbd.Shared.Pagination;
