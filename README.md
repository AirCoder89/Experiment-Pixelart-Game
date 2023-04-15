# Pixelart Game [Unity Prototype]

The architecture provides a root entry point for creating and manipulating systems, holding and creating a map of all the systems, providing API to control the systems and manage dependencies, distributing all the necessary models, and enabling full control of the code flow and each part individually.

**Watch a demo on youtube**
[![CLICK HERE](https://user-images.githubusercontent.com/62396712/110776479-66e9c600-8260-11eb-931a-21947d7ec591.PNG)](https://www.youtube.com/watch?v=jgJFXI0sbnY)

---

The image bellow shows the principle elements and the relationship among them.

![1](https://user-images.githubusercontent.com/62396712/110773737-58e67600-825d-11eb-8bd5-1d5909f88925.png)


- **Application**: A root and a single entry point to create and manipulate systems.

- **Controller**: Hold and create a map for all systems.

- **GameSystem** : An Abstract class responsible of :

       * Inject dependency through constructors.
       * Manage all related "GameComponent" and "GameView" instances.
       * Distribute all the necessary models.

- **Model**: "ScriptableObject" sub-classes that hold the data and the configurations for all the game parts.

- **SystemFacade**: Systems’ API & Dependencies container.

- **GameView**:  Entity that represents actors in the unity space through a "GameObject" and creates a map for the attached components.

- **GameComponent**: Reusable modules that are attached to “GameView” (entities) and contain all the behaviors.

---
An important part in this architecture is the "Facade Design Pattern", it takes charge of providing API to control systems or even play the role of the dependencies container and distribute systems instances.


![2](https://user-images.githubusercontent.com/62396712/110774291-ef1a9c00-825d-11eb-83b2-f894513c170a.png)

---

We have a full control of the code flow and we can control each part individually. 
This is an example to show how Tick function is called.

![3](https://user-images.githubusercontent.com/62396712/110775068-cba42100-825e-11eb-8c43-c23406627542.png)


---

System and Component accessing is very handy and easy through generic methods.

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

The following diagram shows GameView’s hierarchy and all its related components.

![8](https://user-images.githubusercontent.com/62396712/110776025-e2974300-825f-11eb-828d-e8e2becd4a22.png)

---

This diagram represents the responsible systems for each "GameComponent".


![9](https://user-images.githubusercontent.com/62396712/110776115-fb075d80-825f-11eb-8c81-136cbdc91919.png)


---

A few behaviors examples:

![10](https://user-images.githubusercontent.com/62396712/110776155-065a8900-8260-11eb-9d0b-3d49bf31cb50.png)
![11](https://user-images.githubusercontent.com/62396712/110776159-078bb600-8260-11eb-9403-6a7d69bb3ecd.png)
![12](https://user-images.githubusercontent.com/62396712/110776167-09557980-8260-11eb-846b-8a068c9573ee.png)

---

The enemy AI state machine with a basics states.


![13](https://user-images.githubusercontent.com/62396712/110776220-16726880-8260-11eb-88a9-ab3d7f4ed27b.png)


