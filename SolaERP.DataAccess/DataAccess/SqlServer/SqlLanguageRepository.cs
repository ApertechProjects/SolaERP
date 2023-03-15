using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Dtos.Language;
using SolaERP.Infrastructure.Dtos.Shared;
using SolaERP.Infrastructure.Dtos.Translate;
using SolaERP.Infrastructure.Entities.Account;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Entities.Language;
using SolaERP.Infrastructure.Entities.Translate;
using SolaERP.Infrastructure.Models;
using SolaERP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlLanguageRepository : ILanguageRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlLanguageRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteLanguageAsync(int id)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_Language_IUD @LanguageId";
                command.Parameters.AddWithValue(command, "@LanguageId", id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> DeleteTranslateAsync(int id)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = "SET NOCOUNT OFF EXEC SP_Translate_IUD @TranslateId";
                command.Parameters.AddWithValue(command, "@TranslateId", id);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<Language>> GetLanguagesLoadAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec [dbo].[SP_Language_Load]";
                using var reader = await command.ExecuteReaderAsync();

                List<Language> languages = new List<Language>();

                while (reader.Read())
                {
                    languages.Add(reader.GetByEntityStructure<Language>());
                }
                return languages;
            }
        }

        public async Task<List<Translate>> GetTranslatesLoadAsync()
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "select * from [dbo].[VW_Translate_List]";

                using var reader = await command.ExecuteReaderAsync();

                List<Translate> translates = new List<Translate>();

                while (await reader.ReadAsync())
                {
                    translates.Add(reader.GetByEntityStructure<Translate>());
                }
                return translates;
            }
        }

        public async Task<List<Translate>> GetTranslatesLoadByLanguageCodeAsync(string code)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "exec [dbo].[SP_Translate_Load_By_Code] @Code";
                command.Parameters.AddWithValue(command, "@Code", code);

                using var reader = await command.ExecuteReaderAsync();

                List<Translate> translates = new List<Translate>();

                while (reader.Read())
                {
                    translates.Add(reader.GetByEntityStructure<Translate>());
                }
                return translates;
            }
        }

        public async Task<bool> SaveLanguageAsync(Language language)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_Language_IUD    
                                                                @LanguageId,
                                                                @LanguageName,
                                                                @LanguageCode,
                                                                @IsActive"
                ;

                command.Parameters.AddWithValue(command, "@LanguageId", language.LanguageId);
                command.Parameters.AddWithValue(command, "@LanguageName", language.LanguageName);
                command.Parameters.AddWithValue(command, "@LanguageCode", language.LanguageCode);
                command.Parameters.AddWithValue(command, "@IsActive", language.IsActive);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> SaveTranslateAsync(Translate translate)
        {
            using (var command = _unitOfWork.CreateCommand() as SqlCommand)
            {
                command.CommandText = @"SET NOCOUNT OFF EXEC SP_Translate_IUD
                                                                @TranslateId,
                                                                @LanguageCode,
                                                                @Key,
                                                                @Text"
                ;

                command.Parameters.AddWithValue(command, "@TranslateId", translate.TranslateId);
                command.Parameters.AddWithValue(command, "@LanguageCode", translate.LanguageCode);
                command.Parameters.AddWithValue(command, "@Key", translate.Key);
                command.Parameters.AddWithValue(command, "@Text", translate.Text);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
