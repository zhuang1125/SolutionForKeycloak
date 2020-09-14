import json
from datetime import datetime
from django.core import serializers
from django.shortcuts import render
from django.http import HttpResponse,JsonResponse
from hello.models import Category,Menu
from hello.forms import EditCategoryForm
from django.shortcuts import get_object_or_404 
# Create your views here.

def hello(request):
    return HttpResponse("hello world!");

def test(request):
    return HttpResponse("test")

def testjson(request):
    data = {
    'name': 'dengwei',
    'sex': '男'
    }

    return HttpResponse(json.dumps(data),content_type="application/json")

def testjson2(request):
    #data = {
    #'name': 'dengwei',
    #'sex': '男'
    #}

    data = [{
    'name': 'test01',
    'sex': '男'
    },{
    'name': 'test02',
    'sex': '女'
    }]
    
    
    return JsonResponse(data,safe=False)



def category_edit(request,category_id):
    return HttpResponse("edit")

def lazy_orders_index(request):
    categorys = Category.objects.all()#查询所有的类别
    menus = Menu.objects.all()#查询所有的菜单

    json_categorys = []#定义两个数组 用来保存model的json数据
    json_menus = []

    for category in categorys:
        json_categorys.append({
        "category_id":category.category_id,
         "category_name":category.category_name#遍历实体对象 构建ViewModel的数据
         })

    for menu in menus:
        json_menus.append({
            "category_name":menu.category.category_name,
            "menu_name":menu.menu_name
            })

    context = {'json_categorys': json.dumps(json_categorys),'json_menus':json.dumps(json_menus),'categorys':categorys,'menus':menus,'year':datetime.now().year}
    return render(request,'hello/lazyorders.cshtml',context)#将数据和模板传给render函数 渲染成html返回给客户端

def category_delete(request,category_id):
    Category.objects.get(pk=category_id).delete()
    return JsonResponse({'code':0,'message':'ok'})


def category_add(request):
    if request.method == 'GET':
        form = EditCategoryForm()
        context={'form':form}
        return render(request,'hello/category_add.cshtml',context)
    else:
        form = EditCategoryForm(request.POST)
        if form.is_valid():
            form.save()
        return JsonResponse({'code':0,'message':'ok'})

def category_edit(request,category_id):
    category = get_object_or_404(Category, pk=category_id)
    if request.method == 'GET':
        form = EditCategoryForm(instance=category)
        context = {'form':form,'category_id':category.category_id}
        return render(request,'hello/category_detail.cshtml',context)
    elif request.method == 'POST':
        form = EditCategoryForm(request.POST)
        if form.is_valid():
            category.category_name = form.data['category_name']
            category.save()
        #category.category_name = request.POST['category_name']
        #category.save()
        return JsonResponse({'code':0,'message':'ok'})