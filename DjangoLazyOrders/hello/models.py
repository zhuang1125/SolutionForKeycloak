from django.db import models

# Create your models here.
class Question(models.Model):
    question_text = models.CharField(max_length=200)
    pub_date = models.DateTimeField('date published')
    def __str__(self):
        return self.question_text


class Choice(models.Model):
    question = models.ForeignKey(Question, on_delete=models.CASCADE)
    choice_text = models.CharField(max_length=200)
    votes = models.IntegerField(default=0)
    def __str__(self):
        return self.choice_text

#类别表
class Category(models.Model):
    category_id = models.AutoField(primary_key=True,verbose_name='类别ID')
    category_name = models.CharField(max_length=30,verbose_name='类别名')
    def __str__(self):
        return self.category_name
    class Meta:
        verbose_name_plural = '类别表'


#菜单表
class Menu(models.Model):
    menu_id = models.AutoField(primary_key=True,verbose_name='菜单ID')
    category = models.ForeignKey(Category,on_delete=models.CASCADE)
    menu_name = models.CharField(max_length=50,verbose_name='菜单名')
    img_path = models.CharField(max_length=100,verbose_name='图片路径',default='')
    img_file = models.ImageField(upload_to='images/')
    price = models.DecimalField(max_digits=5, decimal_places=2)
    def __str__(self):
        return self.menu_name
    class Meta:
        verbose_name_plural = '菜单'


#购物车
class Carts(models.Model):
    cart_id = models.AutoField(primary_key=True,verbose_name='购物车ID')
    open_id = models.CharField(max_length=50,verbose_name='')
    menu = models.ForeignKey(Menu,on_delete=models.CASCADE,default=0)


#订单表
class Orders(models.Model):
    order_id = models.AutoField(primary_key=True)
    open_id = models.CharField(max_length=50,verbose_name='用户ID')
    is_paid = models.BooleanField('是否已支付')
    address = models.CharField(max_length=100,verbose_name='收货地址')
    order_time = models.DateTimeField(verbose_name='下单时间')
    pay_time = models.DateTimeField('支付时间')
    menus = models.ManyToManyField(Menu)