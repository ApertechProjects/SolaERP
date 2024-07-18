using SolaERP.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolaERP.DataAccess.Extensions;
using SolaERP.Application.Entities.ApproveStage;
using System.Data;
using SolaERP.Job.Enums;
using SolaERP.Application.Entities.Auth;
using SolaERP.Application.Entities.ApproveRole;
using SolaERP.Application.Entities.Procedure;
using Procedure = SolaERP.Job.Enums.Procedure;
using SolaERP.Application.Attributes;
using System.Reflection;
namespace SolaERP.Job
{
    public class Helper
    {
        private readonly IUnitOfWork _unitOfWork;
        public Helper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public enum IsSentValue
        {
            IsSent1 = 0,
            IsSent2 = 1,
            IsSent3 = 2,
        }
        public string GetRowInfoCommandString(IsSentValue isSentValue)
        {
            switch (isSentValue)
            {
                case IsSentValue.IsSent1:
                    return " AND IsSent = 0 ";
                case IsSentValue.IsSent2:
                    return " AND IsSent = 1 and IsSent2 = 0 and Sent2WillSend = @date";
                case IsSentValue.IsSent3:
                    return " AND IsSent = 1 and IsSent2 = 1 and IsSent3 = 0 and Sent3WillSend = @date";
                default:
                    return "";
            }
        }

        public string RowInfoCommandText()
        {
            var text = "select NS.SourceId,NotificationSenderId, ISNULL(ST.ApprovalStatusName,'In Approve Stage') ApproveStatus," +
                       "IsSent,IsSent2,IsSent3,NS.SourceId,RM.RequestNo,au.FullName Requester," +
                       "ISNULL(RM.RequestComment,'') Comment,P.ProcedureName,L.ActionId,L.Date LocalDateTime " +
                       "from Config.NotificationSender NS " +
                       "INNER JOIN Config.Procedures P ON NS.ProcedureId = P.ProcedureId " +
                       "INNER JOIN Register.Logs L ON L.SourceId = NS.SourceId " +
                       "INNER JOIN Register.LogTypes LT ON L.LogTypeId = LT.LogTypeId " +
                       "INNER JOIN Procurement.RequestMain RM ON NS.SourceId = RM.RequestMainId " +
                       "INNER JOIN Config.AppUser au on RM.Requester = au.Id " +
                       "LEFT JOIN Register.ApprovalStatus ST ON NS.ApproveStatusId = ST.ApprovalStatusId " +
                       $"where L.ActionId = 1  and NS.ProcedureId = @procedureId and NS.UserId = @userId and LT.LogTypeId = 7 and NS.ApproveStatusId!=99";
            return text;
        }

        public string RowInfoCommandTextForAssignedToBuyer()
        {
            var text = "select NS.SourceId,NotificationSenderId,case NS.ApproveStatusId when 99 THEN 'Assigned Buyer' END ApproveStatus," +
                 "IsSent,IsSent2,IsSent3,NS.SourceId,RM.RequestNo,au.FullName Requester," +
                 "ISNULL(RM.RequestComment,'') Comment,P.ProcedureName,L.ActionId,L.Date LocalDateTime " +
                 "from Config.NotificationSender NS " +
                 "INNER JOIN Config.Procedures P ON NS.ProcedureId = P.ProcedureId " +
                 "INNER JOIN Register.Logs L ON L.SourceId = NS.SourceId " +
                 "INNER JOIN Register.LogTypes LT ON L.LogTypeId = LT.LogTypeId " +
                 "INNER JOIN Procurement.RequestMain RM ON NS.SourceId = RM.RequestMainId " +
                 "INNER JOIN Config.AppUser au on RM.Requester = au.Id " +
                 $"where L.ActionId = 1  and NS.ProcedureId = @procedureId and NS.UserId = @userId and NS.ApproveStatusId = 99 and LT.LogTypeId = 7";
            return text;
        }


        public string UserCommandString(IsSentValue isSentValue)
        {
            switch (isSentValue)
            {
                case IsSentValue.IsSent1:
                    return " AND IsSent = 0 ";
                case IsSentValue.IsSent2:
                    return " AND IsSent = 1 and IsSent2 = 0";
                case IsSentValue.IsSent3:
                    return " AND IsSent = 1 and IsSent2 = 1 and IsSent3 = 0";
                default:
                    return "";
            }
        }

        public string UserCommandText()
        {
            var text = "select distinct UserId,AU.Email,AU.FullName,AU.Language from " +
                       "Config.NotificationSender NS INNER JOIN Config.AppUser AU ON NS.UserId = AU.Id " +
                       "where ProcedureId = @procedureId ";
            return text;
        }

        public HashSet<RowInfoDraft> GetRowInfosForIsSent(Procedure procedure, int userId, StatusType statusType)
        {
            StringBuilder stringBuilder = GetRowInfoCommandText(statusType);

            HashSet<RowInfoDraft> rows = new HashSet<RowInfoDraft>();

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = stringBuilder.Append(GetRowInfoCommandString(IsSentValue.IsSent1)).ToString();
                command.Parameters.AddWithValue(command, "@procedureId", procedure);
                command.Parameters.AddWithValue(command, "@userId", userId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rows.Add(GetFromReaderForRowInfo(reader));
                }
                return rows;
            }
        }

        private StringBuilder GetRowInfoCommandText(StatusType statusType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (statusType == StatusType.Other)
                stringBuilder.Append(RowInfoCommandText());
            else
                stringBuilder.Append(RowInfoCommandTextForAssignedToBuyer());

            return stringBuilder;
        }

