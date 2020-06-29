# Buy&Run
**Buy&Run - an e-auction built on the ASP.NET Core web API.**

**[Working project demo video](https://youtu.be/s5uEG6LZPYQ)**

![main page](https://res.cloudinary.com/dlnyphsj6/image/upload/v1593423466/samples/Screenshot_at_Jun_28_21-31-51_bxv7uc.png)

**The project is composed by the following layers:**

**DAL** (*Data Access Layer*) - data access level. Contains models and classes for interacting with the MSSQL database.

**BLL** (*Business Logic Layer*) - contains main business logic

**API** - controllers

**Identity server** - a separate authentication server built on Identity server 4.

**Frontend** server built on Angular 9

- Using MSSQL database
- Images of lots and user avatars are uploaded to the [Cloudinary](https://cloudinary.com).
- The user password and other data from the forms are validated on the front end side and on the back end.
- There are different user access levels for different roles (*admin, moderator, regular user*).
- Implemented pagination on the backend side.
