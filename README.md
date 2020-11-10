# Задача
Требуется реализовать сервис на asp core и консольное приложение, которое вызывает методы на сервисе. 

Есть коллекция пользователей. У каждого пользователя есть ид, имя и статус (New, Active, Blocked, Deleted). Информация о пользователях хранится в БД MySql.

При запуске сервиса нужно загрузить информацию из БД, затем раз в 10 минут обновлять данные. Считаем, что запросов на получение данных много, а изменение данных происходит редко, поэтому при запросе UserInfo нужно брать данные из памяти а не делать запрос в БД.


Все исключения в сервисе должны обрабатываться в коде.


Методы, в путях которых есть /auth/ должны использовать механизм Basic Authorization.


# Описание методов
### Создание пользователя.
Метод **CreateUser**
* Url: `/auth/CreateUser`
* Request: `POST`

Входные параметры — XML вида:
```xml
<Request>
 <user Id="999" Name="alex">
 <Status>New</Status>
 </user>
</Request>
```
Ответ также в формате XML:
```xml
<Response Success="true" ErrorId="0">
 <user Id="999" Name="alex">
 <Status>New</Status>
 </user>
</Response>
```
Пример ответа при ошибке:
```xml
<Response Success="false" ErrorId="1">
 <ErrorMsg>User with id 99 already exist</ErrorMsg>
</Response>
```
### Удаление пользователя.
Метод **RemoveUser**
* Request: `POST`
* Url: `/auth/RemoveUser`


Входные параметры — JSON вида:
```json
{"RemoveUser":{"Id":999}}
```
Ответ:
успех: 
```json
{"Msg": "User was removed","Success": true,"user":{"Status": "Deleted","Id": 999,"Name": 
"alex"}}
```
Ошибка:
```json
{"ErrorId": 2,"Msg": "User not found","Success": false}
```
### Информация о пользователе
Метод **UserInfo**
* Request: `GET`
* Url: `/public/UserInfo?id=999`

### Изменения статуса пользователя
Метод **SetStatus**
* Request: `POST`
* Url: `/auth/SetStatus`
* Content-Type: `application/x-www-form-urlencoded`


Параметры:
```
Id int <Ид пользователя>
NewStatus string <Статус пользователя>
```
Формат ответа JSON
```json
{"Success": true,"user":{"Status": "Deleted","Id": 999,"Name": 
"alex"}}
```