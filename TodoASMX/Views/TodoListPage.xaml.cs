using System;
using System.Web.Services.Protocols;
using Xamarin.Forms;

namespace TodoASMX
{
    public partial class TodoListPage : ContentPage
    {

        public TodoListPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var res = await App.TodoManager.GetTodoItemsAsync();
            if (res.error == null)
            {
                listView.ItemsSource = res.res;
            }
            else
            {
                if(res.error is SoapException)
                {
                  await  DisplayAlert("Error", ((SoapException)res.error).Detail.InnerText, "ok");
                }
                else
                {
                    await DisplayAlert("Error", res.error.Message, "ok");
                }
                
            }
        }

        async void OnAddItemClicked(object sender, EventArgs e)
        {
            var todoItem = new TodoItem()
            {
                ID = Guid.NewGuid().ToString()
            };
            var todoPage = new TodoItemPage(true);
            todoPage.BindingContext = todoItem;
            await Navigation.PushAsync(todoPage);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var todoItem = e.SelectedItem as TodoItem;
            var todoPage = new TodoItemPage();
            todoPage.BindingContext = todoItem;
            await Navigation.PushAsync(todoPage);
        }
    }
}
