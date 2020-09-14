from django import forms
from django.forms import ModelForm
from hello.models import Category

class EditCategoryForm(ModelForm):
    category_name = forms.CharField(label='类别名:',max_length=10,widget=forms.TextInput({
                                   'class': 'form-control',
                                   'placeholder': '类别名'}))
    class Meta:
        model = Category
        #fields = ['category_name']
        fields = '__all__'