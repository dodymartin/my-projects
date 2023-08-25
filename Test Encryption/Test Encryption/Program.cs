// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using Test_Encryption;

var key = "reyal";
var pwd = "fdsadfsaf asasdfsadf sadfsafd asdfsadf sadf asdfsadf asdf ";
var encPwd = Encryptor2.EncryptData(pwd, key);
encPwd = "Zm8BGW6Ejqk8STVPp5Cdnw==";
Console.WriteLine($"encPwd = {encPwd}");
Console.WriteLine($"decPwd = {Encryptor2.DecryptData(encPwd, key)}");
Console.WriteLine($"pwd    = {pwd}");

Console.ReadKey();
