using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Dtos.Translate;
using SolaERP.Application.Entities.Language;
using SolaERP.Application.Entities.Translate;
using SolaERP.Application.UnitOfWork;
using SolaERP.DataAccess.Extensions;
using System.Data.Common;
using System.Data.SqlClient;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlLanguageRepository : ILanguageRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlLanguageRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckDuplicateTranslate(string languageCode, string key)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "Select dbo.SF_CheckDuplicateTranslate(@languageCode,@key) IsVerified";
                command.Parameters.AddWithValue(command, "@languageCode", languageCode);
                command.Parameters.AddWithValue(command, "@key", key);
                using var reader = await command.ExecuteReaderAsync();
                bool res = false;
                if (reader.Read())
                    res = reader.Get<bool>("IsVerified");

                return res;
            }
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

        public async Task<bool> SaveTranslateAsync(TranslateDto translate)
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
