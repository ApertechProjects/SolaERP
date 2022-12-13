using Microsoft.Extensions.Configuration;
using SolaERP.Infrastructure.Contracts.Services;
using System.Net;
using System.Net.Mail;

namespace SolaERP.Application.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Email"], "Apertech", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Email"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string templatePath)
        {
            using StreamReader reader = new StreamReader(templatePath);
            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_configuration["Mail:Username"]);

                    smtpClient.Host = _configuration["Mail:Host"];
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;

                    message.From = fromAddress;
                    message.Subject = "Email Verification for Reset Password";
                    message.IsBodyHtml = true;

                    #region Test 
                    //					message.Body = @"<!DOCTYPE html>

                    //<html lang=""en"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:v=""urn:schemas-microsoft-com:vml"">

                    //<head>
                    //	<title></title>
                    //	<meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"" />
                    //	<meta content=""width=device-width, initial-scale=1.0"" name=""viewport"" />
                    //	<link href=""https://fonts.googleapis.com/css?family=Open+Sans"" rel=""stylesheet"" type=""text/css"" />
                    //	<link href=""https://fonts.googleapis.com/css?family=Cabin"" rel=""stylesheet"" type=""text/css"" />
                    //	<style>
                    //		* {
                    //			box-sizing: border-box;
                    //		}

                    //		body {
                    //			margin: 0;
                    //			padding: 0;
                    //		}

                    //		a[x-apple-data-detectors] {
                    //			color: inherit !important;
                    //			text-decoration: inherit !important;
                    //		}

                    //		#MessageViewBody a {
                    //			color: inherit;
                    //			text-decoration: none;
                    //		}

                    //		p {
                    //			line-height: inherit
                    //		}

                    //		.desktop_hide,
                    //		.desktop_hide table {
                    //			/* mso-hide: all; */
                    //			display: none;
                    //			max-height: 0px;
                    //			overflow: hidden;
                    //		}

                    //		@media (max-width:620px) {

                    //			.desktop_hide table.icons-inner,
                    //			.social_block.desktop_hide .social-table {
                    //				display: inline-block !important;
                    //			}

                    //			.icons-inner {
                    //				text-align: center;
                    //			}

                    //			.icons-inner td {
                    //				margin: 0 auto;
                    //			}

                    //			.image_block img.big,
                    //			.row-content {
                    //				width: 100% !important;
                    //			}

                    //			.mobile_hide {
                    //				display: none;
                    //			}

                    //			.stack .column {
                    //				width: 100%;
                    //				display: block;
                    //			}

                    //			.mobile_hide {
                    //				min-height: 0;
                    //				max-height: 0;
                    //				max-width: 0;
                    //				overflow: hidden;
                    //				font-size: 0px;
                    //			}

                    //			.desktop_hide,
                    //			.desktop_hide table {
                    //				display: table !important;
                    //				max-height: none !important;
                    //			}
                    //		}
                    //	</style>
                    //</head>

                    //<body style=""background-color: #d9dffa; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;"">
                    //	<table cellpadding=""0"" cellspacing=""0"" class=""nl-container"" role=""presentation"" style=""background-color: #d9dffa;""
                    //		width=""100%"">
                    //		<tbody>
                    //			<tr>
                    //				<td>
                    //					<table cellpadding=""0"" cellspacing=""0"" class=""row row-1"" role=""presentation""
                    //						style=""display: flex; align-items: center; justify-content: center; background-color: #cfd6f4;""
                    //						width=""100%"">
                    //						<tbody>
                    //							<tr>
                    //								<td>
                    //									<table cellpadding=""0"" cellspacing=""0"" class=""row-content stack"" role=""presentation""
                    //										style=""display: flex; align-items: center; justify-content: center; color: #000000; width: 600px;""
                    //										width=""600"">
                    //										<tbody>
                    //											<tr>
                    //												<td class=""column column-1""
                    //													style="" font-weight: 400; text-align: left; vertical-align: top; padding-top: 20px; padding-bottom: 0px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;""
                    //													width=""100%"">
                    //													<table cellpadding=""0"" cellspacing=""0"" class=""image_block block-1""
                    //														role=""presentation"" width=""100%"">
                    //														<tr>
                    //															<td class=""pad""
                    //																style=""width:100%;padding-right:0px;padding-left:0px;"">
                    //																<div class=""alignment""
                    //																	style=""display: flex; align-items: center; justify-content: center; line-height:10px"">
                    //																	<img alt=""Card Header with Border and Shadow Animated""
                    //																		class=""big"" src=""images/animated_header.gif""
                    //																		style=""display: block; height: auto; border: 0; width: 600px; max-width: 100%;""
                    //																		title=""Card Header with Border and Shadow Animated""
                    //																		width=""600"" />
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //												</td>
                    //											</tr>
                    //										</tbody>
                    //									</table>
                    //								</td>
                    //							</tr>
                    //						</tbody>
                    //					</table>
                    //					<table cellpadding=""0"" cellspacing=""0"" class=""row row-2"" role=""presentation""
                    //						style=""display: flex; align-items: center; justify-content: center; background-color: #d9dffa; background-image: url('images/body_background_2.png'); background-position: top center; background-repeat: repeat;""
                    //						width=""100%"">
                    //						<tbody>
                    //							<tr>
                    //								<td>
                    //									<table cellpadding=""0"" cellspacing=""0"" class=""row-content stack"" role=""presentation""
                    //										style=""display: flex; align-items: center; justify-content: center; color: #000000; width: 600px;""
                    //										width=""600"">
                    //										<tbody>
                    //											<tr>
                    //												<td class=""column column-1""
                    //													style="" font-weight: 400; text-align: left; padding-left: 50px; padding-right: 50px; vertical-align: top; padding-top: 15px; padding-bottom: 15px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;""
                    //													width=""100%"">
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""text_block block-1""
                    //														role=""presentation"" style="" word-break: break-word;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div style=""font-family: sans-serif"">
                    //																	<div
                    //																		style=""font-size: 14px; color: #506bec; line-height: 1.2; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;"">
                    //																		<p style=""margin: 0; font-size: 14px;"">
                    //																			<strong><span style=""font-size:38px;"">Forgot
                    //																					your password?</span></strong>
                    //																		</p>
                    //																	</div>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""text_block block-2""
                    //														role=""presentation"" style="" word-break: break-word;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div style=""font-family: sans-serif"">
                    //																	<div
                    //																		style=""font-size: 14px; color: #40507a; line-height: 1.2; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;"">
                    //																		<p style=""margin: 0; font-size: 14px;"">
                    //																			<span style=""font-size:16px;"">Hey, we
                    //																				received a request to reset your
                    //																				password.</span>
                    //																		</p>
                    //																	</div>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""text_block block-3""
                    //														role=""presentation"" style="" word-break: break-word;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div style=""font-family: sans-serif"">
                    //																	<div
                    //																		style=""font-size: 14px; color: #40507a; line-height: 1.2; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;"">
                    //																		<p style=""margin: 0; font-size: 14px;"">
                    //																			<span style=""font-size:16px;"">Let’s get you
                    //																				a new one!</span>
                    //																		</p>
                    //																	</div>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""0"" cellspacing=""0"" class=""button_block block-4""
                    //														role=""presentation"" width=""100%"">
                    //														<tr>
                    //															<td class=""pad""
                    //																style=""padding-bottom:20px;padding-left:10px;padding-right:10px;padding-top:20px;text-align:left;"">
                    //																<div style=""display: flex; align-items: left;""
                    //																	class=""alignment"">
                    //																	<a href=""http://116.203.90.202:88/reset-password""
                    //																		style=""text-decoration:none; display:inline-block;color:#ffffff; background-color:#506bec;border-radius:16px; width:auto; font-weight:undefined; border: 1px solid transparent; padding-top:8px;padding-bottom:8px;font-family:Helvetica Neue, Helvetica, Arial, sans-serif;font-size:15px;text-align:center; word-break:keep-all;""
                    //																		target=""_blank"">
                    //																		<span
                    //																			style=""padding-left:25px;padding-right:20px;font-size:15px;display:inline-block;letter-spacing:normal;""><span
                    //																				style=""word-break: break-word;""><span
                    //																					data-mce-
                    //																					style=""line-height: 30px;""><strong>RESET
                    //																						MY
                    //																						PASSWORD</strong></span></span></span></a>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""text_block block-5""
                    //														role=""presentation"" style="" word-break: break-word;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div style=""font-family: sans-serif"">
                    //																	<div
                    //																		style=""font-size: 14px; color: #40507a; line-height: 1.2; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;"">
                    //																		<p style=""margin: 0; font-size: 14px;"">
                    //																			<span style=""font-size:14px;"">Having
                    //																				trouble? <a
                    //																					href=""http://www.example.com/""
                    //																					rel=""noopener""
                    //																					style=""text-decoration: none; color: #40507a;""
                    //																					target=""_blank""
                    //																					title=""@SolaErp""><strong>@SolaErp</strong></a></span>
                    //																		</p>
                    //																	</div>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""text_block block-6""
                    //														role=""presentation"" style="" word-break: break-word;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div style=""font-family: sans-serif"">
                    //																	<div
                    //																		style=""font-size: 14px; color: #40507a; line-height: 1.2; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;"">
                    //																		<p style=""margin: 0; font-size: 14px;"">
                    //																			Didn’t request a password reset? You can
                    //																			ignore this message.</p>
                    //																	</div>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //												</td>
                    //											</tr>
                    //										</tbody>
                    //									</table>
                    //								</td>
                    //							</tr>
                    //						</tbody>
                    //					</table>
                    //					<table cellpadding=""0"" cellspacing=""0"" class=""row row-3"" role=""presentation""
                    //						style=""display: flex; align-items: center; justify-content: center;"" width=""100%"">
                    //						<tbody>
                    //							<tr>
                    //								<td>
                    //									<table cellpadding=""0"" cellspacing=""0"" class=""row-content stack"" role=""presentation""
                    //										style=""display: flex; align-items: center; justify-content: center; color: #000000; width: 600px;""
                    //										width=""600"">
                    //										<tbody>
                    //											<tr>
                    //												<td class=""column column-1""
                    //													style="" font-weight: 400; text-align: left; vertical-align: top; padding-top: 0px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;""
                    //													width=""100%"">
                    //													<table cellpadding=""0"" cellspacing=""0"" class=""image_block block-1""
                    //														role=""presentation"" width=""100%"">
                    //														<tr>
                    //															<td class=""pad""
                    //																style=""width:100%;padding-right:0px;padding-left:0px;"">
                    //																<div class=""alignment""
                    //																	style=""display: flex; align-items: center; justify-content: center; line-height:10px"">
                    //																	<img alt=""Card Bottom with Border and Shadow Image""
                    //																		class=""big"" src=""~/images/bottom_img.png""
                    //																		style=""display: block; height: auto; border: 0; width: 600px; max-width: 100%;""
                    //																		title=""Card Bottom with Border and Shadow Image""
                    //																		width=""600"" />
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //												</td>
                    //											</tr>
                    //										</tbody>
                    //									</table>
                    //								</td>
                    //							</tr>
                    //						</tbody>
                    //					</table>
                    //					<table cellpadding=""0"" cellspacing=""0"" class=""row row-4"" role=""presentation""
                    //						style=""display: flex; align-items: center; justify-content: center;"" width=""100%"">
                    //						<tbody>
                    //							<tr>
                    //								<td>
                    //									<table cellpadding=""0"" cellspacing=""0"" class=""row-content stack"" role=""presentation""
                    //										style=""display: flex; align-items: center; justify-content: center; color: #000000; width: 600px;""
                    //										width=""600"">
                    //										<tbody>
                    //											<tr>
                    //												<td class=""column column-1""
                    //													style="" font-weight: 400; text-align: left; padding-left: 10px; padding-right: 10px; vertical-align: top; padding-top: 10px; padding-bottom: 20px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;""
                    //													width=""100%"">
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""image_block block-1""
                    //														role=""presentation"" width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div class=""alignment""
                    //																	style=""display: flex; align-items: center; justify-content: center; line-height:10px"">
                    //																	<a href=""http://www.example.com/""
                    //																		style=""outline:none"" tabindex=""-1""
                    //																		target=""_blank""><img alt=""Your Logo""
                    //																			src=""~/images/logo.png""
                    //																			style=""display: block; height: auto; border: 0; width: 145px; max-width: 100%;""
                    //																			title=""Your Logo"" width=""145"" /></a>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""social_block block-2""
                    //														role=""presentation""
                    //														style=""display: flex; align-items: center; justify-content: center;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div
                    //																	class=""display: flex; align-items: center; justify-content: center; alignment"">
                    //																	<table cellpadding=""0"" cellspacing=""0""
                    //																		class=""social-table"" role=""presentation""
                    //																		style="" display: inline-block;"" width=""72px"">
                    //																		<tr>
                    //																			<td style=""padding:0 2px 0 2px;""><a
                    //																					href=""https://www.instagram.com/""
                    //																					target=""_blank""><img alt=""Instagram""
                    //																						height=""32""
                    //																						src=""images/instagram2x.png""
                    //																						style=""display: block; height: auto; border: 0;""
                    //																						title=""instagram""
                    //																						width=""32"" /></a></td>
                    //																			<td style=""padding:0 2px 0 2px;""><a
                    //																					href=""https://www.twitter.com/""
                    //																					target=""_blank""><img alt=""Twitter""
                    //																						height=""32""
                    //																						src=""images/twitter2x.png""
                    //																						style=""display: block; height: auto; border: 0;""
                    //																						title=""twitter""
                    //																						width=""32"" /></a></td>
                    //																		</tr>
                    //																	</table>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""text_block block-3""
                    //														role=""presentation"" style="" word-break: break-word;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div style=""font-family: sans-serif"">
                    //																	<div
                    //																		style=""font-size: 14px; color: #97a2da; line-height: 1.2; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;"">
                    //																		<p
                    //																			style=""margin: 0; font-size: 14px; text-align: center;"">
                    //																			+(994) 77-555–7777</p>
                    //																	</div>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""text_block block-4""
                    //														role=""presentation"" style="" word-break: break-word;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div style=""font-family: sans-serif"">
                    //																	<div
                    //																		style=""font-size: 14px; color: #97a2da; line-height: 1.2; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;"">
                    //																		<p
                    //																			style=""margin: 0; font-size: 14px; text-align: center;"">
                    //																			This link will expire in the next 24
                    //																			hours.<br />Please feel free to contact us
                    //																			at sola@apertech.net. </p>
                    //																	</div>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //													<table cellpadding=""10"" cellspacing=""0"" class=""text_block block-5""
                    //														role=""presentation"" style="" word-break: break-word;""
                    //														width=""100%"">
                    //														<tr>
                    //															<td class=""pad"">
                    //																<div style=""font-family: sans-serif"">
                    //																	<div
                    //																		style=""font-size: 14px; color: #97a2da; line-height: 1.2; font-family: Helvetica Neue, Helvetica, Arial, sans-serif;"">
                    //																		<p
                    //																			style=""margin: 0; text-align: center; font-size: 12px;"">
                    //																			<span style=""font-size:12px;"">Copyright©
                    //																				2022 Your Brand.</span>
                    //																		</p>
                    //																		<p id=""m_8010100107078456808text01""
                    //																			style=""margin: 0; text-align: center; font-size: 12px;"">
                    //																			<span style=""font-size:12px;""><a
                    //																					href=""http://www.example.com/""
                    //																					rel=""noopener""
                    //																					style=""text-decoration: underline; color: #97a2da;""
                    //																					target=""_blank""
                    //																					title=""Unsubscribe "">Unsubscribe</a>
                    //																				| <a href=""http://www.example.com/""
                    //																					rel=""noopener""
                    //																					style=""text-decoration: underline; color: #97a2da;""
                    //																					target=""_blank""
                    //																					title=""Manage your preferences"">Manage
                    //																					your preferences</a> | <a
                    //																					href=""http://www.example.com/""
                    //																					rel=""noopener""
                    //																					style=""text-decoration: underline; color: #97a2da;""
                    //																					target=""_blank""
                    //																					title=""Privacy Policy"">Privacy
                    //																					Policy</a></span>
                    //																		</p>
                    //																	</div>
                    //																</div>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //												</td>
                    //											</tr>
                    //										</tbody>
                    //									</table>
                    //								</td>
                    //							</tr>
                    //						</tbody>
                    //					</table>
                    //					<!-- <table cellpadding=""0"" cellspacing=""0"" class=""row row-5"" role=""presentation""
                    //						style=""display: flex; align-items: center; justify-content: center;"" width=""100%"">
                    //						<tbody>
                    //							<tr>
                    //								<td>
                    //									<table cellpadding=""0"" cellspacing=""0"" class=""row-content stack"" role=""presentation""
                    //										style=""display: flex; align-items: center; justify-content: center; color: #000000; width: 600px;""
                    //										width=""600"">
                    //										<tbody>
                    //											<tr>
                    //												<td class=""column column-1""
                    //													style="" font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;""
                    //													width=""100%"">
                    //													<table cellpadding=""0"" cellspacing=""0"" class=""icons_block block-1""
                    //														role=""presentation"" width=""100%"">
                    //														<tr>
                    //															<td class=""pad""
                    //																style=""vertical-align: middle; color: #9d9d9d; font-family: inherit; font-size: 15px; padding-bottom: 5px; padding-top: 5px; text-align: center;"">
                    //																<table cellpadding=""0"" cellspacing=""0""
                    //																	role=""presentation"" width=""100%"">
                    //																	<tr>
                    //																		<td class=""alignment""
                    //																			style=""vertical-align: middle; text-align: center;"">
                    //																			<table cellpadding=""0"" cellspacing=""0""
                    //																				class=""icons-inner"" role=""presentation""
                    //																				style="" display: inline-block; margin-right: -4px; padding-left: 0px; padding-right: 0px;"">
                    //																				<tr>
                    //																					<td
                    //																						style=""vertical-align: middle; text-align: center; padding-top: 5px; padding-bottom: 5px; padding-left: 5px; padding-right: 6px;"">
                    //																						<a href=""https://www.designedwithbee.com/""
                    //																							style=""text-decoration: none;""
                    //																							target=""_blank""><img
                    //																								alt=""Designed with BEE""
                    //																								class=""icon"" height=""32""
                    //																								src=""images/bee.png""
                    //																								style=""display: flex; align-items: center; justify-content: center; height: auto; margin: 0 auto; border: 0;""
                    //																								width=""34"" /></a>
                    //																					</td>
                    //																					<td
                    //																						style=""font-family: Helvetica Neue, Helvetica, Arial, sans-serif; font-size: 15px; color: #9d9d9d; vertical-align: middle; letter-spacing: undefined; text-align: center;"">
                    //																						<a href=""https://www.designedwithbee.com/""
                    //																							style=""color: #9d9d9d; text-decoration: none;""
                    //																							target=""_blank"">Designed
                    //																							with BEE</a>
                    //																					</td>
                    //																				</tr>
                    //																			</table>
                    //																		</td>
                    //																	</tr>
                    //																</table>
                    //															</td>
                    //														</tr>
                    //													</table>
                    //												</td>
                    //											</tr>
                    //										</tbody>
                    //									</table>
                    //								</td>
                    //							</tr>
                    //						</tbody>
                    //					</table> -->
                    //				</td>
                    //			</tr>
                    //		</tbody>
                    //	</table>
                    //</body>

                    //</html>";
                    #endregion

                    message.Body = reader.ReadToEnd();
                    message.To.Add(to);

                    await smtpClient.SendMailAsync(message);
                }
            }
        }
    }
}
