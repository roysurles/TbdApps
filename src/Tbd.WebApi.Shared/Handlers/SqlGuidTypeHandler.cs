using System;
using System.Data;

using Dapper;

namespace RecipeApp.Shared.Handlers
{
    public class SqlGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override Guid Parse(object value) =>
            value is null ? Guid.Empty : Guid.Parse(value.ToString());

        public override void SetValue(IDbDataParameter parameter, Guid value) =>
            parameter.Value = value.ToString();
    }
}
