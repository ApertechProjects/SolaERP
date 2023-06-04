using SolaERP.Business.Constants;
using SolaERP.Business.Dtos.EntityDtos;
using SolaERP.Business.Dtos.EntityDtos.AdditionalPrivilege;
using SolaERP.Business.Dtos.EntityDtos.ApprovalStage;
using SolaERP.Business.Dtos.EntityDtos.ApproveRole;
using SolaERP.Business.Dtos.EntityDtos.ApproveStage;
using SolaERP.Business.Dtos.EntityDtos.Attachment;
using SolaERP.Business.Dtos.EntityDtos.BU;
using SolaERP.Business.Dtos.EntityDtos.DueDiligence;
using SolaERP.Business.Dtos.EntityDtos.General;
using SolaERP.Business.Dtos.EntityDtos.Group;
using SolaERP.Business.Dtos.EntityDtos.User;
using SolaERP.Business.Dtos.EntityDtos.UserMenu;
using SolaERP.Business.Dtos.EntityDtos.Vendor;
using SolaERP.Business.Dtos.GeneralDtos;
using SolaERP.Business.Dtos.Wrappers;
using SolaERP.Business.Models;
using SolaERP.Application.Contracts.Repositories;
using System.Data;

namespace SolaERP.Business.CommonLogic
{
    public class EntityLogic
    {
        private readonly IUserRepository _userRepository;
        public EntityLogic(ConfHelper confHelper, IUserRepository userRepository = null)
        {
            ConfHelper = confHelper;
            _userRepository = userRepository;
        }

        private ConfHelper ConfHelper { get; }

        public async Task<int> GetUserIdByToken(string token)
        {
            //try
            //{
            //    return (int)((await GetData.FromQuery($"SELECT dbo.SF_UserIdByToken('{token}')", ConfHelper.DevelopmentUrl)).Rows[0][0]);
            //}
            //catch
            //{
            //    return -1;
            //}
            return 1046;
        }



