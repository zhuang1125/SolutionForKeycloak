using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using TodoASMXService.Models;
using TodoASMXService.Services;

namespace TodoASMXService
{
    [WebService(Namespace = "http://www.xamarin.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class TodoService : System.Web.Services.WebService
    {
        static readonly ITodoService todoService = new Services.TodoService(new TodoRepository());
       
        [WebMethod]
        public List<TodoItem> GetTodoItems()
        {
            return todoService.GetData().ToList();
        }

        [WebMethod]
        public void CreateTodoItem(TodoItem item)
        {
            try
            {
                if (item == null ||
                    string.IsNullOrWhiteSpace(item.Name) ||
                    string.IsNullOrWhiteSpace(item.Notes))
                {
                    throw new SoapException("TodoItem name and notes fields are required", SoapException.ClientFaultCode);
                }

                // Determine if the ID already exists
                var itemExists = todoService.DoesItemExist(item.ID);
                if (itemExists)
                {
                  //  throw new SoapException("TodoItem ID is in use", SoapException.ClientFaultCode);

                    XmlDocument doc = new XmlDocument();
                    XmlNode node = doc.CreateNode(XmlNodeType.Element,
                        SoapException.DetailElementName.Name,
                        SoapException.DetailElementName.Namespace);
                    node.InnerText = "TodoItem ID is in use";
                    throw new SoapException("Error", SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri, node, new Exception("TodoItem ID is in use"));
                }
                todoService.InsertData(item);
            }
            catch (Exception ex)
            {


                XmlDocument doc = new XmlDocument();
                XmlNode node = doc.CreateNode(XmlNodeType.Element,
                    SoapException.DetailElementName.Name,
                    SoapException.DetailElementName.Namespace);
                node.InnerText = ex.Message;
                throw new SoapException("Error", SoapException.ServerFaultCode, Context.Request.Url.AbsoluteUri, node, ex);
                
            }
        }

        [WebMethod]
        public void EditTodoItem(TodoItem item)
        {
            try
            {
                if (item == null ||
                    string.IsNullOrWhiteSpace(item.Name) ||
                    string.IsNullOrWhiteSpace(item.Notes))
                {
                   // throw new SoapException("TodoItem name and notes fields are required", SoapException.ClientFaultCode);

                    XmlDocument doc = new XmlDocument();
                    XmlNode node = doc.CreateNode(XmlNodeType.Element,
                        SoapException.DetailElementName.Name,
                        SoapException.DetailElementName.Namespace);
                    node.InnerText = "TodoItem name and notes fields are required";
                    throw new SoapException("Error", SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri, node, new Exception("TodoItem name and notes fields are required"));
                }

                var todoItem = todoService.Find(item.ID);
                if (todoItem != null)
                {
                    todoService.UpdateData(item);
                }
                else
                {
                   // throw new SoapException("TodoItem not found", SoapException.ClientFaultCode);

                    XmlDocument doc = new XmlDocument();
                    XmlNode node = doc.CreateNode(XmlNodeType.Element,
                        SoapException.DetailElementName.Name,
                        SoapException.DetailElementName.Namespace);
                    node.InnerText = "TodoItem not found";
                    throw new SoapException("Error", SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri, node, new Exception("TodoItem not found"));
                }
            }
            catch (Exception ex)
            {
                XmlDocument doc = new XmlDocument();
                XmlNode node = doc.CreateNode(XmlNodeType.Element,
                    SoapException.DetailElementName.Name,
                    SoapException.DetailElementName.Namespace);
                node.InnerText = ex.Message;
                throw new SoapException("Error", SoapException.ServerFaultCode,Context.Request.Url.AbsoluteUri,node, ex);
            }
        }

        [WebMethod]
        public void DeleteTodoItem(string id)
        {
            try
            {
                var todoItem = todoService.Find(id);
                if (todoItem != null)
                {
                    todoService.DeleteData(id);
                }
                else
                {
                   // throw new SoapException("TodoItem not found", SoapException.ClientFaultCode);

                    XmlDocument doc = new XmlDocument();
                    XmlNode node = doc.CreateNode(XmlNodeType.Element,
                        SoapException.DetailElementName.Name,
                        SoapException.DetailElementName.Namespace);
                    node.InnerText = "TodoItem not found";
                    throw new SoapException("Error", SoapException.ServerFaultCode, Context.Request.Url.AbsoluteUri, node, new Exception("TodoItem not found"));
                }
            }
            catch (Exception ex)
            {
                XmlDocument doc = new XmlDocument();
                XmlNode node = doc.CreateNode(XmlNodeType.Element,
                    SoapException.DetailElementName.Name,
                    SoapException.DetailElementName.Namespace);
                node.InnerText = ex.Message;
                throw new SoapException("Error", SoapException.ServerFaultCode, Context.Request.Url.AbsoluteUri, node, ex);
            }
        }
    }
}
