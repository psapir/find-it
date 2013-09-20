using FuelSDK;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using System.Xml.Linq;

namespace FindIt.ETCrawler
{

    public class Crawler
    {
        /// <summary>
        /// Main Crawl method
        /// </summary>
        /// <param name="mid"></param>
        public void Crawl(ET_Client client)
        {
            RetrieveFolderStructure("0", client, "email", "");
            RetrieveFolderStructure("0", client, "dataextension", "");
            RetrieveFolderStructure("0", client, "media", "");
            RetrieveFolderStructure("0", client, "content", "");
        }

        public void RetrieveFolderStructure(string id, ET_Client etClient, string contentType, string path)
        {
                // Create the SOAP binding for call.
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Name = "UserNameSoapBinding";
                binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
                binding.MaxReceivedMessageSize = 2147483647;
                var client = new FuelSDK.SoapClient(binding, etClient.soapclient.Endpoint.Address);
                client.ClientCredentials.UserName.UserName = "*";
                client.ClientCredentials.UserName.Password = "*";

                using (var scope = new OperationContextScope(client.InnerChannel))
                {
                    // Add oAuth token to SOAP header.
                    XNamespace ns = "http://exacttarget.com";
                    var oauthElement = new XElement(ns + "oAuthToken", etClient.internalAuthToken);
                    var xmlHeader = MessageHeader.CreateHeader("oAuth", "http://exacttarget.com", oauthElement);
                    OperationContext.Current.OutgoingMessageHeaders.Add(xmlHeader);

                    List<object> folders = new List<object>();

                    String requestID;
                    String status;
                    APIObject[] results;

                    SimpleFilterPart sfp = new SimpleFilterPart();
                    sfp.Property = "ContentType";
                    sfp.SimpleOperator = SimpleOperators.equals;
                    sfp.Value = new string[] { contentType };

                    SimpleFilterPart rf = new SimpleFilterPart();
                    rf.Property = "ParentFolder.ID";
                    rf.SimpleOperator = SimpleOperators.equals;
                    rf.Value = new string[] { id };

                    ComplexFilterPart cfp = new ComplexFilterPart();
                    cfp.LeftOperand = sfp;
                    cfp.LogicalOperator = LogicalOperators.AND;
                    cfp.RightOperand = rf;

                    RetrieveRequest rr = new RetrieveRequest();

                    rr.ObjectType = "DataFolder";
                    rr.Properties = new string[] { "ID", "Name", "ParentFolder.ID", "ParentFolder.Name" };
                    rr.Filter = cfp;

                    status = client.Retrieve(rr, out requestID, out results);

                    if (results.Count() > 0)
                    {
                        foreach (DataFolder df in results)
                        {
                            RetrieveObjectsByFolderID(df.ID.ToString(), etClient, path + "/" + df.Name + "/", contentType);
                            RetrieveFolderStructure(df.ID.ToString(), etClient, contentType, path + "/" + df.Name + "/");
                        }
                    }
                }
        }

