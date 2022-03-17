

  <h3 align="center">RISE ASSESSMENT</h3>

    BAŞLAMA TARİHİ :14.07.2022
    BİTİŞ TARİHİ : 17.03.2022


# Proje Hakkında


Birbirleri ile haberleşen minimum iki microservice'in olduğu bir yapı tasarlayarak, basit 
bir telefon rehberi uygulaması oluşturulması sağlanacaktır.

Beklenen işlevler:
• Rehberde kişi oluşturma
• Rehberde kişi kaldırma
• Rehberdeki kişiye iletişim bilgisi ekleme
• Rehberdeki kişiden iletişim bilgisi kaldırma
• Rehberdeki kişilerin listelenmesi
• Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin 
getirilmesi
• Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor 
talebi
• Sistemin oluşturduğu raporların listelenmesi
• Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi


### **Kullanılan Teknolojiler**

* [ASP.NET.CORE 5]
* [POSTGRESQL]
* [MONGODB]
* [RABBITMQ]


### **Kullanılan Kütüphaneler**

- **Swashbuckle** -  RestApi'lere arayüz sağlamak sağlamak için
- **Npgsql.EntityFrameworkCore.PostgreSQL** - EntityFrameworkCore ile postgre kullanmak için
- **EFCore.NamingConventions** - Postgresql'in snake case isimlendirmesini camelcase isimlendirmeye dönüştürmek için
- **DotNetCore.CAP**  -DbTransaction'nın başarılı olması halinde kaydedilen eventların kuyruğa iletmesini yönetmek için,
- **refit** - Hızlı ve pratik bir şekilde httpclient'lar oluşturmak için
- **RabbitMQ.Client** - Rabbitmq yönetimi için
- **Autofac** - IoC kapsayıcı olarak


<!-- GETTING STARTED -->

### **Ön Koşullar**

.net 5 SDK,
MongoDB 4.4.1 veya üstü
PostgreSQL 13.2  veya üstü

### Kurulum

Git'den proje çekildikten sonra yapılması gerekenler

**Contacting**
1. Contacting.API projesinin altındaki appsettings dosyaları içindeki ConnectionString değerine veritabanı connection bilgileri ve EventBusConnection değerine rabbitmq connection bilgileri yazılmalı

**Reporting**
1. Reporting.API projesinin altındaki appsettings dosyaları içindeki ConnectionString değerine veritabanı connection bilgileri ve EventBusConnection değerine rabbitmq connection bilgileri yazılmalı

2. MongoDb configurasyon dosyasında aşağıdaki replikasyon ayarı yapılmalı  
Not* Transaction işlemleri için gerekli
> replication:
   replSetName: "kumeAdi"
   
    ardından mongodb consolundan aşağıdaki komut çalıştırılmalı.
    >  rs.initiate()






