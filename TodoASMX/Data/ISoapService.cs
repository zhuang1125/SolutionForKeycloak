using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoASMX
{
	public class SoapResult<T>
	{
		public T res { get; set; }
		public System.Exception error { get; set; }
		
	}
	public interface ISoapService
	{
		Task<SoapResult<List<TodoItem>>> RefreshDataAsync();

		Task<SoapResult<bool>> SaveTodoItemAsync (TodoItem item, bool isNewItem);

		Task<SoapResult<bool>> DeleteTodoItemAsync (string id);
	}
}
