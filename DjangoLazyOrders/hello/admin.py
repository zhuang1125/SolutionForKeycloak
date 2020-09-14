from django.contrib import admin

# Register your models here.

from .models import Question,Choice,Category,Menu,Carts,Orders

admin.site.register(Question)
admin.site.register(Choice)
admin.site.register(Category)
admin.site.register(Menu)
admin.site.register(Carts)
admin.site.register(Orders)