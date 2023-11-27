global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Net;
global using System.Threading;
global using System.Threading.Tasks;

global using FluentAssertions;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using Moq;

global using RecipeApp.CoreApi.Features.Ingredient.V1_0;
global using RecipeApp.CoreApi.Features.Instruction.V1_0;
global using RecipeApp.CoreApi.Features.Introduction.V1_0;
global using RecipeApp.Shared.Features.Ingredient;
global using RecipeApp.Shared.Features.Instruction;
global using RecipeApp.Shared.Features.Introduction;

global using Tbd.Shared.ApiResult;

global using Xunit;
