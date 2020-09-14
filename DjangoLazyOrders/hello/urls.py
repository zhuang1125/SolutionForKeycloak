class urls(object):
    """description of class"""


from django.urls import path,include
from hello import views

urlpatterns = [path('hello/',views.hello,name='hello'),
               path('test/',views.test,name='test'),
               path('testjson/',views.testjson,name='testjson'),
               path('testjson2/',views.testjson2,name='testjson2'),
               path('lazy_orders_index/',views.lazy_orders_index,name='lazy_orders_index'),
               path('category_add/',views.category_add,name='category_add'),
               path('<int:category_id>/category/', include([path('delete/', views.category_delete,name='category_delete'),
                                                            path('edit/', views.category_edit,name='category_edit')]))
               ]