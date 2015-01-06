Dark Energy
==========
Dark Energy is a turn-based role-playing game (RPG) designed for mobile devices. It allows the player to assume
the role of a heroic character and explore a virtual world based in a high fantasy setting.
##Architecture
###Vision
The core architecture of this project is designed to be easy to maintain and highly efficient. It provides the
developer with a high-performance game design framework that employs an intuitive programming interface
centered on a variety of well-known design patterns.
###Technology
Dark Energy is written in C# with XAML in Visual Studio 2013. The SharpDX[3] framework is used in order to
access the power of DirectX technology. Data that loads at runtime is stored in XML files.
###Justification
####C Sharp
C# was chosen due to its maintainability and object-oriented approach to programming. It is preferred over
Java and Objective-C due to its elegant implementation of lambda functions, anonymous types and LINQ, the
existence of properties, the power to overwrite operators and the fact that events are a built-in language
feature.
A popular argument against using C# is that programs written in it are limited to operating systems developed
by Microsoft. As the past few years have seen the rise of software that can compile C# code into native iOS
and Android applications, this is no longer the case.
####Reflection
Reflection is thoroughly used throughout the project. This provides a beneficial trade-off between
computational speed and maintainability. Developer productivity and code simplicity is increased as objects can
be loaded and instantiated at runtime by merely passing an ID through a method (see DataManager).
XML
Regarding content management, the decision to store data in the XML format as opposed to JSON was
ultimately influenced by maintainability. XML files are easier to interpret by humans and the slightly larger space
they occupy is computationally negligible.
####Windows Phone
Lastly, from a financial perspective, targeting the Windows Phone market – one that currently lacks turn-based
role-playing games of this particular kind and complexity, is a viable strategy to establish early popularity.
###Programming Patterns
####Component-Oriented Programming
Classes formed of multiple domains without coupling them to one another. The GameSystem class exists to
fulfill this purpose.Factory Method
Many entities such as characters, items and abilities are created at runtime from external files by simply passing
an ID to designated managers.
####Observer
Event-driven programming. Most entities have listeners that let the framework know when their properties
have altered.
####State
Certain entities, such as characters, have a set of states that they can adopt. This enables an object to elegantly
alter its behavior without resorting to massive conditional statements.
####Type
Similar entities share the same behavior without having to designate any separate classes. For example, all
enemies use a single class called Enemy.