        public async Task<ApiResult> GetVendorList(string token, int BU)
        {
            if (await UserIsAuthorized(token))
            {
                int userId = await _userRepository.ConvertIdentity(token);
                var vendorList = new VendorListWrapper
                {
                    WFAVendor = (await GetData.FromQuery($"EXEC dbo.SP_VendorWFA {userId},{BU}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<VendorWFA>(),
                    AllVendor = (await GetData.FromQuery($"EXEC dbo.SP_VendorAll {userId},{BU}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<VendorAll>(),
                    ApproveStages = await GetApproveStage_Lists("VENDR"),
                    VendorDrafts = await GetVendorDraft(userId, BU)
                };

                #region Verion 1
                //if (!vendorList.WFAVendor.Any())
                //{
                //    vendorList.WFAVendor.Add(new VendorWFA { FullName = " " });
                //}
                //else
                //{
                //    var wfaData = vendorList.WFAVendor[0];
                //    for (int i = 1; i < 30; i++)
                //    {
                //        vendorList.WFAVendor.Add(new VendorWFA
                //        {
                //            VendorId = wfaData.VendorId + i,
                //            AgreeWithDefaultDays = wfaData.AgreeWithDefaultDays,
                //            CompanyLocation = wfaData.CompanyLocation,
                //            CompanyWebsite = wfaData.CompanyWebsite,
                //            CountryCode = wfaData.CountryCode,
                //            CreditDays = wfaData.CreditDays,
                //            ExpirationDate = wfaData.ExpirationDate,
                //            FullName = wfaData.FullName,
                //            Id = wfaData.Id,
                //            LastActivity = wfaData.LastActivity,
                //            PaymentTermsCode = wfaData.PaymentTermsCode,
                //            Position = wfaData.Position,
                //            RepresentedCompanies = wfaData.RepresentedCompanies,
                //            RepresentedProducts = wfaData.RepresentedProducts,
                //            Sequence = wfaData.Sequence,
                //            VendorName = wfaData.VendorName + i,
                //            VendorCode = wfaData.VendorCode + i

                //        });
                //    }

                //}
                #endregion

                return new ApiResult
                {
                    Data = vendorList,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }


        public async Task<ApiResult> GetBUList(string token)
        {
            if (await UserIsAuthorized(token))
            {
                var userId = await GetUserIdByToken(token);
                var BUList = new List<KeyValueTextBoxingDto>();
                DataTable BU_ListDT = await GetData.FromQuery($"EXEC SP_BusinessUnitsList " + userId, ConfHelper.DevelopmentUrl);

                for (int i = 0; i < BU_ListDT.Rows.Count; i++)
                {
                    var rowData = BU_ListDT.Rows[i];
                    BUList.Add(new KeyValueTextBoxingDto
                    {
                        Key = i.ToString(),
                        Value = $"{rowData["BusinessUnitId"]}",
                        Text = $"{rowData["BusinessUnitCode"]} - {rowData["BusinessUnitName"]}"
                    });
                }
                return new ApiResult { Data = BUList, OperationIsSuccess = true, IsAuthorized = true };
            }
            return UnAuthorizedUserResult();
        }

        public async Task<bool> UserIsAuthorized(string userToken)
        {
            return true;
            if (string.IsNullOrEmpty(userToken)) return false;

            int userId = await new EntityLogic(ConfHelper).GetUserIdByToken(userToken);
            if (userId < 0) return false;

            return true;

        }


        public ApiResult UnAuthorizedUserResult()
        {
            ApiResult apiResult = new ApiResult();
            apiResult.ErrorList.Add("User token is not valid.Please,Login Application!");
            return apiResult;
        }

        public async Task<ApiResult> GetVendorDetails(string token, int vendorId)
        {

            if (await UserIsAuthorized(token))
            {
                VendorDetailWrapper vendorDetailWrapper = new();
                vendorDetailWrapper.VendorLoad = (await GetData.FromQuery($"EXEC dbo.SP_Vendor_Load {vendorId}", ConfHelper.DevelopmentUrl)).ConvertToClassModel<Vendor_Load>();
                vendorDetailWrapper.VendorLoad.LOGO = (await GetData.FromQuery($"EXEC dbo.SP_AttachmentList_Load  {vendorId},'','{Constants.VendorConstants._VEN_LOGO}'", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<AttachmentList_Load>();
                if (vendorDetailWrapper.VendorLoad.LOGO.Any())
                {
                    vendorDetailWrapper.VendorLoad.LOGO[0].FileData = ((await GetData.FromQuery($"EXEC dbo.SP_Attachment_Load {vendorDetailWrapper.VendorLoad.LOGO[0].AttachmentId}", ConfHelper.DevelopmentUrl)).ConvertToClassModel<Attachment_Load>()).FileData;
                }

                vendorDetailWrapper.VendorLoad.OLET = (await GetData.FromQuery($"EXEC dbo.SP_AttachmentList_Load  {vendorId},'','{Constants.VendorConstants._VEN_OLET}'", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<AttachmentList_Load>();
                var vendorOLET = vendorDetailWrapper.VendorLoad.OLET;
                if (vendorOLET.Any())
                {
                    foreach (var item in vendorOLET)
                    {
                        item.FileData = ((await GetData.FromQuery($"EXEC dbo.SP_Attachment_Load {item.AttachmentId}", ConfHelper.DevelopmentUrl)).ConvertToClassModel<Attachment_Load>()).FileData;
                    }
                }

                vendorDetailWrapper.VendorLoad.PrequalificationCategories = (await GetData.FromQuery($"SELECT * FROM dbo.VW_PrequalificationCategoryList", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<PrequalificationCategory>();
                vendorDetailWrapper.VendorLoad.VendorProductServices = (await GetData.FromQuery($"SELECT * FROM dbo.VW_ProductService_List", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<VendorProductServiceCls>();

                DataTable VPSIds = await GetData.FromQuery($"EXEC SP_VendorProductServices_Load  " + vendorId, ConfHelper.DevelopmentUrl);
                vendorDetailWrapper.VendorLoad.VendorProductServiceIds = VPSIds.AsEnumerable().Select(m => m.Field<int>("ProductServiceId")).ToList();
                vendorDetailWrapper.VendorLoad.VendorProductServices.ForEach(m =>
                {
                    if (vendorDetailWrapper.VendorLoad.VendorProductServiceIds.Any(s => s == m.ProductServiceId))
                    {
                        m.IsSelected = true;
                    }
                });
                vendorDetailWrapper.VendorLoad.SelectedVendorProductServices = string.Join(',', vendorDetailWrapper.VendorLoad.VendorProductServices.Where(m => m.IsSelected).Select(m => m.ProductServiceName).ToList());
                DataTable VPCIds = await GetData.FromQuery($"EXEC SP_VendorPrequalificationCategory_Load " + vendorId, ConfHelper.DevelopmentUrl);
                vendorDetailWrapper.VendorLoad.PrequalificationCategorieIds = VPCIds.AsEnumerable().Select(m => m.Field<int>("PrequalificationCategoryId")).ToList();

                vendorDetailWrapper.VendorLoad.PrequalificationCategories.ForEach(m =>
                {
                    if (vendorDetailWrapper.VendorLoad.PrequalificationCategorieIds.Any(s => s == m.PrequalificationCategoryId))
                    {
                        m.IsSelected = true;
                    }
                });
                vendorDetailWrapper.VendorLoad.SelectedPrequalificationCategories = string.Join(',', vendorDetailWrapper.VendorLoad.PrequalificationCategories.Where(m => m.IsSelected).Select(m => m.PrequalificationCategoryName).ToList());

                vendorDetailWrapper.VendorBankLoad = (await GetData.FromQuery($"EXEC dbo.SP_VendorBank_Load {vendorId}", ConfHelper.DevelopmentUrl)).ConvertToClassModel<VendorBank_Load>();
                vendorDetailWrapper.VendorBankLoad.BNK = (await GetData.FromQuery($"EXEC dbo.SP_AttachmentList_Load  {vendorId},'','{Constants.VendorConstants._VEN_BNK}'", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<AttachmentList_Load>();

                vendorDetailWrapper.VendorEvaluation_Load = (await GetData.FromQuery($"EXEC dbo.SP_VendorEvaluation_Load {vendorId}", ConfHelper.DevelopmentUrl)).ConvertToClassModel<VendorEvaluation_Load>();
                vendorDetailWrapper.VendorEvaluation_Load.ISO = (await GetData.FromQuery($"EXEC dbo.SP_AttachmentList_Load  {vendorId},'','{Constants.VendorConstants._VEN_ISO}'", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<AttachmentList_Load>();
                vendorDetailWrapper.VendorEvaluation_Load.OTH = (await GetData.FromQuery($"EXEC dbo.SP_AttachmentList_Load  {vendorId},'','{Constants.VendorConstants._VEN_OTH}'", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<AttachmentList_Load>();
                return new ApiResult
                {
                    Data = vendorDetailWrapper,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }


        #region Done
        //DONE
        internal async Task<ApiResult> GetUserMenu_Load(string token)
        {
            if (true)//await UserIsAuthorized(token))
            {
                int userId = await GetUserIdByToken(token);
                var result = new GenericMapLogic<UserMenu_Load>().BuildModel(await GetData.FromQuery($"EXEC dbo.SP_UserMenu_Load {userId}", ConfHelper.DevelopmentUrl));

                return result;
            }
            return UnAuthorizedUserResult();
        }
        //DONE
        internal async Task<ApiResult> GetUserMenuWithoutAccess(string token)
        {
            if (await UserIsAuthorized(token))
            {
                var random = new Random();
                var menuDataResult = new List<ParentMenu>();
                int userId = await GetUserIdByToken(token);

                var menuDatas = (await GetData.FromQuery($"EXEC dbo.SP_UserMenu_Load {userId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<UserMenu_Load>();

                var parentMenus = menuDatas.Where(m => m.ParentId == 0).ToList();
                foreach (var parentMenu in parentMenus)
                {
                    var menuItemResult = new ParentMenu()
                    {
                        Icon = parentMenu.Icon,
                        Id = parentMenu.MenuId,
                        Link = parentMenu.URL,
                        ParentMenuName = parentMenu.MenuName,
                        ReactIcon = parentMenu.ReactIcon
                    };
                    var submenus = menuDatas.Where(m => m.ParentId == parentMenu.MenuId).ToList();
                    foreach (var subMenu in submenus)
                    {
                        menuItemResult.SubMenus.Add(new()
                        {
                            Icon = subMenu.Icon,
                            Id = subMenu.MenuId,
                            ParentMenuId = parentMenu.MenuId,
                            SubLink = subMenu.URL,
                            SubMenuName = subMenu.MenuName,
                            ReactIcon = subMenu.ReactIcon
                        });
                    }
                    menuDataResult.Add(menuItemResult);

                }

                return new ApiResult()
                {
                    Data = menuDataResult,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }
        #endregion



        internal async Task<ApiResult> GetGroups(string token)
        {
            if (await UserIsAuthorized(token))
            {
                var result = new GenericMapLogic<GroupMain>().BuildModel(await GetData.FromQuery($"EXEC dbo.SP_GroupMain_Load", ConfHelper.DevelopmentUrl));
                return result;
            }
            return UnAuthorizedUserResult();
        }



        internal async Task<ApiResult> DownloadFile(string token, int fileId)
        {
            if (await UserIsAuthorized(token))
            {

                var fileData = (await GetData.FromQuery($"EXEC dbo.SP_Attachment_Load {fileId}", ConfHelper.DevelopmentUrl)).ConvertToClassModel<Attachment_Load>();

                return new ApiResult()
                {
                    Data = fileData,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }
        internal async Task<ApiResult> DeleteFile(string token, int fileId)
        {
            if (await UserIsAuthorized(token))
            {

                _ = await GetData.FromQuery($"EXEC dbo.SP_Attachments_IUD {fileId}", ConfHelper.DevelopmentUrl);

                return new ApiResult()
                {
                    Data = ResultMessageConstans._OPERATION_SUCCESSFUL,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> VendorSendToApprove(string token, VendorSendToApprove vendorSendToApproves)
        {
            if (await UserIsAuthorized(token))
            {
                if (vendorSendToApproves is null || !vendorSendToApproves.VendorsId.Any() || vendorSendToApproves.VendorsId is null || vendorSendToApproves.VendorsId.Any(m => m == 0))
                {
                    return new ApiResult()
                    {
                        Data = ResultMessageConstans._OPERATION_FAIL,
                        IsAuthorized = true,
                        OperationIsSuccess = false
                    };
                }

                foreach (var vendorId in vendorSendToApproves.VendorsId)
                    _ = await GetData.FromQuery($"EXEC dbo.SP_VendorSendToApprove {vendorId},{vendorSendToApproves.ApproveStageMainId}", ConfHelper.DevelopmentUrl);

                return new ApiResult()
                {
                    Data = ResultMessageConstans._OPERATION_SUCCESSFUL,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> SaveFile(string token, SaveFileWrapper files)
        {
            if (await UserIsAuthorized(token))
            {
            }
            return UnAuthorizedUserResult();

        }

        public async Task<ApiResult> VendorApprove(string token, List<VendorWFAModel> vm)
        {
            if (await UserIsAuthorized(token))
            {
                int userId = await GetUserIdByToken(token);

                foreach (var vmi in vm)
                    _ = await GetData.FromQuery($"EXEC dbo.SP_VendorApprove {vmi.VendorId},{userId},{vmi.ApproveStatus},'{vmi.Comment}',{vmi.Sequence}", ConfHelper.DevelopmentUrl);

                return new ApiResult()
                {
                    Data = ResultMessageConstans._OPERATION_SUCCESSFUL,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }

        public async Task<List<VendorDraft>> GetVendorDraft(int userId, int BU)
        {
            var vendorDraftList = (await GetData.FromQuery($"EXEC dbo.SP_VendorDraft {userId},{BU}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<VendorDraft>();
            return vendorDraftList;
        }

        public async Task<List<ApproveStage_List>> GetApproveStage_Lists(string procKey)
        {
            var res = (await GetData.FromQuery($"EXEC dbo.SP_ApproveStages_List '{procKey}'", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<ApproveStage_List>();
            return res;
        }

        public async Task<VendorDueDiligence> GetVendorDueDiligence(int vendorId)
        {
            return (await GetData.FromQuery($"EXEC SP_VendorDueDiligence_Load " + vendorId, ConfHelper.DevelopmentUrl)).ConvertToClassModel<VendorDueDiligence>();
        }


        public async Task<ApiResult> GetDueDiligence(string token, int vendorId)
        {
            if (await UserIsAuthorized(token))
            {
                List<DueDiligenceWrapper> dueDiligenceResult = await GetDueDiligenceResult();
                return new ApiResult()
                {
                    Data = dueDiligenceResult,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();

            List<DueDiligenceWrapper> formatData(List<DueDiligence> paramData)
            {

                List<DueDiligenceWrapper> values = new();

                var titleList = paramData.Select(m => m.Title).Distinct();

                foreach (var title in titleList)
                {
                    values.Add(new DueDiligenceWrapper { Title = title, Data = paramData.Where(m => m.Title == title).ToList() });
                }
                return values;
            }

            async Task<List<DueDiligenceWrapper>> GetDueDiligenceResult()
            {
                var dueDiligence = (await GetData.FromQuery($"EXEC dbo.SP_DueDiligenceDesign_Load", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<DueDiligence>();
                decimal weightSum = dueDiligence.Sum(m => m.Weight);
                var dueDatas = await GetVendorDueDiligence(vendorId);
                foreach (var m in dueDiligence)
                {
                    m.PercentWeight = string.Format("{0:0.##}", (m.Weight / weightSum) * 100);
                    m.Values = dueDatas;
                    m.Attachments = (await GetData.FromQuery($"EXEC dbo.SP_AttachmentList_Load {m.DueDiligenceDesignId},'','{Constants.VendorConstants._VEN_DUE}'", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<AttachmentList_Load>();
                }

                var formattedResult = formatData(dueDiligence);
                return formattedResult;
            }
        }

        public async Task<ApiResult> GetUserList(string token)
        {
            if (await UserIsAuthorized(token))
            {
                var userList = (await GetData.FromQuery("SELECT * FROM dbo.VW_Users_List", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<User>();
                return new ApiResult()
                {
                    Data = userList,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }

        public async Task<ApiResult> GetUserListForGroup(string token, int groupId)
        {
            if (await UserIsAuthorized(token))
            {
                var userList = (await GetData.FromQuery("SELECT * FROM dbo.VW_Users_List", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<User>();
                if (groupId > 0)
                {
                    var selectedUsers = (await GetData.FromQuery($"EXEC dbo.SP_GroupUsers_Load {groupId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<GroupUser>();
                    selectedUsers.ForEach(m =>
                    {
                        var existUser = userList.Where(u => u.Id == m.Id).Single();
                        existUser.IsSelected = true;
                    });
                }
                return new ApiResult()
                {
                    Data = userList,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }


        internal async Task<ApiResult> GetApproveRolesByGroupId(string token, int groupId)
        {
            if (await UserIsAuthorized(token))
            {

                var approveList = (await GetData.FromQuery("SELECT * FROM dbo.VW_ApproveRoles_List", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<ApproveRole>();
                if (groupId > 0)
                {
                    var selectedApproveRoles = (await GetData.FromQuery($"EXEC dbo.SP_GroupApproveRoles_Load {groupId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<GroupApproveRole>();
                    selectedApproveRoles.ForEach(m =>
                    {
                        var existApproveRole = approveList.Where(u => u.ApproveRoleId == m.ApproveRoleId).Single();
                        existApproveRole.IsSelected = true;
                    });
                }

                return new ApiResult()
                {
                    Data = approveList,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetGroupAdditionalPrivileges(string token, int groupId)
        {
            if (await UserIsAuthorized(token))
            {
                var additionalPrivileges = (await GetData.FromQuery($"EXEC dbo.SP_GroupAdditionalPrivileges_Load {groupId}", ConfHelper.DevelopmentUrl)).ConvertToClassModel<AdditionalPrivilegeGroup>();

                return new ApiResult()
                {
                    Data = additionalPrivileges,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };

            }
            return UnAuthorizedUserResult();

        }


        internal async Task<ApiResult> SaveGroup(string token, GroupSaveWrapper gs)
        {
            if (gs != null)
            {
                if (await UserIsAuthorized(token))
                {
                    int userId = await GetUserIdByToken(token);

                    _ = await GetData.FromQuery($"EXEC SP_Groups_IUD {gs.GroupId},'{gs.GroupName}','{gs.Description}',{userId}", ConfHelper.DevelopmentUrl);

                    if (gs.GroupId == 0)
                        gs.GroupId = await Utility.GetMaxId("Config.Groups", ConfHelper);

                    if (gs.Users != null)
                    {
                        _ = await GetData.FromQuery($"EXEC SP_GroupUsers_ID  {gs.GroupId}", ConfHelper.DevelopmentUrl);
                        for (int i = 0; i < gs.Users.Count; i++)
                        {
                            _ = await GetData.FromQuery($"EXEC SP_GroupUsers_ID  {gs.GroupId},{gs.Users[i]}", ConfHelper.DevelopmentUrl);
                            //What happens if user ads its self in our situtation?
                        }
                    }

                    if (gs.BusinessUnitIds != null)
                    {
                        _ = await GetData.FromQuery($"EXEC SP_GroupBusinessUnits_ID {gs.GroupId}", ConfHelper.DevelopmentUrl);
                        for (int i = 0; i < gs.BusinessUnitIds.Count; i++)
                        {
                            _ = await GetData.FromQuery($"EXEC SP_GroupBusinessUnits_ID {gs.GroupId},{gs.BusinessUnitIds[i]}", ConfHelper.DevelopmentUrl);
                        }
                    }

                    if (gs.ApproveRoles != null)
                    {
                        _ = await GetData.FromQuery($"EXEC SP_GroupApproveRoles_ID {gs.GroupId}", ConfHelper.DevelopmentUrl);
                        for (int i = 0; i < gs.ApproveRoles.Count; i++)
                        {
                            _ = await GetData.FromQuery($"EXEC SP_GroupApproveRoles_ID {gs.GroupId},{gs.ApproveRoles[i]}", ConfHelper.DevelopmentUrl);
                        }
                    }

                    if (gs.AdditionalPrivilege != null)
                    {
                        _ = await GetData.FromQuery($"EXEC SP_GroupAdditionalPrivileges_IUD  {gs.AdditionalPrivilege.GroupAdditionalPrivilegeId}", ConfHelper.DevelopmentUrl);

                        _ = await GetData.FromQuery($"EXEC SP_GroupAdditionalPrivileges_IUD  {gs.AdditionalPrivilege.GroupAdditionalPrivilegeId},{gs.GroupId},{Convert.ToInt16(gs.AdditionalPrivilege.VendorDraft)}", ConfHelper.DevelopmentUrl);

                    }


                    if (gs.Menus != null)
                    {

                        var MenuIds = gs.Menus.GetMenuIds();
                        _ = await GetData.FromQuery($"EXEC SP_GroupMenus_ID " +
                                $"{gs.GroupId}",
                                ConfHelper.DevelopmentUrl);
                        for (int i = 0; i < MenuIds.Count; i++)
                        {
                            _ = await GetData.FromQuery($"EXEC SP_GroupMenus_ID " +
                                $"{gs.GroupId}," +
                                $"{MenuIds[i]}," +
                                $"{V("Create", MenuIds[i])}," +
                                $"{V("Edit", MenuIds[i])}," +
                                $"{V("Delete", MenuIds[i])}," +
                                $"{V("Export", MenuIds[i])}",
                                ConfHelper.DevelopmentUrl);
                        }


                        int V(string accessName, int menuId)
                        {
                            switch (accessName)
                            {
                                case "Create":
                                    return Convert.ToInt16(gs.Menus.Create.Contains(menuId));
                                case "Edit":
                                    return Convert.ToInt16(gs.Menus.Edit.Contains(menuId));
                                case "Delete":
                                    return Convert.ToInt16(gs.Menus.Delete.Contains(menuId));
                                default:
                                    return Convert.ToInt16(gs.Menus.Export.Contains(menuId));
                            }
                        }
                    }



                    return new ApiResult
                    {
                        Data = ResultMessageConstans._OPERATION_SUCCESSFUL,
                        OperationIsSuccess = true,
                        IsAuthorized = true
                    };
                }
                return UnAuthorizedUserResult();
            }
            else
            {
                return new ApiResult
                {
                    Data = ResultMessageConstans._OPERATION_FAIL,
                    OperationIsSuccess = true,
                    IsAuthorized = true
                };
            }
        }

        internal async Task<ApiResult> GetUserMenu_LoadForGroup(string token, int groupId)
        {
            if (await UserIsAuthorized(token))
            {
                int userId = 1407;//await GetUserIdByToken(token);
                var result = new ApiResult();
                var userMenuResult = (await GetData.FromQuery($"EXEC dbo.SP_UserMenu_Load {userId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<UserMenu_Load>();
                var wrapperModel = new GroupMenuExternalDataWrapper()
                {
                    MenuItems = userMenuResult,
                    SelectedMenuList = new MenuGroupWRP()
                };
                result.Data = wrapperModel;
                result.IsAuthorized = result.OperationIsSuccess = true;
                if (groupId > 0)
                {
                    var menuResult = (await GetData.FromQuery($"EXEC dbo.SP_GroupMenus_Load {groupId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<GroupMenu>();
                    foreach (var savedMenu in userMenuResult)
                    {
                        var compareMenu = menuResult.Where(m => m.MenuId == savedMenu.MenuId).FirstOrDefault();
                        if (compareMenu is not null)
                        {
                            savedMenu.CreateAccess = Convert.ToBoolean(compareMenu.CreateAccess);
                            savedMenu.EditAccess = Convert.ToBoolean(compareMenu.EditAccess);
                            savedMenu.DeleteAccess = Convert.ToBoolean(compareMenu.DeleteAccess);
                            savedMenu.ExportAccess = Convert.ToBoolean(compareMenu.ExportAccess);
                            if (savedMenu.CreateAccess)
                                wrapperModel.SelectedMenuList.Create.Add(compareMenu.MenuId);
                            if (savedMenu.EditAccess)
                                wrapperModel.SelectedMenuList.Edit.Add(compareMenu.MenuId);
                            if (savedMenu.DeleteAccess)
                                wrapperModel.SelectedMenuList.Delete.Add(compareMenu.MenuId);
                            if (savedMenu.ExportAccess)
                                wrapperModel.SelectedMenuList.Export.Add(compareMenu.MenuId);

                        }
                    }
                }
                else
                {
                    foreach (var savedMenu in wrapperModel.MenuItems)
                    {
                        savedMenu.CreateAccess =
                        savedMenu.EditAccess =
                        savedMenu.DeleteAccess =
                        savedMenu.ExportAccess = false;
                    }
                }
                return result;
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetBUListForGroup(string token, int groupId)
        {
            if (await UserIsAuthorized(token))
            {
                var BUList = (List<KeyValueTextBoxingDto>)(await GetBUList(token)).Data;

                if (groupId > 0)
                {
                    List<BusinessUnitGroup> selectedBUList = (await GetData.FromQuery($"EXEC dbo.SP_GroupBusinessUnit_Load  {groupId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<BusinessUnitGroup>();
                    selectedBUList.ForEach(m =>
                    {
                        var selectedBaseItem = BUList.Where(b => b.Value == m.BusinessUnitId.ToString()).First();
                        selectedBaseItem.IsSelected = true;
                    });
                }
                return new ApiResult { Data = BUList, OperationIsSuccess = true, IsAuthorized = true };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetApprovalStageLoad(string token, int buId)
        {
            if (await UserIsAuthorized(token))
            {
                var approvalResult = (await GetData.FromQuery($"EXEC dbo.SP_ApproveStageMain_Load {buId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<ApproveStageMain_Load>();

                return new ApiResult
                {
                    Data = approvalResult,
                    OperationIsSuccess = true,
                    IsAuthorized = true,
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetApprovalStageHeaderLoad(string token, int approvalStageMainId)
        {
            if (await UserIsAuthorized(token))
            {
                var approvalHeaderResult = (await GetData.FromQuery($"EXEC dbo.SP_ApproveStageHeader_Load {approvalStageMainId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<ApproveStageHeader_Load>();

                return new ApiResult
                {
                    Data = approvalHeaderResult,
                    OperationIsSuccess = true,
                    IsAuthorized = true,
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetApprovalStageDetailsLoad(string token, int approvalStageMainId)
        {
            if (await UserIsAuthorized(token))
            {
                var approvalDetailResult = (await GetData.FromQuery($"EXEC dbo.SP_ApproveStageDetails_Load {approvalStageMainId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<ApproveStageDetails_Load>();

                return new ApiResult
                {
                    Data = approvalDetailResult,
                    OperationIsSuccess = true,
                    IsAuthorized = true,
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetApprovalStageRolesLoad(string token, int approvalStageDetailId)
        {
            if (await UserIsAuthorized(token))
            {
                var approvalRolesResult = (await GetData.FromQuery($"EXEC dbo.SP_ApproveStageRoles_Load {approvalStageDetailId}", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<ApprovalStageRoles_Load>();

                return new ApiResult
                {
                    Data = approvalRolesResult,
                    OperationIsSuccess = true,
                    IsAuthorized = true,
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetBusinessUnitList(string token)
        {
            if (await UserIsAuthorized(token))
            {
                var businessUnitResult = (await GetData.FromQuery($"Select * from dbo.VW_BusinessUnits_List", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<BusinessUnit>();

                return new ApiResult
                {
                    Data = businessUnitResult,
                    OperationIsSuccess = true,
                    IsAuthorized = true,
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetApproveRoles(string token)
        {
            if (await UserIsAuthorized(token))
            {
                var approveList = (await GetData.FromQuery("SELECT * FROM dbo.VW_ApproveRoles_List", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<ApproveRole>();

                return new ApiResult()
                {
                    Data = approveList,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> GetProceduresList(string token)
        {
            if (await UserIsAuthorized(token))
            {
                var proceduresList = (await GetData.FromQuery("SELECT * FROM dbo.VW_Procedures_List", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<Procedures_List>();

                return new ApiResult()
                {
                    Data = proceduresList,
                    IsAuthorized = true,
                    OperationIsSuccess = true
                };
            }
            return UnAuthorizedUserResult();
        }

        internal async Task<ApiResult> DeleteGroups(string token, int groupId)
        {
            if (await UserIsAuthorized(token))
            {
                if (await UserIsAuthorized(token))
                {
                    _ = await GetData.FromQuery($"EXEC SP_Groups_IUD {groupId}", ConfHelper.DevelopmentUrl);

                    return new ApiResult()
                    {
                        Data = ResultMessageConstans._OPERATION_SUCCESSFUL,
                        IsAuthorized = true,
                        OperationIsSuccess = true
                    };
                }
            }
            return UnAuthorizedUserResult();

            //internal async Task<ApiResult> GetAdditionalPrivileges(string token)
            //{
            //    if (await UserIsAuthorized(token))
            //    {
            //        var userList = (await GetData.FromQuery("SELECT * FROM dbo.VW_ApproveRoles_List", ConfHelper.DevelopmentUrl)).ConvertToClassListModel<ApproveRole>();
            //        return new ApiResult()
            //        {
            //            Data = userList,
            //            IsAuthorized = true,
            //            OperationIsSuccess = true
            //        };
            //    }
            //    return UnAuthorizedUserResult();
            //}
        }

        internal async Task<ApiResult> SaveApprovalStage(string token, SaveApprovalStageWrapper saveApprovalStageWrapper)
        {
            if (await UserIsAuthorized(token))
            {
                int approveStageMainId = saveApprovalStageWrapper.ApproveStageHeader_Load.ApproveStageMainId;
                int approveStageDetailId = 0;
                int userId = await GetUserIdByToken(token);
                await GetData.FromQuery($"exec SP_ApproveStagesMain_IUD " +
                    $"{saveApprovalStageWrapper.ApproveStageHeader_Load.ApproveStageMainId}," +
                    $"{saveApprovalStageWrapper.ApproveStageHeader_Load.ProcedureId}," +
                    $"{saveApprovalStageWrapper.ApproveStageHeader_Load.BusinessUnitId}," +
                    $"'{saveApprovalStageWrapper.ApproveStageHeader_Load.ApproveStageName}'," +
                    $"{userId}", ConfHelper.DevelopmentUrl);

                for (int i = 0; i < saveApprovalStageWrapper.ApproveStageDetails_Load.Count; i++)
                {
                    if (saveApprovalStageWrapper.ApproveStageDetails_Load[i].Type == "remove")
                    {
                        await GetData.FromQuery($"exec SP_ApproveStagesDetails_IUD " +
                          $"{saveApprovalStageWrapper.ApproveStageDetails_Load[i].ApproveStageDetailsId}"
                           , ConfHelper.DevelopmentUrl);
                    }
                }

                if (saveApprovalStageWrapper.ApproveStageHeader_Load.ApproveStageMainId == 0)
                    approveStageMainId = await Utility.GetMaxId("Config.ApproveStagesMain", ConfHelper);

                for (int i = 0; i < saveApprovalStageWrapper.ApproveStageDetails_Load.Count; i++)
                {

                    await GetData.FromQuery($"exec SP_ApproveStagesDetails_IUD " +
                      $"{saveApprovalStageWrapper.ApproveStageDetails_Load[i].ApproveStageDetailsId}," +
                      $"{approveStageMainId}," +
                      $"'{saveApprovalStageWrapper.ApproveStageDetails_Load[i].ApproveStageDetailsName}'," +
                      $"{saveApprovalStageWrapper.ApproveStageDetails_Load[i].Sequence}"
                       , ConfHelper.DevelopmentUrl);


                    if (saveApprovalStageWrapper.ApproveStageHeader_Load.ApproveStageMainId == 0)
                        approveStageDetailId = await Utility.GetMaxId("Config.ApproveStagesDetail", ConfHelper);

                    for (int j = 0; j < saveApprovalStageWrapper.ApproveStageDetails_Load[i].approvalStageRoles_Loads.Count; j++)
                    {
                        await GetData.FromQuery($"exec SP_ApproveStageRoles_ID " +
                     $"{approveStageDetailId}," +
                     $"{saveApprovalStageWrapper.ApproveStageDetails_Load[i].approvalStageRoles_Loads[j].ApproveRoleId}," +
                     $"'{saveApprovalStageWrapper.ApproveStageDetails_Load[i].approvalStageRoles_Loads[j].AmountFrom}'," +
                     $"{saveApprovalStageWrapper.ApproveStageDetails_Load[i].approvalStageRoles_Loads[j].AmountTo}"
                      , ConfHelper.DevelopmentUrl);
                    }
                }

                return new ApiResult
                {
                    Data = ResultMessageConstans._OPERATION_SUCCESSFUL,
                    OperationIsSuccess = true,
                    IsAuthorized = true
                };

            }
            return UnAuthorizedUserResult();
        }


        internal async Task<ApiResult> DeleteApprovalDetail(string token, int approveStageDetailsId)
        {
            if (await UserIsAuthorized(token))
            {
                if (approveStageDetailsId > 0)
                {
                    await GetData.FromQuery($"exec SP_ApproveStagesDetails_IUD " +
                      $"{approveStageDetailsId}"
                       , ConfHelper.DevelopmentUrl);

                    return new ApiResult
                    {
                        Data = ResultMessageConstans._OPERATION_SUCCESSFUL,
                        OperationIsSuccess = true,
                        IsAuthorized = true
                    };

                }

            }
            return UnAuthorizedUserResult();
        }

        public async Task<ApiResult> GetActiveVendors(string authToken, int businessUnitId)
        {
            int userId = await _userRepository.ConvertIdentity(authToken);
            if (await UserIsAuthorized(authToken))
            {
                var data = await GetData.FromQuery($"exec SP_VendorList {userId},{businessUnitId}"
                    , ConfHelper.DevelopmentUrl);


                return new ApiResult
                {
                    Data = data.ConvertToClassListModel<VendorList>(),
                    OperationIsSuccess = true,
                    IsAuthorized = true
                };

            }
            return UnAuthorizedUserResult();

        }
    }
}
