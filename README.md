# Pixelart Game [Unity Prototype]

<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/628d98c6-0224-48a2-b3e3-321b5f48e681" alt="InspectMe Logo" width="100"></td>
    <td>
      🛠️ Boost your Unity workflows with <a href="https://divinitycodes.de/">InspectMe</a>! Our tool simplifies debugging with an intuitive tree view. Check it out! 👉 
      <a href="https://assetstore.unity.com/packages/tools/utilities/inspectme-lite-advanced-debugging-code-clarity-283366">InspectMe Lite</a> - 
      <a href="https://assetstore.unity.com/packages/tools/utilities/inspectme-pro-advanced-debugging-code-clarity-256329">InspectMe Pro</a>
    </td>
  </tr>
</table>

---

The architecture provides a root entry point for creating and manipulating systems, holding and creating a map of all the systems, providing API to control the systems and manage dependencies, distributing all the necessary models, and enabling full control of the code flow and each part individually.

**Watch a demo on youtube**
[![CLICK HERE](https://user-images.githubusercontent.com/62396712/110776479-66e9c600-8260-11eb-931a-21947d7ec591.PNG)](https://www.youtube.com/watch?v=jgJFXI0sbnY)

---

The image bellow shows the principle elements and the relationship among them.

![1](https://user-images.githubusercontent.com/62396712/110773737-58e67600-825d-11eb-8bd5-1d5909f88925.png)


- **Application**: This is the root and single entry point for creating and manipulating systems in the architecture. It provides a central location for managing the different parts of the game.

- **Controller**: responsible for holding and creating a map of all the systems in the game. It provides an interface for accessing and managing these systems.

- **GameSystem** : An abstract class that manages all the related GameComponent and GameView instances in the game. It is responsible for injecting dependencies through constructors, managing the game models, and distributing the necessary data to other parts of the game.

- **Model**: These are ScriptableObject sub-classes that hold data and configurations for all the different parts of the game. They provide a flexible and reusable way of storing game data.

- **SystemFacade**: acts as a dependencies container, providing an API for accessing and managing the different systems in the game. It also provides a central location for handling dependencies between different parts of the game.

- **GameView**: is an entity that represents actors in the Unity space through a GameObject. It creates a map of the attached components and provides a way of accessing and manipulating them.

- **GameComponent**: These are reusable modules that are attached to GameView entities and contain all the different behaviors for that entity. They provide a way of adding functionality to the game without creating new entities or systems.

---
In this architecture, the "Facade Design Pattern" plays a crucial role by serving as an interface to provide an API for controlling systems and acting as the dependencies container. By encapsulating the complexity of the underlying system and exposing a simplified interface, the facade design pattern simplifies the client code's interaction with the system. Additionally, it provides a unified interface to access the subsystem's functionality, which can be utilized to create high-level functionality that builds on the subsystems.


![2](https://user-images.githubusercontent.com/62396712/110774291-ef1a9c00-825d-11eb-83b2-f894513c170a.png)

---

we can have complete control over the code flow of the game and each individual part. For example, the "Tick" function, which is responsible for updating the state of the game, can be called at the appropriate time to ensure smooth gameplay. This level of control allows for greater flexibility and precision in game development, resulting in a more polished and optimized final product.

![3](https://user-images.githubusercontent.com/62396712/110775068-cba42100-825e-11eb-8c43-c23406627542.png)


---

Accessing systems and components is made simple and convenient through the use of generic methods.

![4](https://user-images.githubusercontent.com/62396712/110775168-e8405900-825e-11eb-9ff7-ebf1da5e607a.png)

```cs
public T GetSystem<T> where T : GameSystem
{
     if(!_systems.ContainsKey(typeof(T)) return null;
     return (T) _systems[typeof(T)];
}
```


![6](https://user-images.githubusercontent.com/62396712/110775198-ee363a00-825e-11eb-8782-cf733ea652f4.png)

```cs
public T GetComponent<T> where T : GameComponent
{
     if(!HasComponent<T>()) return null;
     var component = Components[typeof(T)] as T;
     return component;
}
```

---

This diagram provides an overview of the GameView hierarchy and its related components. It shows how the GameView entity represents actors in the Unity space through a GameObject and creates a map for the attached components.

![8](https://user-images.githubusercontent.com/62396712/110776025-e2974300-825f-11eb-828d-e8e2becd4a22.png)

---

This diagram provides an overview of the relationship between game components and systems. It shows which systems are responsible for managing each game component, providing a clear picture of the dependencies between the various parts of the architecture.


![9](https://user-images.githubusercontent.com/62396712/110776115-fb075d80-825f-11eb-8c81-136cbdc91919.png)


---

A few behaviors examples:

![10](https://user-images.githubusercontent.com/62396712/110776155-065a8900-8260-11eb-9d0b-3d49bf31cb50.png)
![11](https://user-images.githubusercontent.com/62396712/110776159-078bb600-8260-11eb-9403-6a7d69bb3ecd.png)
![12](https://user-images.githubusercontent.com/62396712/110776167-09557980-8260-11eb-846b-8a068c9573ee.png)

---

The enemy AI state machine with a basics states.


![13](https://user-images.githubusercontent.com/62396712/110776220-16726880-8260-11eb-88a9-ab3d7f4ed27b.png)

---

<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/628d98c6-0224-48a2-b3e3-321b5f48e681" alt="InspectMe Logo" width="100"></td>
    <td>
      🛠️ Boost your Unity workflows with <a href="https://divinitycodes.de/">InspectMe</a>! Our tool simplifies debugging with an intuitive tree view. Check it out! 👉 
      <a href="https://assetstore.unity.com/packages/tools/utilities/inspectme-lite-advanced-debugging-code-clarity-283366">InspectMe Lite</a> - 
      <a href="https://assetstore.unity.com/packages/tools/utilities/inspectme-pro-advanced-debugging-code-clarity-256329">InspectMe Pro</a>
    </td>
  </tr>
</table>
