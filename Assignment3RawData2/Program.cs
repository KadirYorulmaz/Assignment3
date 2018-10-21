using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Entities;
using Newtonsoft.Json;

namespace Assignment2RawData2
{
    class Program
    {
        static void Main(string[] args)
        {
       
            var client = new TcpClient();


            client.Connect(IPAddress.Parse("127.0.0.1"), 5000);

            var strm = client.GetStream();

            //GET
            var svrMsgFromResponse = getResponse(getRequest(1), strm, client);
            var categoryFromJSON = JsonConvert.DeserializeObject<Category>(svrMsgFromResponse.Body);
            Console.WriteLine($"Response: {svrMsgFromResponse.Status + " " + categoryFromJSON.Name + " " + categoryFromJSON.ID }");

            //POST
            //Category category = new Category
            //{
            //    ID = 4,
            //    Name = "Banan"
            //};

            //var svrMsgFromResponse = getResponse(postRequest(category), strm, client);
            //var categoryFromJSON = JsonConvert.DeserializeObject<Category>(svrMsgFromResponse.Body);
            //Console.WriteLine($"Response: {svrMsgFromResponse.Status + " " + categoryFromJSON.Name + " " + categoryFromJSON.ID }");

            //UPDATE
            //var idToUpdate = 1;
            //var getResponseToUpdate = getResponse(getRequest(idToUpdate), strm, client);
            //var deseralizeCategoryGetResponse = JsonConvert.DeserializeObject<Category>(getResponseToUpdate.Body);
            //var svrMsgFromResponse = getResponse(updateRequest(idToUpdate, deseralizeCategoryGetResponse), strm, client);
            //var categoryFromJSON = JsonConvert.DeserializeObject<Category>(svrMsgFromResponse.Body);
            //Console.WriteLine($"Response: {svrMsgFromResponse.Status + " " + categoryFromJSON.Name + " " + categoryFromJSON.ID }");

            //DELETE
            //var idToDelete = 2;
            //var svrMsgFromResponse = getResponse(deleteRequest(idToDelete), strm, client);
            //var categoryFromJSON = JsonConvert.DeserializeObject<List<Category>>(svrMsgFromResponse.Body);
            //foreach (var c in categoryFromJSON)
            //{
            //    Console.WriteLine($"Response: {svrMsgFromResponse.Status + " " + c.Name + " " + c.ID }");
            //}



            strm.Close();


            client.Close();
        }



        private static Response getResponse(string request, NetworkStream strm, TcpClient client)
        {
            var msg = Encoding.UTF8.GetBytes(request);

            strm.Write(msg, 0, msg.Length);


            var buffer = new byte[client.ReceiveBufferSize];

            var readCnt = strm.Read(buffer, 0, buffer.Length);

            var svrMsg = Encoding.UTF8.GetString(buffer, 0, readCnt);

            var svrMsgFromResponse = JsonConvert.DeserializeObject<Response>(svrMsg);

            return svrMsgFromResponse;
        }

        private static string getRequest(int id)
        {
            Request requestGet = new Request
            {
                Method = "GET",
                Path = $"/Category/{id}",
                Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };


            var getRequestAsJson = JsonConvert.SerializeObject(requestGet);

            return getRequestAsJson;
        }

        private static string postRequest(Category category)
        {
            
            var categoryAsJson = JsonConvert.SerializeObject(category);

            Request requestPost = new Request
            {
                Method = "POST",
                Path = "/Category/",
                Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Body = categoryAsJson
            };

            var postRequestAsJson = JsonConvert.SerializeObject(requestPost);

            return postRequestAsJson;
        }

        private static string updateRequest(int id, Category category)
        {
            var categoryToUpdate = getRequest(id);

         

            category.Name = "Ananas";

            var categoryAsJson = JsonConvert.SerializeObject(category);

            Request requestUpdate = new Request
            {
                Method = "UPDATE",
                Path = $"Category/{id}",
                Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Body = categoryAsJson
            };

            var updateRequestAsJson = JsonConvert.SerializeObject(requestUpdate);

            return updateRequestAsJson;


        }


        private static string deleteRequest(int id)
        {
            Request requestDelete = new Request
            {
                Method = "DELETE",
                Path = $"/Category/{id}",
                Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            var deleteRequestAsJson = JsonConvert.SerializeObject(requestDelete);

            return deleteRequestAsJson;
        }
    }
}

//Echo Client