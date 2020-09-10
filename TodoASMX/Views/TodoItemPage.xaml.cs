using System;
using System.Web.Services.Protocols;
using Xamarin.Forms;

namespace TodoASMX
{
    public partial class TodoItemPage : ContentPage
    {
        bool isNewItem;

        public TodoItemPage(bool isNew = false)
        {
            InitializeComponent();
            isNewItem = isNew;
        }

        async void OnSaveActivated(object sender, EventArgs e)
        {
            var todoItem = (TodoItem)BindingContext;
            var resdata = await App.TodoManager.SaveTodoItemAsync(todoItem, isNewItem);
            if (resdata.error is null)
            {
                await Navigation.PopAsync();
            }
            else
            {
                if (resdata.error is SoapException)
                {
                   
                    await DisplayAlert("Error", ((SoapException)resdata.error).Message, "ok");
                    await DisplayAlert("Error", ((SoapException)resdata.error).Detail.InnerText, "ok");
                }
                else
                {
                    await DisplayAlert("Error", resdata.error.Message, "ok");
                }

            }


        }

        async void OnDeleteActivated(object sender, EventArgs e)
        {
            var todoItem = (TodoItem)BindingContext;
            var resdata = await App.TodoManager.DeleteTodoItemAsync(todoItem);

            if (resdata.error is null)
            {
                await Navigation.PopAsync();
            }
            else
            {
                if (resdata.error is SoapException)
                {
                    await DisplayAlert("Error", ((SoapException)resdata.error).Detail.InnerText, "ok");
                }
                else
                {
                    await DisplayAlert("Error", resdata.error.Message, "ok");
                }

            }
           
        }

        async void OnCancelActivated(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
