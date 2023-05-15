Output :


![React App and 15 more pages - Person 1 - Microsoft​ Edge 15-05-2023 08_51_54](https://github.com/komalnagre/Phone-Authentication/assets/128583348/9fb484db-5b21-4420-88fe-66ed5430cf28)

![React App and 15 more pages - Person 1 - Microsoft​ Edge 15-05-2023 08_52_29](https://github.com/komalnagre/Phone-Authentication/assets/128583348/ed4cdcce-ae7e-4648-9b70-879db6cc2ee2)

Login Application using React , MySQL and .net core web API

Installation needed to run this application:
1. Visual Studio Code for Frontend (React)
2. Visual Studio for Backend (.Net Core Web API)
3. MySQL Workbench (Database Part)

Technologies :
1.React
2..Net Core Web API
3.MySQL

steps to run this application:

For Frontend Part 
1.Create new project in firebase for otp authentication
check here https://codinglatte.com/posts/how-to/how-to-create-a-firebase-project/

2.Copy the details of your firebase web app  as shown below and paste in React App -> firebase.js file

const firebaseConfig = {
    apiKey: "AIzaSyAJzaPI93lsgcf9fniLuvPTg42eqqU1WjE",
    authDomain: "login-9aa48.firebaseapp.com",
    projectId: "login-9aa48",
    storageBucket: "login-9aa48.appspot.com",
    messagingSenderId: "211534932707",
    appId: "1:211534932707:web:b2b0687ad0e634ebfc33d6",
    measurementId: "G-GWNG262W32"
  };

3.Open ReactApp Folder in Visual Studio Code and make changes in firebase.js as shown above.
Import Firebase in Visual Studio Code using below command .

"npm install firebase"

Your frontend part will ready to run now

For Backend Part 
1.Import Datbase file in Microsoft SQL SERVER Management Studio
2.Opem Solution present in WebAPI folder and replace the  the connection string with your database connection string.


Details Steps :

Create a new React project using create-react-app:

npx create-react-app login-app


Install the necessary dependencies:
npm install firebase axios


Create a Firebase account and configure it with your React app. Use the following code to send an OTP to the user's phone number:

import firebase from "firebase/app";
import "firebase/auth";

const app = firebase.initializeApp({
  // Your Firebase config goes here
});

export function sendOTP(phoneNumber) {
  return app.auth().signInWithPhoneNumber(phoneNumber, window.recaptchaVerifier);
}


Create a .NET Core Web API project using Visual Studio:

Create a new project in Visual Studio.

Select ".NET Core" as the project type and "Web API" as the project template.

Add the following NuGet packages: 
MySql.Data, Microsoft.EntityFrameworkCore,
Microsoft.EntityFrameworkCore.Design.



Create a database and a table for storing the phone numbers and OTPs:

CREATE DATABASE login_db;
USE login_db;

CREATE TABLE login (
  id INT NOT NULL AUTO_INCREMENT,
  phone_number VARCHAR(20) NOT NULL,
  otp VARCHAR(10) NOT NULL,
  PRIMARY KEY (id)
);



Create a data model for the login table:
csharp

using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
    }
}



Create a DbContext class for the database:
arduino
Copy code
using LoginAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Data
{
    public class LoginContext : DbContext
    {
        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
        {
        }

        public DbSet<Login> Logins { get; set; }
    }
}
  
  
  
  
  
Create a controller for the login API:

using System;
using System.Linq;
using System.Threading.Tasks;
using LoginAPI.Data;
using LoginAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginContext _context;

        public LoginController(LoginContext context)
        {
            _context = context;
        }

        [HttpPost("sendotp")]
        public async Task<IActionResult> SendOTP(Login login)
        {
            // Generate an OTP and store it in the database
            var otp = GenerateOTP();
            login.OTP = otp;
            _context.Logins.Add(login);
            await _context.SaveChangesAsync();

            // Send the OTP to the user's phone number
            // You can use Twilio or any other SMS provider for this
            // For now, we'll just return the OTP
            return Ok(new { otp });
        }

        [HttpPost("validateotp")]
        public async Task<IActionResult> ValidateOTP(Login login)
        {
            // Check if the entered OTP matches the one in the database
            var result = await _context.Logins
                .Where(l => l.PhoneNumber == login.PhoneNumber && l.OTP == login.OTP)
                .FirstOrDefaultAsync();

            if (result != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        private string GenerateOTP()
        {
            // Generate a 6-digit OTP
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
       
