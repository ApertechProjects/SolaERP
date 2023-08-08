using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Services
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<string> UploadFile(UploadFile uploadFile)
        {
            #region
            //using var client = new HttpClient();

            //var filePath = Path.Combine("IntegrationTests", "file.csv");
            //var gg = File.ReadAllBytes(filePath);
            //var byteArrayContent = new ByteArrayContent(gg);
            //var postResponse = await client.PostAsync("http://116.203.90.202:8080/api/v1/home/upload", new MultipartFormDataContent
            //{
            //    {byteArrayContent }
            //});

            //if (postResponse.IsSuccessStatusCode)
            //{
            //    var data = await postResponse.Content.ReadAsStringAsync();
            //}


            //var filePath = @"C:\Users\User\Documents\image.svg";

            //using (var multipartFormContent = new MultipartFormDataContent())
            //{
            //    //Load the file and set the file's Content-Type header
            //    var fileStreamContent = new StreamContent(File.OpenRead(filePath));
            //    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/svg");

            //    //Add the file
            //    multipartFormContent.Add(fileStreamContent, name: formFiles[0].Name, fileName: formFiles[0].FileName);

            //    //Send it
            //    var response = await client.PostAsync("http://116.203.90.202:8080/api/v1/home/upload", multipartFormContent);
            //    response.EnsureSuccessStatusCode();
            //    return await response.Content.ReadAsStringAsync();
            //}
            #endregion

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                // Dosya adını alın
                string fileName = uploadFile.Files[0].FileName;

                // Dosyayı bir byte dizisine okuyun
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await uploadFile.Files[0].CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                // Byte dizisini içeriğe ekleyin
                var fileContent = new ByteArrayContent(fileBytes);
                content.Add(new StringContent("rfqs"), "module");
                content.Add(fileContent, "files", fileName);

                // İstek gönderin
                var response = await client.PostAsync("http://116.203.90.202:8080/api/v1/home/upload", content);
                var ttt = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Başarılı işlem
                }
                else
                {
                    // Hata durumu
                }
            }


            return null;
        }
        
    }


}

