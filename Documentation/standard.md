# Open-CodeChems coding standard

## Introduction
As software enginneering students we believe that quality in software is important, so that   This document is a guide to unify the Csharp (C#) source code into  the team, it is based on the micfosoft c# oficial standard. Here will be showed conventions, rules and formats to be followed by the team on the development of Open-Codechems game.0

## Purpose
This document aims comunication between developers to be efective. Unifiying the coding style pretends to improve the fact of understand when reading coding that was written by another member of the team. Using this standard we want to implement proved coding practices that makes a code secure, trustable, and maintainable.
## Tencnologies to use
* To create user interfaces we are gonna be using Godot in his mono version.
* As a c# implementation we are gonna be working on .NetCore 6.0.10
* To net cominication grpc is gonna be used
* The database is an instance of SQLServer hosted in Azure

## Naming conventions
* Use descriptive names that are relationed with their function. (Better having long and descriptive names than short ones and not undestanables)
* Use pascal casing ("PascalCasing") when naming a class or public members of types, such as fields, properties, events, methods, and local functions, use pascal casing.
```cs
public class DataService
{
}
```

* When naming an interface, use pascal casing in addition to prefixing the name with an I. This clearly indicates to consumers that it's an interface.

```cs
public interface IWorkerQueue
{
}
```

* Use camel casing ("camelCasing") when naming private or internal fields.

```cs
public class DataService
{
    private IWorkerQueue workerQueue;
}
```

* Use public static fields for change parameters to other scenes.

``` cs
public class DataService
{
    private static IWorkerQueue workerQueue;
}
```
``` cs
public class Network
{
    private IWorkerQueue workerQueue = DataService.workerQueue;
}
```

* When writting method parameters, use camel casing.

``` cs
public void ClearTable()
{
}
```
* When writting constants fields, use Uper camel case and use underscores.
``` cs
public constant EMAIL_FOR_ACTUAL_PLAYER = 0;
```
* When create nodes of Godot, use as suffix the type of node.
``` gd
EditProfileNinePatchRect
```

* The principal node will be naming as the scene
``` gd
EditProfile.tscn
Principal node: EditProfile
```

* When create events from Godot to C#, this will be naming with the prefix _ and start with lowercase
``` cs
public void _on_CancelTextureButton_pressed()
{
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
}
```

## Comennting conventions
* Place the comment on a separate line, not at the end of a line of code.

* Begin comment text with an uppercase letter.
* End comment text with a period.
* Don't create formatted blocks of asterisks around comments.

``` cs
// The following declaration creates a query. It does not run
// the query.
```
## Documentation of code
* For the documentation of code will be use XML, specifying the parameters, the return and a description of the method
* Only use this comments when the method is confusing
``` cs
/// <summary>
/// check if exist a register with the hash password and the username
/// </summary>
/// <param name = "username"> receives an string with the username of the user </param>
/// <param name = "hashPassword"> receives an string with the password of the user </param>
/// <returns> boolean with true value if exist a register with the password </returns>
```

Insert one space between the comment delimiter (//) and the comment text, as shown in the following example.
## Layout conventions
* As working on diferent text editor due to having different operative systems we created a configuration file, click [here](https://github.com/JosepHyv/Open-CodeChems/blob/main/.editorconfig) to watch it.

## try-catch
* Use a try-catch statement for most exception handling.
```cs
static string GetValueFromArray(string[] profiles, int index)
{
    try
    {
        return profiles[index];
    }
    catch (System.IndexOutOfRangeException ex)
    {
        Console.WriteLine("Index is out of range: {0}", index);
        throw;
    }
}
```

## new operator
* Use one of the concise forms of object instantiation, as shown in the following declarations. 

```cs
var profile = new Profile();
```
* Use object initializers to simplify object creation, as shown in the following example.
````cs
var profile = new Profile { Nickname = "Carsi", Victories = 0; Defeats = 0 };
```
* All operator will be initialized

```cs
int reportes = 0;
string name = "";
bool statusRegister = "";
```
* One variable declaration is made per line

```cs
int idUsuario = 0;
string pathImageProfile = "";
```

## Style of code
*   For the indetation all code have the identation of the a tab.

```cs
public void RegisterUser()
{
    bool statusOfRegister = false;
}
```
* The use of keys will be down of the method
```cs
public string getUsername()
{

}
```

## Control structures
* In each of this structures will be use spaces between parentheses and condition

```cs
var profile = new Profile();
```

## Operators && and ||
* To avoid exceptions and increase performance by omitting unnecessary comparisons, use && instead of & and || instead of | when performing comparisons, as shown in the example below.
``` cs
if ((divisor != 0) && (dividend / divisor > 0))
{
    Console.WriteLine("Quotient: {0}", dividend / divisor);
}
else
{
    Console.WriteLine("Attempted division by 0 ends up here.");
}
```
# Security
* For reforzed the security all password will be encrypted with SHA256

```cs
Encryption PasswordHasher = new Encryption();
string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
```
