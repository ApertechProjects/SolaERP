using SolaERP.Application.Entities;
using SolaERP.Application.Models;
using System.Data;

namespace SolaERP.Application.Contracts.Common
{
    public interface IBaseRepository
    {
        string ReplaceQuery(string sqlElement, ReplaceParams paramListReplaced);
    }
}