        public void RetrieveObjectsByFolderID(string folderId, ET_Client etClient, string path, string contentType)
        {
             // Create the SOAP binding for call.
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Name = "UserNameSoapBinding";
                binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
                binding.MaxReceivedMessageSize = 2147483647;
                var client = new FuelSDK.SoapClient(binding, etClient.soapclient.Endpoint.Address);
                client.ClientCredentials.UserName.UserName = "*";
                client.ClientCredentials.UserName.Password = "*";

                using (var scope = new OperationContextScope(client.InnerChannel))
                {
                    // Add oAuth token to SOAP header.
                    XNamespace ns = "http://exacttarget.com";
                    var oauthElement = new XElement(ns + "oAuthToken", etClient.internalAuthToken);
                    var xmlHeader = MessageHeader.CreateHeader("oAuth", "http://exacttarget.com", oauthElement);
                    OperationContext.Current.OutgoingMessageHeaders.Add(xmlHeader);

                    List<object> folders = new List<object>();
                    string cType = "Email";

                    switch (contentType)
                    {
                        case "media":
                            cType = "Portfolio";
                            break;
                        case "dataextension":
                            cType = "DataExtension";
                            break;
                        case "content":
                            cType = "ContentArea";
                            break;
                    }

                    string requestID;
                    // Filter by the Folder/Category 
                    SimpleFilterPart sfp = new SimpleFilterPart();
                    sfp.Property = "CategoryID";
                    sfp.SimpleOperator = SimpleOperators.equals;
                    sfp.Value = new string[] { folderId };

                    // Create the RetrieveRequest object

                    RetrieveRequest request = new RetrieveRequest();
                    request.ObjectType = cType;
                    request.Filter = sfp;
                    request.Properties = new string[] { "CustomerKey", "Name", "CreatedDate", "ModifiedDate" };

                    if(contentType == "media")
                        request.Properties = new string[] { "CustomerKey", "FileName", "CreatedDate", "ModifiedDate" };

                    // Execute the Retrieve
                    APIObject[] results;
                   
                    string status = client.Retrieve(request, out requestID, out results);

                    for (int cntr = 0; cntr < results.Length; cntr++)
                    {
                            string sql = "INSERT INTO [Results] ([IdResult] ,[CustomerKey] ,[Name] ,[ResultType] ,[Path] ,[URL],[ThumbnailURL],[CreatedDate],[ModifiedDate],[IdContactIndex])VALUES (@IdResult ,@CustomerKey ,@Name ,@ResultType ,@Path ,@URL,@ThumbnailURL,@CreatedDate,@ModifiedDate,@IdContactIndex)";

                            switch (contentType)
                            {
                                case "media":
                                    var portfolio = (Portfolio)results[cntr];
                                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["FindIt.Data.Entities"].ConnectionString))
                                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                                    {

                                        cmd.Parameters.Add("@IdResult", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                                        cmd.Parameters.Add("@CustomerKey", System.Data.SqlDbType.VarChar).Value = portfolio.CustomerKey;
                                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = portfolio.FileName;
                                        cmd.Parameters.Add("@ResultType", System.Data.SqlDbType.VarChar).Value = contentType;
                                        cmd.Parameters.Add("@Path", System.Data.SqlDbType.VarChar).Value = path;
                                        cmd.Parameters.Add("@URL", System.Data.SqlDbType.VarChar).Value = "";
                                        cmd.Parameters.Add("@ThumbnailURL", System.Data.SqlDbType.VarChar).Value = "";
                                        cmd.Parameters.Add("@CreatedDate", System.Data.SqlDbType.DateTime).Value = portfolio.CreatedDate;
                                        cmd.Parameters.Add("@ModifiedDate", System.Data.SqlDbType.DateTime).Value = portfolio.ModifiedDate;
                                        cmd.Parameters.Add("@IdContactIndex", System.Data.SqlDbType.UniqueIdentifier).Value = new Guid("A08D166C-F310-4F2D-8D44-5DD8F1564B8F");
                                        cn.Open();

                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                                case "dataextension":
                                    var dataextension = (DataExtension)results[cntr];
                                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["FindIt.Data.Entities"].ConnectionString))
                                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                                    {

                                        cmd.Parameters.Add("@IdResult", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                                        cmd.Parameters.Add("@CustomerKey", System.Data.SqlDbType.VarChar).Value = dataextension.CustomerKey;
                                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = dataextension.Name;
                                        cmd.Parameters.Add("@ResultType", System.Data.SqlDbType.VarChar).Value = contentType;
                                        cmd.Parameters.Add("@Path", System.Data.SqlDbType.VarChar).Value = path;
                                        cmd.Parameters.Add("@URL", System.Data.SqlDbType.VarChar).Value = "";
                                        cmd.Parameters.Add("@ThumbnailURL", System.Data.SqlDbType.VarChar).Value = "";
                                        cmd.Parameters.Add("@CreatedDate", System.Data.SqlDbType.DateTime).Value = dataextension.CreatedDate;
                                        cmd.Parameters.Add("@ModifiedDate", System.Data.SqlDbType.DateTime).Value = dataextension.ModifiedDate;
                                        cmd.Parameters.Add("@IdContactIndex", System.Data.SqlDbType.UniqueIdentifier).Value = new Guid("A08D166C-F310-4F2D-8D44-5DD8F1564B8F");
                                        cn.Open();

                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                                case "content":
                                    var content = (ContentArea)results[cntr];
                                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["FindIt.Data.Entities"].ConnectionString))
                                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                                    {

                                        cmd.Parameters.Add("@IdResult", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                                        cmd.Parameters.Add("@CustomerKey", System.Data.SqlDbType.VarChar).Value = content.CustomerKey;
                                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = content.Name;
                                        cmd.Parameters.Add("@ResultType", System.Data.SqlDbType.VarChar).Value = contentType;
                                        cmd.Parameters.Add("@Path", System.Data.SqlDbType.VarChar).Value = path;
                                        cmd.Parameters.Add("@URL", System.Data.SqlDbType.VarChar).Value = "";
                                        cmd.Parameters.Add("@ThumbnailURL", System.Data.SqlDbType.VarChar).Value = "";
                                        cmd.Parameters.Add("@CreatedDate", System.Data.SqlDbType.DateTime).Value = content.CreatedDate;
                                        cmd.Parameters.Add("@ModifiedDate", System.Data.SqlDbType.DateTime).Value = content.ModifiedDate;
                                        cmd.Parameters.Add("@IdContactIndex", System.Data.SqlDbType.UniqueIdentifier).Value = new Guid("A08D166C-F310-4F2D-8D44-5DD8F1564B8F");
                                        cn.Open();

                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                                default:
                                    var email = (Email)results[cntr];
                                    
                                    using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["FindIt.Data.Entities"].ConnectionString))
                                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                                    {
                                       
                                        cmd.Parameters.Add("@IdResult",System.Data.SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                                        cmd.Parameters.Add("@CustomerKey", System.Data.SqlDbType.VarChar).Value = email.CustomerKey;
                                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = email.Name;
                                        cmd.Parameters.Add("@ResultType", System.Data.SqlDbType.VarChar).Value = contentType;
                                        cmd.Parameters.Add("@Path", System.Data.SqlDbType.VarChar).Value = path;
                                        cmd.Parameters.Add("@URL", System.Data.SqlDbType.VarChar).Value = "";
                                        cmd.Parameters.Add("@ThumbnailURL", System.Data.SqlDbType.VarChar).Value = "";
                                        cmd.Parameters.Add("@CreatedDate", System.Data.SqlDbType.DateTime).Value = email.CreatedDate;
                                        cmd.Parameters.Add("@ModifiedDate", System.Data.SqlDbType.DateTime).Value = email.ModifiedDate;
                                        cmd.Parameters.Add("@IdContactIndex", System.Data.SqlDbType.UniqueIdentifier).Value = new Guid("A08D166C-F310-4F2D-8D44-5DD8F1564B8F");
                                        cn.Open();

                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                            }

                          
                        
                    }
                }
        }
    }
}