        public HashSet<RowInfoDraft> GetRowInfosForIsSent2(Procedure procedure, int userId, StatusType statusType)
        {
            StringBuilder stringBuilder = GetRowInfoCommandText(statusType);

            HashSet<RowInfoDraft> rows = new HashSet<RowInfoDraft>();

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = stringBuilder.Append(GetRowInfoCommandString(IsSentValue.IsSent2)).ToString();

                command.Parameters.AddWithValue(command, "@procedureId", procedure);
                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@date", DateTime.Now.ToShortDateString());

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rows.Add(GetFromReaderForRowInfo(reader));
                }
                return rows;
            }
        }

        public HashSet<RowInfoDraft> GetRowInfosForIsSent3(Procedure procedure, int userId, StatusType statusType)
        {
            StringBuilder stringBuilder = GetRowInfoCommandText(statusType);

            HashSet<RowInfoDraft> rows = new HashSet<RowInfoDraft>();

            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = stringBuilder.Append(GetRowInfoCommandString(IsSentValue.IsSent2)).ToString();

                command.Parameters.AddWithValue(command, "@procedureId", procedure);
                command.Parameters.AddWithValue(command, "@userId", userId);
                command.Parameters.AddWithValue(command, "@date", DateTime.Now.ToShortDateString());

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rows.Add(GetFromReaderForRowInfo(reader));
                }
                return rows;
            }
        }

        public List<PersonDraft> GetUsersIsSent(Procedure procedure)
        {
            List<PersonDraft> users = new List<PersonDraft>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = UserCommandText() + UserCommandString(IsSentValue.IsSent1);
                command.Parameters.AddWithValue(command, "@procedureId", procedure);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(GetFromReaderForPerson(reader));
                }
                return users;
            }
        }

        public List<PersonDraft> GetUsersIsSent2(Procedure procedure)
        {
            List<PersonDraft> users = new List<PersonDraft>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = UserCommandText() + UserCommandString(IsSentValue.IsSent2);
                command.Parameters.AddWithValue(command, "@procedureId", procedure);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(GetFromReaderForPerson(reader));
                }
                return users;
            }
        }

        public List<PersonDraft> GetUsersIsSent3(Procedure procedure)
        {
            List<PersonDraft> users = new List<PersonDraft>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = UserCommandText() + UserCommandString(IsSentValue.IsSent3);
                command.Parameters.AddWithValue(command, "@procedureId", procedure);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(GetFromReaderForPerson(reader));
                }
                return users;
            }
        }

        private RowInfoDraft GetFromReaderForRowInfo(IDataReader reader)
        {
            return new RowInfoDraft
            {
                id = reader.Get<int>("SourceId"),
                notificationSenderId = reader.Get<int>("NotificationSenderId"),
                approveStatus = reader.Get<string>("ApproveStatus"),
                comment = reader.Get<string>("Comment"),
                localDateTime = reader.Get<DateTime>("LocalDateTime").ToString(),
                name = reader.Get<string>("Requester"),
                number = reader.Get<string>("RequestNo"),
                procedure = reader.Get<string>("ProcedureName"),
                reasonDescription = "",
                sequence = 1,

            };
        }

        private PersonDraft GetFromReaderForPerson(IDataReader reader)
        {
            return new PersonDraft
            {
                Email = reader.Get<string>("Email"),
                Language = reader.Get<string>("Language"),
                UserName = reader.Get<string>("FullName"),
                UserId = reader.Get<int>("UserId")
            };
        }

        public bool UpdateIsSent1(int[] ids)
        {
            var idsRes = string.Join(",", ids);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @$"set nocount off update Config.NotificationSender set IsSent = 1,Sent2WillSend = @sendDate where NotificationSenderId IN({idsRes})";
                command.Parameters.AddWithValue(command, "@ids", idsRes);
                command.Parameters.AddWithValue(command, "@sendDate", DateTime.Now.AddDays(3).Date);
                try
                {
                    var res = command.ExecuteNonQuery() > 0;
                    return res;
                }
                catch (Exception ex)
                {
                    return false;
                }


            }
        }

        public bool UpdateIsSent2(int[] ids)
        {
            var idsRes = string.Join(",", ids);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @$"set nocount off update Config.NotificationSender set IsSent2 = 1,Sent3WillSend = @sendDate where NotificationSenderId IN({idsRes})";
                command.Parameters.AddWithValue(command, "@ids", idsRes);
                command.Parameters.AddWithValue(command, "@sendDate", DateTime.Now.AddDays(3).ToShortDateString());
                try
                {
                    var res = command.ExecuteNonQuery() > 0;
                    return res;
                }
                catch (Exception ex)
                {
                    return false;
                }


            }
        }

        public bool UpdateIsSent3(int[] ids)
        {
            var idsRes = string.Join(",", ids);
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = @$"set nocount off update Config.NotificationSender set IsSent3 = 1 where NotificationSenderId IN({idsRes})";
                command.Parameters.AddWithValue(command, "@ids", idsRes);
                command.Parameters.AddWithValue(command, "@sendDate", DateTime.Now.AddDays(3).ToShortDateString());
                try
                {
                    var res = command.ExecuteNonQuery() > 0;
                    return res;
                }
                catch (Exception ex)
                {
                    return false;
                }


            }
        }

    }
}
