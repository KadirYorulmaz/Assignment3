using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace EchoServer
{
    class Program
    {

        static void Main(string[] args)
        {
            Database db = new Database();
            db.SeedCategories();
            //db.Categories.Add(());
            //db.Categories.Where(x => x.Name ==);
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            server.Start();
            Console.WriteLine("Server started ....");


            while (true)
            {
                Response response = new Response();

                var client = server.AcceptTcpClient();
                var strm = client.GetStream();

                var buffer = new byte[client.ReceiveBufferSize];

                var readCnt = strm.Read(buffer, 0, buffer.Length);

                var msg = Encoding.UTF8.GetString(buffer, 0, readCnt);
  
                var requestFromJSON = JsonConvert.DeserializeObject<Request>(msg);


                //bool ifOneOfMethods = (requestFromJSON.Method == "delete" || 
                //                       requestFromJSON.Method == "update" || 
                //                       requestFromJSON.Method == "read" || 
                //                       requestFromJSON.Method == "delete");

                bool ifOneOfRequestPropertyNull = (requestFromJSON.Path == null || requestFromJSON.Body == null);
                
                if (ifOneOfRequestPropertyNull)
                {
                    response = new Response
                    {
                        Status = "missing resource"
                    };
                    
                }

                bool ifMissingBody = (requestFromJSON.Body == null && requestFromJSON.Method == "create" || requestFromJSON.Method == "update" || requestFromJSON.Method == "echo");

                if (ifMissingBody)
                {
                    response = new Response
                    {
                        Status = "missing body"
                    };
                }



                if (requestFromJSON.Method == "create")
                {
                    if (requestFromJSON.Path != null && requestFromJSON.Body != null)
                    {
                        Console.WriteLine("*********************************************create +1");
                        //Splitter path i tre bider
                        var listOfSplitPath = splitPath(requestFromJSON);
                        //listOfSplitPath.ForEach(x => Console.WriteLine(x.ToString()));

                        if (listOfSplitPath[0].ToString() == "api" && listOfSplitPath[1].ToString() == "categories" && !string.IsNullOrEmpty(listOfSplitPath[1].ToString()))
                        {
                            response = new Response
                            {
                                Status = "4 Bad Request"
                            };
                        }
                    }
                    

                }

                else if (requestFromJSON.Method == "read")
                {
                    if (requestFromJSON.Path != null && requestFromJSON.Body != null)
                    {
                  
                    }
                }

                else if (requestFromJSON.Method == "update")
                {
                    if (requestFromJSON.Path != null && requestFromJSON.Body != null)
                    {
                        try
                        {
                            var obj = JToken.Parse(requestFromJSON.Body);
                        }
                        catch
                        {
                            response = new Response
                            {
                                Status = "illegal body"
                            };
                        }
                    }
                }

                else if (requestFromJSON.Method == "delete")
                {
                    if (requestFromJSON.Path != null && requestFromJSON.Body != null)
                    {
                        Console.WriteLine("*********************************************delete +2");
                        //Splitter path i tre bider
                        var listOfSplitPath = splitPath(requestFromJSON);
                        //listOfSplitPath.ForEach(x => Console.WriteLine(x.ToString()));

                        Console.WriteLine(listOfSplitPath.Count);

                        if (listOfSplitPath.Count != 3)
                        {
                            response = new Response
                            {
                                Status = "4 Bad Request"
                            };
                        }
                    }

                    //if (listOfSplitPath[0].ToString() == "api" 
                    //    && listOfSplitPath[1].ToString() == "categories" 
                    //    && !string.IsNullOrEmpty(listOfSplitPath[2].ToString()))
                    //{
                    //    response = new Response
                    //    {
                    //        Status = "4 Bad Request"
                    //    };
                    //}

                }
             

                var responseAsJson = JsonConvert.SerializeObject(response);

                var payload = Encoding.UTF8.GetBytes(responseAsJson);

                strm.Write(payload, 0, payload.Length);

                strm.Close();

                client.Close();
            }

           
        }


        //public static void checkIfHasAllResources(Request request, Response response)
        //{
        //    if (request.Method == "delete" || request.Method == "update" || request.Method == "read" || request.Method == "delete" && request.GetType().GetProperty("Body") == null && request.GetType().GetProperty("Path") == null)
        //    {
        //        response = new Response
        //        {
        //            Status = "missing resource"
        //        };
        //    }
        //}


        public static List<object> splitPath(Request request)
        {
            var regex = @"\/[a-zA-Z0-9]*";

            var lengthOfPath = Regex.Matches(request.Path, regex);

            List<object> list = new List<object>();

            foreach (var item in lengthOfPath)
            {
                list.Add(item.ToString().Trim('/'));
            }

            return list;
        }

    }

    public class Database
    {
        public void SeedCategories()
        {
            Categories = new List<Category>
            {
                new Category
                {
                    ID = 1,
                    Name = "Beverages"
                },
                new Category
                {
                    ID = 2,
                    Name = "Condiments"
                },
                new Category
                {
                    ID = 3,
                    Name = "Confections"
                }
            };

        }

        public List<Category> Categories { get; set; }
    }
}









//else if (requestFromJSON.Method == "GET")
//{
//    var idFromPath =  requestFromJSON.Path.Split("/")[2];

//    var category = db.Categories.Where(x => x.ID == int.Parse(idFromPath)).FirstOrDefault();
//    Console.WriteLine($"Message: {category.ID + " " + category.Name}");

//    response = new Response
//    {
//        Body = JsonConvert.SerializeObject(category),
//        Status = "200"
//    };
//}
//else if (requestFromJSON.Method == "POST")
//{
//    var categoryFromJSON = JsonConvert.DeserializeObject<Category>(requestFromJSON.Body);

//    db.Categories.Add(categoryFromJSON);

//    var newCategoryObject =db.Categories.Where(x => x.ID == categoryFromJSON.ID).FirstOrDefault();
//    response = new Response
//    {
//        Body = JsonConvert.SerializeObject(newCategoryObject),
//        Status = "201"
//    };
//}
//else if (requestFromJSON.Method == "UPDATE")
//{
//    var categoryFromJSON = JsonConvert.DeserializeObject<Category>(requestFromJSON.Body);

//    var categoryToEdit = db.Categories.Where(x => x.ID == categoryFromJSON.ID).FirstOrDefault();
//    categoryToEdit = categoryFromJSON;           

//    var newCategoryObject = db.Categories.Where(x => x.ID == categoryFromJSON.ID).FirstOrDefault();
//    response = new Response
//    {
//        Body = JsonConvert.SerializeObject(newCategoryObject),
//        Status = "200"
//    };
//}
//else if (requestFromJSON.Method == "DELETE")
//{
//    var idFromPath = requestFromJSON.Path.Split("/")[2];

//    var category = db.Categories.Where(x => x.ID == int.Parse(idFromPath)).FirstOrDefault();
//    db.Categories.Remove(category);
//    Console.WriteLine($"Message: {category.ID + " " + category.Name}");

//    var getAllInCategories = db.Categories.ToList();

//    response = new Response
//    {
//        Body = JsonConvert.SerializeObject(getAllInCategories),
//        Status = "200"
//    };
//}
