from django.shortcuts import render

# Create your views here.
from django.http import HttpResponse

# Create your views here.
def Demo(request):
    return HttpResponse("hello world!");

def test(request):
    return HttpResponse("test")