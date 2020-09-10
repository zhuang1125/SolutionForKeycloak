using Android.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace TodoASMX.Droid
{
 
    public class SoapService : ISoapService
    {
        ASMXService.TodoService todoService;
        TaskCompletionSource<SoapResult<List<TodoItem>>> getRequestComplete = null;
        TaskCompletionSource<SoapResult<bool>> saveRequestComplete = null;
        TaskCompletionSource<SoapResult<bool>> deleteRequestComplete = null;

        public List<TodoItem> Items { get; private set; } = new List<TodoItem>();

        public SoapService()
        {
            todoService = new ASMXService.TodoService();
            todoService.Url = Constants.SoapUrl;

            todoService.GetTodoItemsCompleted += TodoService_GetTodoItemsCompleted;
            todoService.CreateTodoItemCompleted += TodoService_SaveTodoItemCompleted;
            todoService.EditTodoItemCompleted += TodoService_SaveTodoItemCompleted;
            todoService.DeleteTodoItemCompleted += TodoService_DeleteTodoItemCompleted;
        }

        ASMXService.TodoItem ToASMXServiceTodoItem(TodoItem item)
        {
            return new ASMXService.TodoItem
            {
                ID = item.ID,
                Name = item.Name,
                Notes = item.Notes,
                Done = item.Done
            };
        }

        static TodoItem FromASMXServiceTodoItem(ASMXService.TodoItem item)
        {
            return new TodoItem
            {
                ID = item.ID,
                Name = item.Name,
                Notes = item.Notes,
                Done = item.Done
            };
        }

        private void TodoService_GetTodoItemsCompleted(object sender, ASMXService.GetTodoItemsCompletedEventArgs e)
        {
            try
            {
                getRequestComplete = getRequestComplete ?? new TaskCompletionSource<SoapResult<List<TodoItem>>>();

                Items = new List<TodoItem>();
                foreach (var item in e.Result)
                {
                    Items.Add(FromASMXServiceTodoItem(item));
                }
                getRequestComplete?.TrySetResult(new SoapResult<List<TodoItem>>() { res = Items, error = e.Error });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
            }
        }

        private void TodoService_SaveTodoItemCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            saveRequestComplete?.TrySetResult(new SoapResult<bool>() { res = e.Error==null ,error=e.Error});
        }


        private void TodoService_DeleteTodoItemCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            deleteRequestComplete?.TrySetResult(new SoapResult<bool>() { res = e.Error == null, error = e.Error });
        }

        public async Task<SoapResult<List<TodoItem>>> RefreshDataAsync()
        {
            getRequestComplete = new TaskCompletionSource<SoapResult<List<TodoItem>>>();
            todoService.GetTodoItemsAsync();
           return await getRequestComplete.Task;
          //  return new SoapResult<List<TodoItem>>() { res = Items, error = e.Error };
        }

        public async Task<SoapResult<bool>> SaveTodoItemAsync(TodoItem item, bool isNewItem = false)
        {
            saveRequestComplete = new TaskCompletionSource<SoapResult<bool>>();
            try
            {
                var todoItem = ToASMXServiceTodoItem(item);
                
                if (isNewItem)
                {
                    todoService.CreateTodoItemAsync(todoItem);
                }
                else
                {
                    todoService.EditTodoItemAsync(todoItem);
                }
            return    await saveRequestComplete.Task;
            }
            catch (SoapException se)
            {
                Debug.WriteLine("\t\t{0}", se.Message);
                saveRequestComplete?.TrySetResult(new SoapResult<bool>() { res =false, error = se });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
                saveRequestComplete?.TrySetResult(new SoapResult<bool>() { res = false, error = ex });
            }
            return await saveRequestComplete.Task;
        }

        public async Task<SoapResult<bool>> DeleteTodoItemAsync(string id)
        {
            deleteRequestComplete = new TaskCompletionSource<SoapResult<bool>>();
            try
            {
                
                todoService.DeleteTodoItemAsync(id);
                await deleteRequestComplete.Task;
            }
            catch (SoapException se)
            {
                Debug.WriteLine("\t\t{0}", se.Message);
                deleteRequestComplete.TrySetResult(new SoapResult<bool>() { res = false, error = se });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\t\tERROR {0}", ex.Message);
                deleteRequestComplete.TrySetResult(new SoapResult<bool>() { res = false, error = ex });
            }
            return await saveRequestComplete.Task;
        }
    }
}
