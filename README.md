# COMP2001 Coursework

This repository is for my Comp2001 assessment 2 API.
It contains the API for my profiles microservice and the SQL files used to create the database.

### API
The [API](https://web.socem.plymouth.ac.uk/COMP2001/BSanderswyatt/) follows the RESTful API principles and is developed in ASP.net framework. It allows for all CRUD operations and uses [swagger UI](https://web.socem.plymouth.ac.uk/COMP2001/BSanderswyatt/swagger/index.html) to visualise the interations with the API

#### Goal
This microservices aim is to provide endpoints which can be used to create, read, update and delete users profiles while also allowing the viewing of other user’s profiles.

### Endpoints
#### Accounts

```http
POST api/account/login
```

<img src="SwaggerImages/Account-Login.png">


```http
POST api/account/register
```

<img src="SwaggerImages/Account-Register.png">

#### Admin

```http
POST api/admin/archive
```

<img src="SwaggerImages/Admin-Archive.png">

```http
POST api/admin/unarchive
```

<img src="SwaggerImages/Admin-Unarchive.png">

#### Users

```http
GET api/user/get-user
```

<img src="SwaggerImages/User-GetUser.png">

```http
PUT api/user/update
```
<img src="SwaggerImages/User-Update.png">

```http
DELETE api/user/delete
```
<img src="SwaggerImages/User-Delete.png">

### SQL files
The SQL files were used to create my database which was hosted by the university.
