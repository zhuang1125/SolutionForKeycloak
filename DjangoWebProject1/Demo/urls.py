from django.urls import path
from Demo import views

urlpatterns = [path('Demo/',views.Demo,name='Demo'),
               path('test/',views.test,name='test')]