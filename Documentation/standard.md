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

* Use camel casing ("camelCasing") when naming private or internal fields, and prefix them with _.

```cs
public class DataService
{
    private IWorkerQueue _workerQueue;
}
```

* When working with static fields that are private or internal, use the s_ prefix and for thread static use t_.

``` cs
public class DataService
{
    private static IWorkerQueue s_workerQueue;

    [ThreadStatic]
    private static TimeSpan t_timeSpan;
}
```

* When writing method parameters, use camel casing.

``` cs
public T SomeMethod<T>(int someNumber, bool isValid)
{
}
```

## Layout conventions
As working on diferent text editor due to having different operative systems we created a configuration file, click [here](https://github.com/JosepHyv/Open-CodeChems/blob/main/.editorconfig) to watch it.

## try-catch